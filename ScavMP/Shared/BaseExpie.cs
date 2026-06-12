using LiteEntitySystem;
using LiteEntitySystem.Extensions;
using ScavMP.Client;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScavMP.Shared
{
    public class BaseExpie : PawnLogic
    {
        public Body Owner { get; private set; }
        public Vector2 NetworkedMoveDir { get; private set; }

        [SyncVarFlags(SyncFlags.Interpolated | SyncFlags.LagCompensated | SyncFlags.SyncGroup1)]
        private SyncVar<Vector2> _position;

        [SyncVarFlags(SyncFlags.Interpolated | SyncFlags.LagCompensated | SyncFlags.SyncGroup1)]
        private SyncVar<FloatAngle> _rotation;
        public readonly SyncString Name = new();

        public Vector2 Position => _position.InterpolatedValue;
        public float Rotation => _rotation.InterpolatedValue;

        public BaseExpie(EntityParams entityParams)
            : base(entityParams) { }

        protected override void RegisterRPC(ref RPCRegistrator r)
        {
            base.RegisterRPC(ref r);
        }

        public void Attach(Body body)
        {
            Owner = body;
            BodyRegistry.Register(body, this);
        }

        protected override void OnDestroy()
        {
            BodyRegistry.Unregister(Owner);
            GameObject.Destroy(Owner.gameObject);
        }

        protected override void Update()
        {
            base.Update();

            if (!IsServer && !IsLocalControlled)
                return;

            ref readonly var input = ref GetCurrentInput<PlayerInput>();

            // сохраняем для патча
            NetworkedMoveDir = new Vector2(input.MoveX, input.MoveY);

            if (IsServer)
                _position.Value = new Vector2(
                    Owner.transform.position.x,
                    Owner.transform.position.y
                );
        }

        protected override void VisualUpdate()
        {
            Owner.transform.position = _position.InterpolatedValue;
        }

        public void Spawn()
        {
            // ... init other components
        }

        public void Spawn(Vector2 position)
        {
            _position.Value = position;
            Spawn();
        }
    }
}
