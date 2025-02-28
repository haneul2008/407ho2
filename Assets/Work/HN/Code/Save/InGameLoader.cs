using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Work.ISC._0._Scripts.Save.ExelData;

namespace Work.HN.Code.Save
{
    public class InGameLoader : MonoBehaviour
    {
        public UnityEvent<MapData> OnMapLoaded;
        
        [SerializeField] private SaveData saveData;
        
        private void Start()
        {
            int sequence = DataReceiver.Instance.UserMapDataSequence;
            string editedMapName = DataReceiver.Instance.PlayEditedMapName;

            if (string.IsNullOrEmpty(editedMapName))
            {
                GetMapData(sequence);
            }
            else if (sequence == 0)
            {
                GetMapData(editedMapName);
            }
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
        
        private void GetMapData(string mapName)
        {
            string json = File.ReadAllText(DataReceiver.Instance.Path);
            UserBuiltInData userData = JsonUtility.FromJson<UserBuiltInData>(json);

            foreach (MapData mapData in userData.userMapList)
            {
                if (mapData.mapName == mapName)
                {
                    OnMapLoaded?.Invoke(mapData);
                    return;
                }
            }
        }
    }
}