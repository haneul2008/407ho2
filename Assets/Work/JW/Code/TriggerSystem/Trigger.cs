using System;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Serialization;
using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Trigger : MonoBehaviour, ITriggerEvent
    {
        public int TargetID { get; set; } //누굴 조종할지의 ID
        public int? TriggerID { get; set; } //자신의 트리거 ID
        [SerializeField] private Vector2 triggerSize;
        private BoxCollider2D _collider;
        protected Transform[] _targets;
        protected bool _isTrigger;
        
        protected virtual void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = true;
        }

        public virtual void TriggerEvent(Entity entity)
        {
            Destroy(gameObject);
        }

        public virtual void SetTargets(Transform[] targets)
        {
            _targets = targets;
        }

        public void TriggerLineScaleSetting(Vector2 size)
        {
            triggerSize = size;
            _collider.size = triggerSize;
        }


#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireCube(transform.position, triggerSize);
            
            Gizmos.color = Color.white;
        }
#endif
    }
}