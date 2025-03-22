using UnityEngine;
using UnityEngine.Events;
using Work.JW.Code.Entities;

namespace Work.ISC._0._Scripts.Objects.Frame
{
    public enum ColliderEnum
    {
        Box,
        Circle
    }

    public abstract class ObjectFrameSO : ScriptableObject, IObjectable
    {
        public UnityEvent OnCollideStartEvent;
        public UnityEvent OnCollideEndEvent;
        
        public string ObjectName;
        public Sprite ObjectImage;
        
        public int ObjectLayer;
        public ColliderEnum colliderType;
        public Vector2 ColliderSize; // if BoxCollider
        public float CircleColliderRadius; // if CircleCollider
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