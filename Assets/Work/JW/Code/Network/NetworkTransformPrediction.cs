using System;
using Unity.Netcode;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.JW.Code.Network
{
    public class NetworkTransformPrediction : NetworkBehaviour, IEntityComponent
    {
        private NetworkVariable<Vector3> _netPosition = 
            new NetworkVariable<Vector3>(writePerm:NetworkVariableWritePermission.Owner);
        private NetworkVariable<Vector3> _netVelocity = 
            new NetworkVariable<Vector3>(writePerm:NetworkVariableWritePermission.Owner);
        
        [SerializeField] private float predictionFactor;
        /*private Vector3 _lastPosition;
        private Vector3 _lastVelocity;*/
        private Entity _entity;
        private EntityMover _mover;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _mover = entity.GetCompo<EntityMover>();
        }
        
        private void FixedUpdate()
        {
            if (IsOwner)
            {
                _netPosition.Value = transform.position;
                _netVelocity.Value = _mover.GetVelocity();
            }
            else
            {
                Vector3 predPosition = _netPosition.Value + (_netVelocity.Value * predictionFactor);
                transform.position = Vector3.Lerp(transform.position, predPosition, Time.fixedDeltaTime * 5);
            }
        }
    }
}