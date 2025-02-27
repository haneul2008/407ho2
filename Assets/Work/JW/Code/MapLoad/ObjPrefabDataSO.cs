using System.Collections.Generic;
using UnityEngine;

namespace Work.JW.Code.MapLoad
{
    [CreateAssetMenu(fileName = "ObjPrefabs", menuName = "SO/Data/ObjPrefabs", order = 0)]
    public class ObjPrefabDataSO : ScriptableObject
    {
        public List<MapObjectPrefabAndId> objPrefabAndIds;
    }
}