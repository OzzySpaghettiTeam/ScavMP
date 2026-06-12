using System;
using System.Net;
using System.Net.Sockets;
using BepInEx.Logging;
using LiteEntitySystem;
using LiteEntitySystem.Transport;
using LiteNetLib;
using LiteNetLib.Utils;
using ScavMP.Shared;
using UnityEngine;
using ILogger = LiteEntitySystem.ILogger;
using Random = UnityEngine.Random;

namespace ScavMP.Server;

public class ServerLogic : MonoBehaviour, ILiteNetEventListener
{
    private LiteNetManager _netManager;
    private NetPacketProcessor _packetProcessor;
    public ushort Tick => _serverEntityManager.Tick;
    private ServerEntityManager _serverEntityManager;
    private ulong _typesHash;
    private static Logger _logger;

    void Awake()
    {
        EntityManager.RegisterFieldType<Vector2>(Vector2.Lerp);
        _netManager = new LiteNetManager(this) { AutoRecycle = true };
        _packetProcessor = new NetPacketProcessor();
        _packetProcessor.SubscribeReusable<JoinPacket, LiteNetPeer>(OnJoinReceived);
        var typesMap = new EntityTypesMap<GameEntities>()
            .Register(GameEntities.Player, e => new BaseExpie(e))
            .Register(GameEntities.PlayerController, e => new BaseExpieController(e));
        _typesHash = typesMap.EvaluateEntityClassDataHash();

        _serverEntityManager = new ServerEntityManager(
            typesMap,
            (byte)PacketType.EntitySystem,
            NetworkGeneral.GameFPS,
            ServerSendRate.EqualToFPS
        );
        _serverEntityManager.AddSingleton<UnityPhysicsManager>();

        _netManager.Start(1753);
    }

    static ServerLogic()
    {
        _logger = new Logger(new ManualLogSource("ScavMP.Server"));
        LiteEntitySystem.Logger.LoggerImpl = _logger;
    }

    private void OnDestroy()
    {
        _netManager.Stop();
        _serverEntityManager = null;
    }

    private void Update()
    {
        _netManager.PollEvents();
        _serverEntityManager?.Update();
    }

    private void OnJoinReceived(JoinPacket joinPacket, LiteNetPeer peer)
    {
        Debug.Log("[S] Join packet received: " + joinPacket.UserName);

        if (joinPacket.GameHash != _typesHash)
        {
            Debug.Log("[S] Client has different code");
            peer.Disconnect();
            return;
        }

        var serverPlayer = _serverEntityManager.AddPlayer(new LiteNetLibNetPeer(peer, true));
        var player = _serverEntityManager.AddEntity<BaseExpie>(e =>
        {
            e.Spawn();
            e.Name.Value = joinPacket.UserName;
        });
        _serverEntityManager.AddController<BaseExpieController>(serverPlayer, player);
    }

    void ILiteNetEventListener.OnPeerConnected(LiteNetPeer peer)
    {
        Debug.Log("[S] Player connected: " + peer);
    }

    void ILiteNetEventListener.OnPeerDisconnected(LiteNetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("[S] Player disconnected: " + disconnectInfo.Reason);

        if (peer.Tag != null)
        {
            _serverEntityManager.RemovePlayer((LiteNetLibNetPeer)peer.Tag);
        }
    }

    void ILiteNetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        Debug.Log("[S] NetworkError: " + socketError);
    }

    void ILiteNetEventListener.OnNetworkReceive(
        LiteNetPeer peer,
        NetPacketReader reader,
        DeliveryMethod deliveryMethod
    )
    {
        byte packetType = reader.PeekByte();
        switch ((PacketType)packetType)
        {
            case PacketType.EntitySystem:
                _serverEntityManager.Deserialize(
                    (LiteNetLibNetPeer)peer.Tag,
                    reader.GetRemainingBytesSpan()
                );
                break;

            case PacketType.Serialized:
                reader.GetByte();
                _packetProcessor.ReadAllPackets(reader, peer);
                break;

            default:
                Debug.Log("Unhandled packet: " + packetType);
                break;
        }
    }

    void ILiteNetEventListener.OnNetworkReceiveUnconnected(
        IPEndPoint remoteEndPoint,
        NetPacketReader reader,
        UnconnectedMessageType messageType
    ) { }

    void ILiteNetEventListener.OnNetworkLatencyUpdate(LiteNetPeer peer, int latency) { }

    void ILiteNetEventListener.OnConnectionRequest(LiteConnectionRequest request)
    {
        request.AcceptIfKey("ExampleGame");
    }
}
