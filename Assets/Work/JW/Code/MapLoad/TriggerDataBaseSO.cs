using System.Collections.Generic;
using UnityEngine;

namespace Work.JW.Code.MapLoad
{
    [CreateAssetMenu(fileName = "TriggerPrefabs", menuName = "SO/Data/TriggerPrefabs", order = 0)]
    public class TriggerDataBaseSO : ScriptableObject
    {
        public List<TriggerDataSO> triggerPrefabAndTypes;
    }
}