using Unity.Netcode;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Code.Entities.Network
{
    public class EntityMoverNetwork : EntityMover
    {
        private NetworkVariable<float> _moveSpeedNetwork = new NetworkVariable<float>();

        protected override void FixedUpdate()
        {
            if (!_entity.IsOwner) return;

            MoveUpdateServerRpc(_movementX);
        }

        public override void SetMovementX(float xMovement)
        {
            if(!_entity.IsOwner) return;

            base.SetMovementX(xMovement);
        }

        public override void AddForce(Vector2 force)
        {
            if(!_entity.IsOwner) return;

            base.AddForce(force);
        }

        public override void AddJump()
        {
            if(!_entity.IsOwner) return;

            base.AddJump();
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