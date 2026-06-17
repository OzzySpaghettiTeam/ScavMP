using LiteEntitySystem;
using LiteEntitySystem.Extensions;
using ScavMP.Client;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScavMP.Shared
{
    public class BaseExpie : PawnLogic
    {
        private Body _bodyComponent;
        private GameObject _bodyGameObject;
        private PlayerInputPacket _currentInput;

        [SyncVarFlags(SyncFlags.Interpolated | SyncFlags.LagCompensated | SyncFlags.SyncGroup1)]
        private SyncVar<Vector2> _position;
        public readonly SyncString Name = new();

        public Vector2 Position => _position.InterpolatedValue;

        public BaseExpie(EntityParams entityParams)
            : base(entityParams) { }

        protected override void OnConstructed()
        {
            _bodyGameObject = GameObject.Instantiate(Prefabs.Instance.PlayerPrefab);
            _bodyComponent = _bodyGameObject.GetComponent<Body>();
        }

        protected override void RegisterRPC(ref RPCRegistrator r)
        {
            base.RegisterRPC(ref r);
        }

        protected override void OnDestroy()
        {
            GameObject.Destroy(_bodyGameObject);
        }

        public void SetInput(in PlayerInputPacket input)
        {
            _currentInput = input;
        }

        protected override void Update()
        {
            base.Update();

            if (!IsServer && !IsLocalControlled)
                return;

            if (IsServer)
                _position.Value = new Vector2(
                    _bodyComponent.transform.position.x,
                    _bodyComponent.transform.position.y
                );
        }

        protected override void VisualUpdate()
        {
            _bodyComponent.transform.position = _position.InterpolatedValue;
        }
    }
}
