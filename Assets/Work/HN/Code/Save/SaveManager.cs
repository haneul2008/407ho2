using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        public TriggerData triggerData;
    }

    [Serializable]
    public class TriggerData
    {
        public TriggerType triggerType;
        public MoveInfo moveInfo;
        public AlphaInfo alphaInfo;
        public ShakeInfo shakeInfo;
        public SpawnOrDestroyInfo spawnOrDestroyInfo;
    }
    
    public class MapData
    {
        public List<ObjectData> objectList = new List<ObjectData>();
        public string mapName = "Test";
    }
    
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private SaveData saveData;
        
        private MapData _mapData;

        private void Awake()
        {
            mapMakerChannel.AddListener<ObjectSaveEvent>(HandleObjectSave);
        }

        private void OnDestroy()
        {
            mapMakerChannel.RemoveListener<ObjectSaveEvent>(HandleObjectSave);
        }

        private void HandleObjectSave(ObjectSaveEvent evt)
        {
            if (evt.isInitialize)
            {
                InitializeData();
            }
            else if (evt.isFinish)
            {
                FinishData();
            }
            
            SaveObject(evt.targetObject);
        }

        private void FinishData()
        {
        }

        private void InitializeData()
        {
            _mapData = new MapData();
        }

        public void RegisterMapData()
        {   
            string json = JsonUtility.ToJson(_mapData);
            saveData.DataSave(json);
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
                triggerData = null
            };

            if (targetObject is EditorTrigger trigger)
            {
                newData.triggerData = new TriggerData();
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

        public void GetMapData(int mapId)
        {
            
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