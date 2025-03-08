using Unity.Netcode;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Code.Entities.Network
{
    public class EntityMoverNetwork : EntityMover
    {
        private NetworkVariable<float> _moveSpeedNetwork = new NetworkVariable<float>();
        
        // public override bool CanMove { get; set; }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _moveSpeedNetwork.OnValueChanged += HandleMoveSpeedValueChange;
        }

        private void HandleMoveSpeedValueChange(float previousValue, float newValue)
        {
            _moveSpeed = newValue;
        }

        public override float SetMoveSpeed(float speed)
        {
            _moveSpeedNetwork.Value = speed;
            return speed;
        }

        protected override void FixedUpdate()
        {
            if (!_entity.IsOwner) return;

            MoveUpdateServerRpc(_movementX);
        }

        [ServerRpc]
        private void MoveUpdateServerRpc(float moveDirX)
        {
            _movementX = moveDirX;
            base.FixedUpdate();
            SyncPositionClientRpc(_rigidCompo.position);
        }

        [ClientRpc]
        private void SyncPositionClientRpc(Vector2 newPosition)
        {
            float distance = Vector2.Distance(transform.position, newPosition);

            if (distance > 0.3f)
            {
                transform.position = Vector2.Lerp(transform.position, newPosition, 0.5f);
            }
            else
            {
                transform.position = newPosition;
            }
        }
    }
}