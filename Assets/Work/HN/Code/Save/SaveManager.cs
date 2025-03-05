using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.ISC._0._Scripts.Save.ExelData;
using Work.ISC._0._Scripts.Save.Firebase;

namespace Work.HN.Code.Save
{
    [Serializable]
    public struct ObjectData
    {
        public int objectId;
        public Vector3 position;
        public string triggerID;
        public Vector3 scale;
        public float angle;
        public Color color;
        public int sortingOrder;
        public bool isTrigger;
        public TriggerData triggerData;

        public bool IsEqualsObject(ObjectData target)
        {
            bool isEqualsTriggerData;

            if(triggerData == null || target.triggerData == null)
            {
                isEqualsTriggerData = true;
            }
            else
            {
                isEqualsTriggerData = triggerData.IsEquals(target.triggerData);
            }

            return objectId == target.objectId && position == target.position &&
                triggerID == target.triggerID && scale == target.scale &&
                angle == target.angle && color == target.color &&
                sortingOrder == target.sortingOrder && isTrigger == target.isTrigger &&
                isEqualsTriggerData;
        }
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

        public bool IsEquals(TriggerData target)
        {
            return targetID == target.targetID &&
                triggerType == target.triggerType &&
                moveInfo.Equals(target.moveInfo) &&
                alphaInfo.Equals(target.alphaInfo) &&
                shakeInfo.Equals(target.shakeInfo) &&
                spawnOrDestroyInfo.Equals(target.spawnOrDestroyInfo);
        }
    }
    
    [Serializable]
    public class MapData
    {
        public List<ObjectData> objectList = new List<ObjectData>();
        public string mapName;
        public bool isVerified = false;
        public bool isRegistered = false;

        public bool IsEqualsMap(MapData target)
        {
            List<ObjectData> targetObjects = target.objectList;

            if (targetObjects.Count != objectList.Count) return false;

            for(int i = 0; i < objectList.Count; i++)
            {
                if (!objectList[i].IsEqualsObject(targetObjects[i]))
                {
                    return false;
                }
            }

            return true;
        }
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
        public UnityEvent<MapData> OnDataSaved;
        public bool IsVerified
        {
            get => _mapData.isVerified;
            set => _mapData.isVerified = value;
        }
        
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private FirebaseData saveData;
        
        private MapData _mapData;
        private UserBuiltInData _userData;
        private DataReceiver _dataReceiver;
        private string _path;
        private MapData _capacityData;

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
            
            mapMakerChannel.AddListener<ObjectSaveEvent>(HandleObjectSave);
        }

        private void Start()
        {
            OnDataLoaded?.Invoke(_mapData);
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
            SaveObject(evt.targetObject);
            
            if (evt.isFinish)
            {
                FinishData();
            }
        }

        private void FinishData()
        {
             MapData mapData = GetUsersMap(_mapData.mapName);

            if (mapData != null)
            {
                _userData.userMapList.Remove(mapData);
            }

            _userData.userMapList.Add(_mapData);
            
            string json = JsonUtility.ToJson(_userData);
            File.WriteAllText(_path, json);

            OnDataSaved?.Invoke(_mapData);
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
            //saveData.SaveData(GetMinifiedJson(mapDataJson), HandleFailSave, () => SceneManager.LoadScene("TitleHN"));
            saveData.SaveData(GetMinifiedJson(mapDataJson));
            
            string userDataJson = JsonUtility.ToJson(_userData);
            File.WriteAllText(_path, userDataJson);
        }

        private void HandleFailSave(ErrorType type)
        {
            _mapData.isRegistered = false;
            
            SaveFailEvent evt = MapMakerEvent.SaveFailEvent;
            evt.errorType = type;
            mapMakerChannel.RaiseEvent(evt);
        }

        public bool CanSaveData(string mapName)
        {
            MapData mapData = GetUsersMap(mapName);
            return mapData == null || mapData == _mapData;
        }

        private void SaveObject(EditorObject targetObject)
        {
            ObjectData newData = GetNewObjectData(targetObject);

            _mapData.objectList.Add(newData);
        }

        private ObjectData GetNewObjectData(EditorObject targetObject)
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
                newData.triggerData = new TriggerData()
                {
                    triggerType = trigger.TriggerType,
                    targetID = trigger.GetTargetID()
                };
                
                SetInfo(newData, trigger);
            }

            return newData;
        }

        private void SetInfo(ObjectData newData, EditorTrigger trigger)
        {
            newData.triggerData.moveInfo = new MoveInfo();
            newData.triggerData.alphaInfo = new AlphaInfo();
            newData.triggerData.shakeInfo = new ShakeInfo();
            newData.triggerData.spawnOrDestroyInfo = new SpawnOrDestroyInfo();
            
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
        
        public int GetMapCapacity(List<EditorObject> objects)
        {
            _capacityData ??= new MapData();
            
            _capacityData.objectList.Clear();
            _capacityData.mapName = _mapData.mapName;
            _capacityData.isRegistered = false;

            foreach (EditorObject obj in objects)
            {
                _capacityData.objectList.Add(GetNewObjectData(obj));
            }
            
            string json = JsonUtility.ToJson(_capacityData);
            
            return GetMinifiedJson(json).Length;
        }

        private string GetMinifiedJson(string json)
        {
            return Regex.Replace(json, @"\s+", "");
        }

        public bool IsEqualsMap(MapData targetMap)
        {
            if(_mapData == null) return false;

            return _mapData.IsEqualsMap(targetMap);
        }
    }
}