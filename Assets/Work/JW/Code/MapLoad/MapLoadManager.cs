using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Objects.Frame;
using Work.JW.Code.TriggerSystem;
using Object = Work.HN.Code.MapMaker.Objects.Object;

namespace Work.JW.Code.MapLoad
{
    [Serializable]
    public class MapTriggerPrefabAndType
    {
        public Trigger prefab;
        public TriggerType type;
    }

    [Serializable]
    public class MapObjectPrefabAndId
    {
        public ObjectFrame prefab;
        public int id;
    }

    public class MapLoadManager : MonoBehaviour
    {
        private MapData _currentMapData;

        [SerializeField] private TriggerPrefabDataSO triggerPrefabData;
        [SerializeField] private ObjPrefabDataSO objPrefabData;
        [SerializeField] private ObjectFrame objFrame;
        
        private Dictionary<TriggerType, Trigger> _triggers;
        private Dictionary<int, ObjectFrame> _inGamePrefabs;
        private Dictionary<int, List<Transform>> _idToObjTrms;
        
        private List<Trigger> _initTriggers;

        private void Awake()
        {
            _triggers = new Dictionary<TriggerType, Trigger>();
            _inGamePrefabs = new Dictionary<int, ObjectFrame>();
            _idToObjTrms = new Dictionary<int, List<Transform>>();
            
            _initTriggers = new List<Trigger>();

            objPrefabData.objPrefabAndIds.ForEach(item => _inGamePrefabs.Add(item.id, item.prefab));
            triggerPrefabData.triggerPrefabAndTypes.ForEach(item => _triggers.Add(item.type, item.prefab));
        }

        public void Initialize(MapData mapData)
        {
            Debug.Assert(mapData != null, "MapData is null!");
            
            _currentMapData = mapData;
            SetMapObjSpawn();
        }

        public void SetMapObjSpawn()
        {
            foreach (var item in _currentMapData.objectList)
            {
                Transform itemTrm;
                
                if (item.isTrigger)
                {
                    itemTrm = AddTriggerToObj(item);
                    itemTrm.position = item.position;
                    itemTrm.localScale = item.scale;
                }
                else
                {
                    //생성
                    var obj = Instantiate(objFrame, transform);
                    itemTrm = obj.transform;
                    
                    obj.ID = item.objectId;
                    obj.transform.position = item.position;
                    obj.transform.localScale = item.scale;
                    obj.transform.localRotation = Quaternion.Euler(0, 0, item.angle);
                    obj.SpriteCompo.color = item.color;
                    obj.SpriteCompo.sortingOrder = item.sortingOrder;
                }

                TriggerIDFilter(item, itemTrm);
            }

            _initTriggers.ForEach(item =>
            {
                if(_idToObjTrms.TryGetValue(item.TargetID, out List<Transform> objTrm))
                {
                    item.SetTargets(objTrm.ToArray());
                }
                else
                {
                    Debug.Log($"Key not found : {item.TargetID}");
                }
            });
        }

        private void TriggerIDFilter(ObjectData data, Transform itemTrm)
        {
            if (string.IsNullOrEmpty(data.triggerID)) return;
            
            int triggerId = int.Parse(data.triggerID);
            List<Transform> objTrms = new List<Transform>();

            if (_idToObjTrms.TryGetValue(triggerId, out List<Transform> trmList))
            {
                trmList.Add(itemTrm);
                _idToObjTrms[triggerId] = trmList;
                return;
            }
            objTrms.Add(itemTrm);
            _idToObjTrms.Add(triggerId, objTrms);
        }

        private Transform AddTriggerToObj(ObjectData data)
        {
            TriggerData triggerData = data.triggerData;
            TriggerType type = data.triggerData.triggerType;
            var trigger = Instantiate(_triggers[type], transform);
            
            trigger.TargetID = triggerData.targetID;
            trigger.TriggerID = string.IsNullOrEmpty(data.triggerID) ? null : int.Parse(data.triggerID);
            
            switch (type)
            {
                case TriggerType.ObjectMove:
                    var moveTrigger = trigger as MoveObjTrigger;
                    MoveInfo moveInfo = triggerData.moveInfo;
                    
                    moveTrigger.SetData(moveInfo.moveAmount, moveInfo.duration);
                    break;
                case TriggerType.Alpha:
                    var alphaTrigger = trigger as AlphaTrigger;
                    AlphaInfo alphaInfo = triggerData.alphaInfo;
                    
                    alphaTrigger.SetData(alphaInfo.endValue, alphaInfo.duration);
                    break;
                case TriggerType.Shake:
                    var shakeTrigger = trigger as ShakeTrigger;
                    ShakeInfo shakeInfo = triggerData.shakeInfo;
                    
                    shakeTrigger.SetData(shakeInfo.strength, shakeInfo.duration);
                    break;
                case TriggerType.Spawn:
                    var spawnTrigger = trigger as SetEnableTrigger;
                    SpawnOrDestroyInfo spawnInfo = triggerData.spawnOrDestroyInfo;
                    
                    spawnTrigger.SetData(true, 0.15f);
                    break;
                case TriggerType.Destroy:
                    var destroyTrigger = trigger as SetEnableTrigger;
                    SpawnOrDestroyInfo destroyInfo = triggerData.spawnOrDestroyInfo;
                    
                    destroyTrigger.SetData(false, 0.15f);
                    break;
                case TriggerType.CameraMove:
                    var camMoveTrigger = trigger as CameraMoveTrigger;
                    MoveInfo camMoveInfo = triggerData.moveInfo;
                    
                    camMoveTrigger.SetData(camMoveInfo.moveAmount, camMoveInfo.duration);
                    break;
                case TriggerType.None:
                    break;
            }
            
            _initTriggers.Add(trigger);
            return trigger.transform;
        }

        public void Clear()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            _idToObjTrms.Clear();
            _initTriggers.Clear();
            _inGamePrefabs.Clear();
        }
    }
}