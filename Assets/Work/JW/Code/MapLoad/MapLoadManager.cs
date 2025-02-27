using System;
using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.HN.Code.Save;
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
        public InGameObject prefab;
        public int id;
    }

    public class MapLoadManager : MonoBehaviour
    {
        private MapData _currentMapData;

        [SerializeField] private TriggerPrefabDataSO _triggerPrefabData;
        private Dictionary<TriggerType, Trigger> _triggers;
        // private Dictionary<int, InGameObject> _inGamePrefabs;
        private Dictionary<string, List<Transform>> _idToObjTrms;

        private void Awake()
        {
            _triggers = new Dictionary<TriggerType, Trigger>();
            _idToObjTrms = new Dictionary<string, List<Transform>>();

            _triggerPrefabData.triggerPrefabAndTypes.ForEach(item => _triggers.Add(item.type, item.prefab));
        }

        public void Initialize(MapData mapData)
        {
            _currentMapData = mapData;
        }

        private void SetMapObj()
        {
            foreach (var item in _currentMapData.objectList)
            {
                if (item.triggerData.triggerType != TriggerType.None)
                {
                    Transform itemTrm = AddTriggerToObj(item);
                    itemTrm.position = item.position;
                }

                /*//Tile 생성
                var tileItem = Instantiate(_inGamePrefabs[item.objectId], transform);

                //Trigger Id에 따라 List에 각각 넣기
                List<Transform> objTrms = _idToObjTrms[item.triggerID];
                if(objTrms == null) objTrms = new List<Transform>();

                objTrms.Add(tileItem.transform);
                _idToObjTrms.Add(item.triggerID, objTrms);

                //tileItem 설정 하기
                */
            }
        }

        private Transform AddTriggerToObj(ObjectData data)
        {
            TriggerData triggerData = data.triggerData;
            TriggerType type = data.triggerData.triggerType;
            var trigger = Instantiate(_triggers[type], transform);

            switch (type)
            {
                case TriggerType.ObjectMove:
                    var moveTrigger = trigger as MoveObjTrigger;
                    MoveInfo moveInfo = triggerData.moveInfo;

                    moveTrigger.SetTargets(_idToObjTrms[data.triggerID].ToArray());
                    moveTrigger.SetData(moveInfo.moveAmount, moveInfo.duration);
                    break;
                case TriggerType.Alpha:
                    var alphaTrigger = trigger as AlphaTrigger;
                    AlphaInfo alphaInfo = triggerData.alphaInfo;

                    alphaTrigger.SetTargets(_idToObjTrms[data.triggerID].ToArray());
                    alphaTrigger.SetData(alphaInfo.endValue, alphaInfo.duration);
                    break;
                case TriggerType.Shake:
                    var shakeTrigger = trigger as ShakeTrigger;
                    ShakeInfo shakeInfo = triggerData.shakeInfo;
                    
                    shakeTrigger.SetTargets(_idToObjTrms[data.triggerID].ToArray());
                    shakeTrigger.SetData(shakeInfo.strength, shakeInfo.duration);
                    break;
                case TriggerType.Spawn:
                    var spawnTrigger = trigger as SetEnableTrigger;
                    
                    spawnTrigger.SetTargets(_idToObjTrms[data.triggerID].ToArray());
                    spawnTrigger.SetData(true, 0.15f);
                    break;
                case TriggerType.Destroy:
                    var destroyTrigger = trigger as SetEnableTrigger;
                    
                    destroyTrigger.SetTargets(_idToObjTrms[data.triggerID].ToArray());
                    destroyTrigger.SetData(false, 0.15f);

                    break;
                case TriggerType.CameraMove:
                    var camMoveTrigger = trigger as CameraMoveTrigger;
                    MoveInfo camMoveInfo = triggerData.moveInfo;
                    
                    camMoveTrigger.SetTargets(_idToObjTrms[data.triggerID].ToArray());
                    camMoveTrigger.SetData(camMoveInfo.moveAmount, camMoveInfo.duration);
                    break;
                case TriggerType.None:
                    break;
            }

            return trigger.transform;
        }
    }
}