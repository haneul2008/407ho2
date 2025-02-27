using UnityEngine;

namespace Work.HN.Code.MapMaker.Objects.Triggers
{
    public enum TriggerType
    {
        ObjectMove,
        Alpha,
        Shake,
        Spawn,
        Destroy,
        CameraMove,
        None
    }
    
    [CreateAssetMenu(menuName = "SO/TriggerData")]
    public class TriggerDataSO : ScriptableObject
    {
        public TriggerType triggerType;
        public Color color;
    }
}