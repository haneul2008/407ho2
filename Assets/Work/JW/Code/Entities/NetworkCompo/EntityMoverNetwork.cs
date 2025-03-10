using Unity.Netcode;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Code.Entities.Network
{
    public class EntityMoverNetwork : EntityMover
    {
        private NetworkVariable<float> _networkMovementX = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);
        private float _lastMovementX;
        private bool _lastIsGrounded;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _networkMovementX.OnValueChanged += HandleMoveDirChange;
        }

        private void HandleMoveDirChange(float prevValue, float newValue)
        {
            OnXInput?.Invoke(newValue);
        }

        protected override void FixedUpdate()
        {
            if (!IsOwner) return;
            
            MoveUpdateServerRpc(_networkMovementX.Value);
            
            /*if (!Mathf.Approximately(_networkMovementX.Value, _lastMovementX))
            {
                MoveUpdateServerRpc(_networkMovementX.Value);
                _lastMovementX = _networkMovementX.Value;
            }*/

            GroundCheckServerRpc();
        }

        public override void SetMovementX(float xMovement)
        {
            if (!IsOwner) return;
            
            _networkMovementX.Value = Mathf.Abs(xMovement) > 0 ? xMovement : 0;
        }

        public override void AddForce(Vector2 force)
        {
            AddForceServerRpc(force);
        }

        public override void AddJump()
        {
            if (!IsOwner) return;
            JumpServerRpc();
        }

        public override bool IsGroundDetected()
        {
            return _lastIsGrounded;
        }

        [ServerRpc(RequireOwnership = false)]
        private void MoveUpdateServerRpc(float moveDirX)
        {
            _movementX = moveDirX;
            base.FixedUpdate();
            SyncPositionClientRpc(_rigidCompo.position);
        }

        [ClientRpc]
        private void SyncPositionClientRpc(Vector2 newPosition)
        {
            float distance = Vector2.Distance(_entity.transform.position, newPosition);

            _entity.transform.position = Vector2.Lerp(_entity.transform.position, newPosition, 0.5f);
            
            /*if (distance > 0.5f)
            {
                _entity.transform.position = newPosition;
            }
            else
            {
                _entity.transform.position = Vector2.Lerp(_entity.transform.position, newPosition, 0.5f);
            }*/
        }

        [ServerRpc(RequireOwnership = false)]
        private void GroundCheckServerRpc()
        {
            bool isGround = base.IsGroundDetected();
            if (isGround != _lastIsGrounded)
            {
                _lastIsGrounded = isGround;
                SyncGroundCheckerClientRpc(isGround);
            }
        }

        [ClientRpc]
        private void SyncGroundCheckerClientRpc(bool isGround)
        {
            _lastIsGrounded = isGround;
        }

        [ServerRpc(RequireOwnership = false)]
        private void JumpServerRpc()
        {
            base.AddJump();
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddForceServerRpc(Vector2 force)
        {
            base.AddForce(force);
            SyncPositionClientRpc(_rigidCompo.position);
        }

        [ClientRpc]
        private void SyncPositionAndVelocityClientRpc(Vector2 newVelocity)
        {
            _rigidCompo.linearVelocity = Vector2.Lerp(_rigidCompo.linearVelocity, newVelocity, 0.5f);
        }
    }
}
