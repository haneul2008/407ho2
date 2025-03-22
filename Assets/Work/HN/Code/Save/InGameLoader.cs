using System;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Work.ISC._0._Scripts.Save.ExelData;
using Work.ISC._0._Scripts.Save.Firebase;

namespace Work.HN.Code.Save
{
    public class InGameLoader : NetworkBehaviour
    {
        public UnityEvent<MapData> OnMapLoaded;
        
        [SerializeField] private FirebaseData saveData;
        
        protected virtual void Start()
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

        protected void GetEditedMapData(string mapName)
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

        protected void GetUserMapData(string mapName)
        {
            saveData.LoadData(mapName, null, mapData => OnMapLoaded?.Invoke(mapData));
        }
    }
}