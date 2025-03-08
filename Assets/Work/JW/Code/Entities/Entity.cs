using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Work.JW.Code.Entities
{
    public abstract class Entity : NetworkBehaviour
    {
        public UnityEvent OnHit;
        protected Dictionary<Type, IEntityComponent> _components;

        private void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            
            InitializeCompo();
        }

        public T GetCompo<T>(bool isDerived = false) where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
            {
                return (T)component;
            }
            
            if (isDerived)
                return default(T);
            
            Type subType = _components.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (subType != null)
                return (T)_components[subType];
            
            
            return default(T);
        }

        protected virtual void InitializeCompo()
        {
            GetComponentsInChildren<IEntityComponent>().ToList()
                .ForEach(compo => _components.Add(compo.GetType(), compo));
            _components.Values.ToList().ForEach(compo => compo.Initialize(this));
        }
        
        public virtual void OnDead()
        {
            OnHit?.Invoke();
        }
    }
}