using UnityEngine;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public class SpawnTriggerPanel : SpawnOrDestroyPanel
    {
        public override TriggerType TriggerType => TriggerType.Spawn;
    }
}