using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.ISC._0._Scripts.Save.ExelData;

namespace Work.HN.Code.Save
{
    [Serializable]
    public struct ObjectData
    {
        public int objectId;
        public Vector2 position;
        public string triggerID;
        public Vector3 scale;
        public float angle;
        public Color color;
        public int sortingOrder;
        public bool isTrigger;
        public TriggerData triggerData;
    }

    [Serializable]
    public class TriggerData
    {
        public int targetID;
        public TriggerType triggerType;
        public MoveInfo moveInfo;
        public AlphaInfo alphaInfo;
        public ShakeInfo shakeInfo;
        public SpawnOrDestroyInfo spawnOrDestroyInfo;
    }
    
    [Serializable]
    public class MapData
    {
        public List<ObjectData> objectList = new List<ObjectData>();
        public string mapName;
        public bool isRegistered = false;
    }

    [Serializable]
    public class UserBuiltInData
    {
        public List<MapData> userMapList = new List<MapData>();
        public float masterVolume;
        public float bgmVolume;
        public float sfxVolume;
    }
    
    public class SaveManager : MonoBehaviour
    {
        public UnityEvent<MapData> OnDataLoaded;
        
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private SaveData saveData;
        
        private MapData _mapData;
        private UserBuiltInData _userData;
        private DataReceiver _dataReceiver;
        private string _path;

        private void Awake()
        {
            _dataReceiver = DataReceiver.Instance;
            
            _path = _dataReceiver.Path;

            if (File.Exists(_path))
            {
                string json = File.ReadAllText(_path);
                _userData = JsonUtility.FromJson<UserBuiltInData>(json);

                if (_dataReceiver.IsCreatedNewMap)
                {
                    CreateNewMap();
                }
                else
                {
                    _mapData = GetUsersMap(_dataReceiver.MapEditDataName);
                }
            }
            else
            {
                _userData = new UserBuiltInData();
                CreateNewMap();
            }
            
            OnDataLoaded?.Invoke(_mapData);
            
            mapMakerChannel.AddListener<ObjectSaveEvent>(HandleObjectSave);
        }

        private void CreateNewMap()
        {
            _mapData = new MapData();
            _mapData.mapName = "NewMap";
        }

        private void OnDestroy()
        {
            mapMakerChannel.RemoveListener<ObjectSaveEvent>(HandleObjectSave);
        }

        private void HandleObjectSave(ObjectSaveEvent evt)
        {
            if (evt.isFinish)
            {
                FinishData();
            }
            
            SaveObject(evt.targetObject);
        }

        private void FinishData()
        {
             MapData mapData = GetUsersMap(_mapData.mapName);
            
            if(mapData != null) _userData.userMapList.Remove(mapData);
            
            _userData.userMapList.Add(_mapData);
            
            string json = JsonUtility.ToJson(_userData);
            File.WriteAllText(_path, json);
        }

        private MapData GetUsersMap(string mapName)
        {
            foreach (MapData mapData in _userData.userMapList)
            {
                if(mapData == null || string.IsNullOrEmpty(mapData.mapName) || string.IsNullOrEmpty(mapName)) continue;
                
                if (mapName.Trim() == mapData.mapName.Trim())
                {
                    return mapData;
                }
            }
            
            return null;
        }

        public void RegisterMapData()
        {   
            _mapData.isRegistered = true;
            
            string mapDataJson = JsonUtility.ToJson(_mapData);
            saveData.DataSave(mapDataJson);
            
            string userDataJson = JsonUtility.ToJson(_userData);
            File.WriteAllText(_path, userDataJson);
        }

        public bool CanSaveData(string mapName)
        {
            MapData mapData = GetUsersMap(mapName);
            return mapData == null || mapData == _mapData;
        }

        private void SaveObject(EditorObject targetObject)
        {
            int? triggerId = targetObject.GetTriggerID();
            
            ObjectData newData = new ObjectData
            {
                objectId = targetObject.ID,
                position = targetObject.GetPosition(),
                scale = targetObject.GetSize(),
                angle = targetObject.GetAngle(),
                color = targetObject.GetColor(),
                sortingOrder = targetObject.GetSortingOrder(),
                triggerID = triggerId.HasValue ? triggerId.Value.ToString() : string.Empty,
                isTrigger = false,
                triggerData = null
            };

            if (targetObject is EditorTrigger trigger)
            {
                newData.isTrigger = true;
                newData.triggerData = new TriggerData();
                newData.triggerData.targetID = trigger.GetTargetID();
                newData.triggerData.triggerType = trigger.TriggerType;
                SetInfo(newData, trigger);
            }
            
            _mapData.objectList.Add(newData);
        }

        private void SetInfo(ObjectData newData, EditorTrigger trigger)
        {
            switch (trigger.TriggerType)
            {
                case TriggerType.ObjectMove:
                case TriggerType.CameraMove:
                    newData.triggerData.moveInfo = trigger.GetInfo<MoveInfo>();
                    break;
                
                case TriggerType.Alpha:
                    newData.triggerData.alphaInfo = trigger.GetInfo<AlphaInfo>();
                    break;
                
                case TriggerType.Shake:
                    newData.triggerData.shakeInfo = trigger.GetInfo<ShakeInfo>();
                    break;
                
                case TriggerType.Spawn:
                case TriggerType.Destroy:
                    newData.triggerData.spawnOrDestroyInfo = trigger.GetInfo<SpawnOrDestroyInfo>();
                    break;
            }
        }

        public string GetMapName()
        {
            return _mapData.mapName;
        }
        
        public void SetMapName(string mapName)
        {
            _mapData.mapName = mapName;
        }
        
        public void ClearObjects()
        {
            _mapData.objectList.Clear();
        }

        [ContextMenu("TestLoad")]
        public void TestLoad()
        {
            saveData.DataLoad("B2:B1000", data =>
            {
                print(data);
            });
        }
    }
}