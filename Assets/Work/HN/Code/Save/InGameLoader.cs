using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Work.ISC._0._Scripts.Save.ExelData;
using Work.ISC._0._Scripts.Save.Firebase;

namespace Work.HN.Code.Save
{
    public class InGameLoader : MonoBehaviour
    {
        public UnityEvent<MapData> OnMapLoaded;
        
        [SerializeField] private FirebaseData saveData;
        
        private void Start()
        {
            string userMapName = DataReceiver.Instance.UserMapName;
            string editedMapName = DataReceiver.Instance.PlayEditedMapName;

            if (string.IsNullOrEmpty(editedMapName))
            {
                GetUserMapData(userMapName);
            }
            else if (string.IsNullOrEmpty(userMapName))
            {
                GetEditedMapData(editedMapName);
            }
        }

        private void GetEditedMapData(string mapName)
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

        private void GetUserMapData(string mapName)
        {
            saveData.LoadData(mapName, null, mapData => OnMapLoaded?.Invoke(mapData));
        }
    }
}