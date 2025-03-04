using System;
using DG.Tweening;
using UnityEngine;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.HN.Code.Save;
using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    public class MoveObjTrigger : Trigger
    {
        [SerializeField] private Vector2 toPos;
        private float _duration = 0.5f;
        
        public override void TriggerEvent(Entity entity)
        {
            if (_targets == null) return;
            if(_isTrigger) return;
            
            MovePosition(entity);
        }

        public override void SetData(TriggerData data)
        {
            MoveInfo info = data.moveInfo;
            
            toPos = info.moveAmount;
            _duration = info.duration;
        }

        public void MovePosition(Entity entity)
        {
            _isTrigger = true;
            
            foreach (var item in _targets)
            {
                Vector3 movePos = item.position + (Vector3)toPos;
                item.DOMove(movePos, _duration).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    _isTrigger = false;
                    base.TriggerEvent(entity);
                });
            }
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(toPos, Vector2.one);
        }
#endif
    }
}