using System;
using UnityEngine;
using Work.ISC._0._Scripts.Objects.Frame;
using Work.JW.Code.Entities;
using Work.JW.Code.Entities.Player;

namespace Work.ISC._0._Scripts.Objects
{
    [CreateAssetMenu(fileName = "StartPointObjectSO", menuName = "SO/Object/StartPoint", order = 0)]
    public class StartPointObjectSO : ObjectFrameSO
    {
        public event Action<Entity> OnGameStartEvent;
        
        
        public override void InitializeObject()
        {
            Entity player = FindAnyObjectByType<Player>();
            
            OnGameStartEvent?.Invoke(player);
        }
        
        public override void ObjectAbility(Entity entity) { }

    }
}