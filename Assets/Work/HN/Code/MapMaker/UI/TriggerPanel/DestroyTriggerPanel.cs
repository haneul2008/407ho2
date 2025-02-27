using UnityEngine;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public class DestroyTriggerPanel : SpawnOrDestroyPanel
    {
        public override TriggerType TriggerType => TriggerType.Destroy;
    }
}