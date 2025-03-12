using Unity.Netcode;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Code.Entities.Network
{
    public class EntityMoverNetwork : EntityMover
    {
        private NetworkVariable<float> _networkMovementX =
            new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);
        private NetworkVariable<bool> _networkIsGrounded =
            new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);

        private float _lastMovementX;
        private bool _lastIsGrounded;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _networkMovementX.OnValueChanged += HandleMoveDirChange;
        }

        protected override void FixedUpdate()
        {
            if (!_entity.IsOwner) return;

            _movementX = _networkMovementX.Value;
            base.FixedUpdate();
            
            GroundCheck();
        }

        public override void SetMovementX(float xMovement)
        {
            if (!_entity.IsOwner) return;

            _networkMovementX.Value = Mathf.Abs(xMovement) > 0 ? xMovement : 0;
        }

        public override bool IsGroundDetected()
        {
            return _networkIsGrounded.Value;
        }
        
        private void GroundCheck()
        {
            _networkIsGrounded.Value = base.IsGroundDetected();
        }

        private void HandleMoveDirChange(float prevValue, float newValue)
        {
            OnXInput?.Invoke(newValue);
        }
    }
}