using LiteEntitySystem;
using UnityEngine;

namespace ScavMP.Shared;

public class BaseExpieController : HumanControllerLogic<PlayerInputPacket, BaseExpie>
{
    private readonly Camera _mainCamera;

    public BaseExpieController(EntityParams entityParams)
        : base(entityParams)
    {
        _mainCamera = Camera.main;
    }

    protected override void OnConstructed() { }

    protected override void Update()
    {
        base.Update();

        if (ControlledEntity == null)
            return;

        // Оптимизон ёпта
        if (EntityManager.IsServer)
        {
            const float maxPlayerDistance = 200f;
            foreach (var otherPlayer in EntityManager.GetEntities<BaseExpie>())
                ServerManager.ToggleSyncGroup(
                    OwnerId,
                    otherPlayer,
                    SyncGroup.SyncGroup1,
                    (otherPlayer.Position - ControlledEntity.Position).sqrMagnitude
                        < maxPlayerDistance * maxPlayerDistance
                );
        }

        // input собираем только на клиенте
        if (!EntityManager.IsClient || !IsLocalControlled)
            return;

        ref var input = ref ModifyPendingInput();
        input.MoveX =
            (Input.GetKey(KeyBinds.GetBind("right")) ? 1f : 0f)
            - (Input.GetKey(KeyBinds.GetBind("left")) ? 1f : 0f);
        input.MoveY =
            (Input.GetKey(KeyBinds.GetBind("up")) ? 1f : 0f)
            - (Input.GetKey(KeyBinds.GetBind("down")) ? 1f : 0f);
        input.Jump = Input.GetKey(KeyBinds.GetBind("jump"));
        input.Crouch = Input.GetKey(KeyBinds.GetBind("down"));
        input.LookPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        ControlledEntity.SetInput(input);
    }
}
