using UnityEngine;
using Work.ISC._0._Scripts.Objects.Frame;
using Work.JW.Code.Entities;

namespace Work.ISC._0._Scripts.Objects
{
    [CreateAssetMenu(fileName = "SpikeObjectSO", menuName = "SO/Object/Spike", order = 0)]
    public class SpikeObjectSO : ObjectFrameSO
    {
        public override void InitializeObject() { }

        public override void ObjectAbility(Entity entity)
        {
            entity.OnDead();
        }
    }
}