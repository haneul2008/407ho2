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
        public Entity Player { get; private set; }
        public override void InitializeObject()
        {
            Player = FindAnyObjectByType<Player>();
        }

        public void TargetToPosition(Transform target)
        {
            Player.transform.position = target.position;
        }
        
        public override void ObjectAbility(Entity entity) { }

    }
}