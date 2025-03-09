using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;
using Work.ISC._0._Scripts.Save.Firebase;

namespace Work.HN.Code.Save
{
    public enum ErrorType
    {
        DuplicatedMyMapName,
        EmptyName,
        NoneStartOrEnd,
        FailRequest,
        ExceededMaxCapacity,
        DuplicatedUserMapName,
        NotVerified
    }

    public class ObjectInvoker : MonoBehaviour
    {
        [SerializeField] private MapMakerManager mapMaker;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private SaveManager saveManager;

        private MapData _lastSavedMapData;
        private string _mapName;
        private bool _isRegister;

        private void Awake()
        {
            mapMakerChannel.AddListener<MapNameChangeEvent>(HandleMapNameChanged);
        }

        private void OnDestroy()
        {
            mapMakerChannel.RemoveListener<MapNameChangeEvent>(HandleMapNameChanged);
        }

        private void Start()
        {
            _mapName = saveManager.GetMapName();
        }

        private void HandleMapNameChanged(MapNameChangeEvent evt)
        {
            _mapName = evt.mapName;
        }

        public bool SaveData(Action<ErrorType> onSaveFail = null)
        {
            List<EditorObject> objects = mapMaker.GetAllObjects();

            if (objects.Count == 0)
            {
                Debug.LogWarning("No objects found!");
                return false;
            }

            if (string.IsNullOrEmpty(_mapName))
            {
                return InvokeSaveFailAction(ErrorType.EmptyName, onSaveFail);
            }

            if (!saveManager.CanSaveData(_mapName))
            {
                return InvokeSaveFailAction(ErrorType.DuplicatedMyMapName, onSaveFail);
            }

            if (!mapMaker.HasStart() || !mapMaker.HasEnd())
            {
                return InvokeSaveFailAction(ErrorType.NoneStartOrEnd, onSaveFail);
            }

            if (saveManager.GetMapCapacity(objects) >= FirebaseData.maxCapacity)
            {
                return InvokeSaveFailAction(ErrorType.ExceededMaxCapacity, onSaveFail);
            }

            for (int i = 0; i < objects.Count; i++)
            {
                if (i == 0)
                {
                    saveManager.SetMapName(_mapName);
                    saveManager.ClearObjects();
                }

                ObjectSaveEvent evt = MapMakerEvent.ObjectSaveEvent;
                evt.targetObject = objects[i];
                evt.isFinish = i == objects.Count - 1;
                mapMakerChannel.RaiseEvent(evt);
            }

            if (!saveManager.IsEqualsMap(_lastSavedMapData))
            {
                saveManager.IsVerified = false;
                return InvokeSaveFailAction(ErrorType.NotVerified, onSaveFail);
            }

            return true;
        }

        private bool InvokeSaveFailAction(ErrorType errorType, Action<ErrorType> onSaveFail = null)
        {
            onSaveFail?.Invoke(errorType);
            return false;
        }

        public bool RegisterData(Action<ErrorType> onFail = null)
        {
            _isRegister = true;

            if (!SaveData(onFail) || !saveManager.IsVerified)
            {
                _isRegister = false;

                return false;
            }

            saveManager.RegisterMapData();

            return true;
        }

        public int GetMapCapacity()
        {
            return saveManager.GetMapCapacity(mapMaker.GetAllObjects());
        }


        public void HandleMapDataChanged(MapData mapData)
        {
            if (_isRegister)
            {
                _isRegister = false;
                return;
            }

            _lastSavedMapData = GetNewMapData(mapData);
        }

        private MapData GetNewMapData(MapData mapData)
        {
            MapData returnValue = new MapData();

            returnValue.mapName = mapData.mapName;

            foreach (ObjectData objData in mapData.objectList)
            {
                returnValue.objectList.Add(objData);
            }

            return returnValue;
        }
    }
}