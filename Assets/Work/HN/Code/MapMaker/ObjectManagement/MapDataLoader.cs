using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.HN.Code.Save;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class MapDataLoader : MonoBehaviour
    {
        [SerializeField] private ObjectListSO objectList, triggerList;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        
        private readonly Dictionary<EditorObject, ObjectData> _objectDataPairs = new Dictionary<EditorObject, ObjectData>();
        
        public void HandleDataLoad(MapData mapData)
        {
            SpawnObjects(mapData);
        }

        private void SpawnObjects(MapData mapData)
        {
            foreach (ObjectData objectData in mapData.objectList)
            {   
                EditorObject obj = GetObject(objectData.objectId);

                EditorObject spawnedObj = Instantiate(obj);

                _objectDataPairs.Add(spawnedObj, objectData);
                
                spawnedObj.OnSpawned += HandleSpawnEvent;
                
                RaiseSpawnEvent(spawnedObj);

                if (objectData.isTrigger)
                {
                    EditorTrigger trigger = spawnedObj as EditorTrigger;

                    SetTriggerInfo(objectData, trigger);
                }
            }
        }

        private void HandleSpawnEvent(EditorObject obj)
        {
            obj.OnSpawned -= HandleSpawnEvent;
            SetObjectInfo(obj, _objectDataPairs[obj]);
        }

        private void SetTriggerInfo(ObjectData objectData, EditorTrigger trigger)
        {
            TriggerData triggerData = objectData.triggerData;
            int targetID = triggerData.targetID;
            
            switch (triggerData.triggerType)
            {
                case TriggerType.ObjectMove:
                case TriggerType.CameraMove:
                    MoveInfo moveInfo = GetInfo<MoveInfo>(triggerData.moveInfo, targetID);
                    trigger.SetData(moveInfo);
                    break;
                
                case TriggerType.Alpha:
                    AlphaInfo alphaInfo = GetInfo<AlphaInfo>(triggerData.alphaInfo, targetID);
                    trigger.SetData(alphaInfo);
                    break;
                
                case TriggerType.Shake:
                    ShakeInfo shakeInfo = GetInfo<ShakeInfo>(triggerData.shakeInfo, targetID);
                    trigger.SetData(shakeInfo);
                    break;
                
                case TriggerType.Spawn:
                case TriggerType.Destroy:
                    SpawnOrDestroyInfo spawnOrDestroyInfo = GetInfo<SpawnOrDestroyInfo>(triggerData.spawnOrDestroyInfo, targetID);
                    trigger.SetData(spawnOrDestroyInfo);
                    break;
            }
        }

        private T GetInfo<T>(ITriggerInfo targetInfo, int targetID) where T : ITriggerInfo
        {
            if(targetInfo is T info)
            {
                T returnValue = info;
                returnValue.ID = targetID;

                return returnValue;
            }
            else
            {
                return default;
            }
        }

        private static void SetObjectInfo(EditorObject spawnedObj, ObjectData objectData)
        {
            int? triggerId;

            if (string.IsNullOrEmpty(objectData.triggerID))
            {
                triggerId = null;
            }
            else
            {
                triggerId = int.Parse(objectData.triggerID);
            }
            
            spawnedObj.InfoManager.ChangeInfo(InfoType.Position, objectData.position);
            spawnedObj.InfoManager.ChangeInfo(InfoType.Size, objectData.scale);
            spawnedObj.InfoManager.ChangeInfo(InfoType.Angle, objectData.angle);
            spawnedObj.InfoManager.ChangeInfo(InfoType.Color, objectData.color);
            spawnedObj.InfoManager.ChangeInfo(InfoType.TriggerID, triggerId);
            spawnedObj.InfoManager.ChangeInfo(InfoType.SortingOrder, objectData.sortingOrder);
        }

        private void RaiseSpawnEvent(EditorObject editorObject)
        {
            ObjectSpawnEvent evt = MapMakerEvent.ObjectSpawnEvent;
            evt.spawnedObject = editorObject;
            mapMakerChannel.RaiseEvent(evt);
        }

        private EditorObject GetObject(int id)
        {
            foreach (EditorObject obj in objectList.objects)
            {
                if (obj.ID == id)
                {
                    return obj;
                }
            }
            
            foreach (EditorObject trigger in triggerList.objects)
            {
                if (trigger.ID == id)
                {
                    return trigger;
                }
            }
            
            return null;
        }
    }
}