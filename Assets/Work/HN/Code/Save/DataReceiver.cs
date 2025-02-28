using System;
using System.IO;
using UnityEngine;

namespace Work.HN.Code.Save
{
    public class DataReceiver : MonoBehaviour
    {
        public static DataReceiver Instance;
        public string Path { get; private set; }
        public bool IsCreatedNewMap { get; private set; }
        public string MapEditDataName { get; private set; }
        public string PlayEditedMapName { get; private set; }
        public int UserMapDataSequence { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Path = $"{Application.persistentDataPath}/GameData.json";

            Instance = this;
            
            DontDestroyOnLoad(this);
        }

        public void CreateNewMap()
        {
            ClearData();
            
            IsCreatedNewMap = true;
        }
        
        public void SetMapEditData(string mapName)
        {
            ClearData();
            
            IsCreatedNewMap = false;
            MapEditDataName = mapName;
        }
        
        public void SetPlayMapData(string mapName)
        {
            ClearData();
            
            IsCreatedNewMap = false;
            PlayEditedMapName = mapName;
        }

        public void SetPlayUserMapData(int sequence)
        {
            ClearData();

            IsCreatedNewMap = false;
            UserMapDataSequence = sequence;
        }

        public UserBuiltInData GetUserMapData()
        {
            if (File.Exists(Path))
            {
                string json = File.ReadAllText(Path);
                return JsonUtility.FromJson<UserBuiltInData>(json);
            }
            else
            {
                return null;
            }
        }

        private void ClearData()
        {
            MapEditDataName = string.Empty;
            PlayEditedMapName = string.Empty;
            UserMapDataSequence = 0;
        }
    }
}