using UnityEngine;

namespace ScavMP.Shared;

public enum PacketType : byte
{
    EntitySystem,
    Serialized,
}

public class JoinPacket
{
    public string UserName { get; set; }
    public ulong GameHash { get; set; }
}

public class WorldPacket { }

public struct PlayerInputPacket
{
    public float MoveX;
    public float MoveY;
    public bool Jump;
    public bool Crouch;
    public Vector2 LookPos;
}
