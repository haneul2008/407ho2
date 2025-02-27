using System;
using DG.Tweening;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    public class MoveObjTrigger : Trigger
    {
        [SerializeField] private Vector2 toPos;
        private float _duration = 0.5f;
        
        public override void TriggerEvent(Entity entity)
        {
            MovePosition();
        }

        public void SetData(Vector2 pos, float duration)
        {
            _duration = duration;
            toPos = pos;
        }

        public void MovePosition()
        {
            if (_targets == null) return;
            
            foreach (var item in _targets)
            {
                item.DOMove(toPos, _duration).SetEase(Ease.OutCubic);
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