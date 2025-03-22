using Unity.Netcode.Components;
using UnityEngine;
using Work.JW.Code.Animators;
using Work.JW.Code.Entities;

namespace Code.Entities.Network
{
    public class EntityRendererNetwork : EntityRenderer
    {
        private NetworkAnimator _netAnimCompo;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            
            _netAnimCompo = entity.GetComponentInChildren<NetworkAnimator>();
        }

        public override void SetParam(AnimParamSO param, bool value)
        {
            _netAnimCompo.Animator.SetBool(param.hashValue, value);
        }

        public override void SetParam(AnimParamSO param, int value)
        {
            _netAnimCompo.Animator.SetInteger(param.hashValue, value);
        }

        public override void SetParam(AnimParamSO param, float value)
        {
            _netAnimCompo.Animator.SetFloat(param.hashValue, value);
        }

        public override void SetParam(AnimParamSO param)
        {
            _netAnimCompo.Animator.SetTrigger(param.hashValue);
        }
    }
}