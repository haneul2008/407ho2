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
            IsCreatedNewMap = true;
        }
        
        public void SetMapEditData(string mapName)
        {
            IsCreatedNewMap = false;
            MapEditDataName = mapName;
        }

        public void SetPlayUserMapData(int sequence)
        {
            IsCreatedNewMap = false;
            UserMapDataSequence = sequence;
        }

        public UserBuiltInData GetUserMapData()
        {
            string json = File.ReadAllText(Path);
            return JsonUtility.FromJson<UserBuiltInData>(json);
        }
    }
}