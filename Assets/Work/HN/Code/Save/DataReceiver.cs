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
        public string UserMapName { get; private set; }

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

        public void TryVerify()
        {
            if (string.IsNullOrEmpty(PlayEditedMapName)) return;

            UserBuiltInData userData = GetUserMapData();

            if (userData == null) return;

            MapData mapData = GetMapData(userData, PlayEditedMapName);

            if (mapData == null) return;

            mapData.isVerified = true;

            string json = JsonUtility.ToJson(userData);
            File.WriteAllText(Path, json);
        }

        private MapData GetMapData(UserBuiltInData userData, string mapName)
        {
            foreach (MapData mapData in userData.userMapList)
            {
                if (mapData.mapName == mapName)
                {
                    return mapData;
                }
            }

            return null;
        }

        public void SetPlayUserMapData(string mapName)
        {
            ClearData();

            IsCreatedNewMap = false;
            UserMapName = mapName;
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
            UserMapName = string.Empty;
        }
    }
}