using System.Collections.Generic;
using UnityEngine;

namespace Work.JW.Code.MapLoad
{
    [CreateAssetMenu(fileName = "TriggerPrefabs", menuName = "SO/Data/TriggerPrefabs", order = 0)]
    public class TriggerPrefabDataSO : ScriptableObject
    {
        public List<MapTriggerPrefabAndType> triggerPrefabAndTypes;
    }
}