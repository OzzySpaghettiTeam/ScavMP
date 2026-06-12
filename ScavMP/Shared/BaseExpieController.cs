using LiteEntitySystem;
using UnityEngine;

namespace ScavMP.Shared;

public class BaseExpieController : HumanControllerLogic<PlayerInput, BaseExpie>
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

        // SyncGroup логика — остаётся как была
        if (EntityManager.IsServer)
        {
            const float maxPlayerDistance = 35f;
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
        input.EndedJump = Input.GetKeyUp(KeyBinds.GetBind("jump"));
        input.Crouch = Input.GetKey(KeyBinds.GetBind("down"));
        input.LookPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

public struct PlayerInput
{
    public float MoveX;
    public float MoveY;
    public bool Jump;
    public bool EndedJump;
    public bool Crouch;
    public Vector2 LookPos;
}
