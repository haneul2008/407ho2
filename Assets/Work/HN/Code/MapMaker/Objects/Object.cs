using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.MapMaker.Core;
using Work.ISC._0._Scripts.Objects.Frame;

namespace Work.HN.Code.MapMaker.Objects
{
    public abstract class Object : MonoBehaviour, ISpawnable
    {
        [field: SerializeField] public int ID { get; protected set; }

        public abstract void Spawn();
    }
}