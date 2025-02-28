using System;
using UnityEngine;
using UnityEngine.Events;
using Work.ISC._0._Scripts.Save.ExelData;

namespace Work.HN.Code.Save
{
    public class InGameLoader : MonoBehaviour
    {
        public UnityEvent<MapData> OnMapLoaded;
        
        [SerializeField] private SaveData saveData;
        
        private void Awake()
        {
            int sequence = DataReceiver.Instance.UserMapDataSequence;
            GetMapData(sequence);
        }

        private void GetMapData(int seq)
        {
            saveData.DataLoad($"B{seq}", data =>
            {
                string json = data;
                MapData mapData = JsonUtility.FromJson<MapData>(json);
                
                OnMapLoaded?.Invoke(mapData);
            });
        }
    }
}