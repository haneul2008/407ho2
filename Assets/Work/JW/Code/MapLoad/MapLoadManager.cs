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
        
        private Dictionary<TriggerType, Trigger> _triggers;
        private Dictionary<int, ObjectFrame> _inGamePrefabs;
        private Dictionary<string, List<Transform>> _idToObjTrms;
        
        private List<Trigger> _initTriggers;

        private void Awake()
        {
            _triggers = new Dictionary<TriggerType, Trigger>();
            _inGamePrefabs = new Dictionary<int, ObjectFrame>();
            _idToObjTrms = new Dictionary<string, List<Transform>>();
            
            _initTriggers = new List<Trigger>();

            objPrefabData.objPrefabAndIds.ForEach(item => _inGamePrefabs.Add(item.id, item.prefab));
            triggerPrefabData.triggerPrefabAndTypes.ForEach(item => _triggers.Add(item.type, item.prefab));
        }

        public void Initialize(MapData mapData)
        {
            Debug.Assert(mapData != null, "MapData is null!");
            
            _currentMapData = mapData;
            SetMapObjSpawn(mapData);
        }

        private void SetMapObjSpawn(MapData mapData)
        {
            foreach (var item in _currentMapData.objectList)
            {
                Transform itemTrm;
                
                if (item.triggerData.triggerType != TriggerType.None)
                {
                    itemTrm = AddTriggerToObj(item);
                    itemTrm.position = item.position;
                    itemTrm.localScale = item.scale;
                }
                else
                {
                    //생성
                    var obj = Instantiate(_inGamePrefabs[item.objectId], transform);
                    itemTrm = obj.transform;
                    
                    obj.ID = item.objectId;
                    obj.transform.position = item.position;
                    obj.transform.localScale = item.scale;
                    obj.transform.localRotation = Quaternion.Euler(0, 0, item.angle);
                    obj.SpriteCompo.color = item.color;
                    obj.SpriteCompo.sortingOrder = item.sortingOrder;
                }

                //Trigger Id에 따라 List에 각각 넣기
                List<Transform> objTrms = new List<Transform>();

                if (_idToObjTrms.TryGetValue(item.triggerID, out List<Transform> trmList))
                {
                    trmList.Add(itemTrm);
                    _idToObjTrms[item.triggerID] = trmList;
                    continue;
                }
                objTrms.Add(itemTrm);
                _idToObjTrms.Add(item.triggerID, objTrms);
            }

            _initTriggers.ForEach(item => item.SetTargets(_idToObjTrms[item.ID].ToArray()));
        }

        private Transform AddTriggerToObj(ObjectData data)
        {
            TriggerData triggerData = data.triggerData;
            TriggerType type = data.triggerData.triggerType;
            var trigger = Instantiate(_triggers[type], transform);
            trigger.ID = data.triggerID;

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
                    
                    spawnTrigger.SetData(true, 0.15f);
                    break;
                case TriggerType.Destroy:
                    var destroyTrigger = trigger as SetEnableTrigger;
                    
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

            return trigger.transform;
        }
    }
}