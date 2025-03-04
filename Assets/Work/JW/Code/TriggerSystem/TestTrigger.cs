using UnityEngine;
using Work.HN.Code.Save;
using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    public class TestTrigger : Trigger
    {
        public override void TriggerEvent(Entity entity)
        {
            print("Hit");
        }

        public override void SetData(TriggerData data)
        {
            
        }
    }
}