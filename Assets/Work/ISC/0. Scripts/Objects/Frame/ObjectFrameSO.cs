using UnityEngine;
using UnityEngine.Events;
using Work.JW.Code.Entities;

namespace Work.ISC._0._Scripts.Objects.Frame
{
    public abstract class ObjectFrameSO : ScriptableObject, IObjectable
    {
        public UnityEvent OnCollideStartEvent;
        public UnityEvent OnCollideEndEvent;
        
        public string ObjectName;
        public Sprite ObjectImage;
        
        public int ObjectLayer;
        public Vector2 ColliderSize;
        public Vector2 ColliderOffset;

        public bool isTrigger;
        
        [TextArea] public string description;

        public abstract void ObjectAbility(Entity entity);

        public abstract void InitializeObject();

        public void ObjectUse(Entity entity)
        {
            OnCollideStartEvent?.Invoke();
            ObjectAbility(entity);

            OnCollideEndEvent?.Invoke();
        }
    }
}