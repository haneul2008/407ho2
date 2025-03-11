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
        EmptyObject,
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
        [SerializeField] private DelegateModerator<Action<ErrorType>> saveModerator;

        private MapData _lastSavedMapData;
        private MapNameContainer _mapNameContainer;
        private bool _isRegister;

        private void OnDestroy()
        {
            _mapNameContainer?.Dispose();
        }

        public void HandleMapLoaded(MapData mapData)
        {
            _mapNameContainer = new MapNameContainer(mapData.mapName, mapMakerChannel);
        }
        
        public bool SaveData(Action<ErrorType> onSaveFail = null)
        {
            saveModerator.SetAction(onSaveFail);

            if (!saveModerator.Execute()) return false;

            List<EditorObject> objects = mapMaker.GetAllObjects();

            SaveObjects(objects);

            return IsMapFixed(onSaveFail);
        }

        private bool IsMapFixed(Action<ErrorType> onSaveFail)
        {
            if (!saveManager.IsEqualsMap(_lastSavedMapData))
            {
                saveManager.IsVerified = false;
                onSaveFail?.Invoke(ErrorType.NotVerified);
                return false;
            }

            return true;
        }

        private void SaveObjects(List<EditorObject> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (i == 0)
                {
                    saveManager.SetMapName(_mapNameContainer.MapName);
                    saveManager.ClearObjects();
                }

                ObjectSaveEvent evt = MapMakerEvent.ObjectSaveEvent;
                evt.targetObject = objects[i];
                evt.isFinish = i == objects.Count - 1;
                mapMakerChannel.RaiseEvent(evt);
            }
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
            return new MapData()
            {
                mapName = mapData.mapName,
                objectList = new List<ObjectData>(mapData.objectList)
            };
        }
    }
}