using UnityEngine;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.JW.Code.TriggerSystem;

namespace Work.JW.Code.MapLoad
{
    [CreateAssetMenu(fileName = "TriggerSO", menuName = "SO/Map/TriggerData", order = 0)]
    public class TriggerDataSO : ScriptableObject
    {
        public TriggerType type;
        public string triggerName;
    }
}