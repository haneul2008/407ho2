using System;
using UnityEngine;
using Work.ISC._0._Scripts.Objects.Frame;
using Work.JW.Code.Entities;

namespace Work.ISC._0._Scripts.Objects
{
    [CreateAssetMenu(fileName = "ArrivalPointObjectSO", menuName = "SO/Object/ArrivalPoint", order = 0)]
    public class ArrivalPointObjectSO : ObjectFrameSO
    {
        public event Action OnClearEvent;
        
        public override void InitializeObject() { }


        public override void ObjectAbility(Entity entity) => OnClearEvent?.Invoke();

    }
}