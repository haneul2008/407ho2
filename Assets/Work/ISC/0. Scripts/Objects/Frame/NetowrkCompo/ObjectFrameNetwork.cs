using System.Collections.Generic;
using UnityEngine;
using Work.JW.Code.Entities;
using Work.JW.Code.TriggerSystem;

namespace Work.ISC._0._Scripts.Objects.Frame.NetworkCompo
{
    public class ObjectFrameNetwork : ObjectFrame
    {
        protected override void Start()
        {
            if (_objSO.GetType() == typeof(StartPointObjectNetworkSO))
            {
                StartPointObjectNetworkSO startPointSO = (StartPointObjectNetworkSO)_objSO;
                startPointSO.TargetToPosition(transform);
            }
            else if (_objSO is ChainsawFramSO chainsawData)
            {
                Animator animator = gameObject.AddComponent<Animator>();
                animator.runtimeAnimatorController = chainsawData.animationController;
            }
        }
    }
}
