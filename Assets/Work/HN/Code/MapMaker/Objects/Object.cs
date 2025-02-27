using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.MapMaker.Core;
using Work.ISC._0._Scripts.Objects.Frame;

namespace Work.HN.Code.MapMaker.Objects
{
    public abstract class Object : MonoBehaviour, ISpawnable
    {
        [field: SerializeField] public int ID { get; protected set; }

        private List<int> _triggerID = new List<int>();

        public void AddTriggerID(int id)
        {
            _triggerID.Add(id);
        }
        
        public abstract void Spawn();
    }
}