using System;
using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.Events;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Objects.Frame;
using Work.JW.Code.TriggerSystem;

namespace Work.JW.Code.MapLoad
{
    [Serializable]
    public class MapObjectPrefabAndId
    {
        public ObjectFrame prefab;
        public int id;
    }

    public class MapLoadManager : MonoBehaviour
    {
        private MapData _currentMapData;

        public UnityEvent OnMapLoaded;

        [Header("Data")]
        [SerializeField] private TriggerDataBaseSO triggerDataBase;
        // [SerializeField] private ObjPrefabDataSO objPrefabData;
        
        [Header("Prefabs")]
        [SerializeField] private ObjectFrame objFrame;
        [SerializeField] private GameObject triggerPrefab;
        
        [Header("ETC")]
        [SerializeField] private Transform outGameLineTrm;
        [SerializeField] private SoundID startSoundID;

        private Dictionary<TriggerType, TriggerDataSO> _triggers;
        private Dictionary<int, ObjectFrame> _inGamePrefabs;
        private Dictionary<int, List<Transform>> _idToObjTrms;

        private List<Trigger> _initTriggers;
        private float _minYValue;

        private void Awake()
        {
            //Init
            _triggers = new Dictionary<TriggerType, TriggerDataSO>();
            _inGamePrefabs = new Dictionary<int, ObjectFrame>();
            _idToObjTrms = new Dictionary<int, List<Transform>>();

            _initTriggers = new List<Trigger>();

            // objPrefabData.objPrefabAndIds.ForEach(item => _inGamePrefabs.Add(item.id, item.prefab));
            triggerDataBase.triggerPrefabAndTypes.ForEach(item => _triggers.Add(item.type, item));
        }

        public void Initialize(MapData mapData)
        {
            Debug.Assert(mapData != null, "MapData is null!");

            _currentMapData = mapData;
        }

        public void OnStartGameSound()
        {
            BroAudio.Play(startSoundID);
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
                    itemTrm.localScale = item.scale * 1.5f;
                }
                else
                    itemTrm = SetObjSpawn(item);

                TriggerIDFilter(item, itemTrm);

                CheckMinYValue(itemTrm.position.y);
            }

            SetTriggerTargets();

            outGameLineTrm.position = new Vector3(0, _minYValue - 5);
            
            OnMapLoaded?.Invoke();
        }

        private void SetTriggerTargets()
        {
            _initTriggers.ForEach(item =>
            {
                if (_idToObjTrms.TryGetValue(item.TargetID, out List<Transform> objTrm))
                {
                    item.SetTargets(objTrm.ToArray());
                }
                else
                {
                    Debug.Log($"Key not found : {item.TargetID}");
                }
            });
        }

        private Transform SetObjSpawn(ObjectData item)
        {
            Transform itemTrm;
            //생성
            var obj = Instantiate(objFrame, transform);
            itemTrm = obj.transform;

            obj.ID = item.objectId;
            obj.transform.position = item.position;
            obj.transform.localScale = item.scale;
            obj.transform.localRotation = Quaternion.Euler(0, 0, item.angle);
            obj.SpriteCompo.color = item.color;
            obj.SpriteCompo.sortingOrder = item.sortingOrder;

            return itemTrm;
        }

        private void CheckMinYValue(float yValue)
        {
            if (yValue < _minYValue)
            {
                _minYValue = yValue;
            }
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
            GameObject itemObj = Instantiate(triggerPrefab, transform);

            var trigger = FindAndAddCompo(itemObj, _triggers[type].triggerName);

            trigger.SetOutLineColor(data.color);

            trigger.TargetID = triggerData.targetID;
            trigger.TriggerID = string.IsNullOrEmpty(data.triggerID) ? null : int.Parse(data.triggerID);

            trigger.SetData(triggerData);

            _initTriggers.Add(trigger);
            return trigger.transform;
        }

        private Trigger FindAndAddCompo(GameObject target, string typeName)
        {
            Type type = Type.GetType($"Work.JW.Code.TriggerSystem.{typeName}");
            Debug.Assert(type != null, "Type not found!");

            return target.AddComponent(type) as Trigger;
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