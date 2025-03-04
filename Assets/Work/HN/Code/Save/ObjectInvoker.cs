using System;
using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.Save
{
    public enum ErrorType
    {
        SameName,
        EmptyName,
        NoneStartOrEnd,
        FailRequest,
        ExceededMaxCapacity,
    }

    public class ObjectInvoker : MonoBehaviour
    {
        [SerializeField] private MapMakerManager mapMaker;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private SaveManager saveManager;

        private string _mapName;

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

        public bool SaveData(bool isRegister, Action<ErrorType> onSaveFail = null)
        {
            List<EditorObject> objects = mapMaker.GetAllObjects();

            if (objects.Count == 0)
            {
                Debug.LogWarning("No objects found!");
                return false;
            }

            if (string.IsNullOrEmpty(_mapName))
            {
                onSaveFail?.Invoke(ErrorType.EmptyName);
                return false;
            }

            if (!saveManager.CanSaveData(_mapName))
            {
                onSaveFail?.Invoke(ErrorType.SameName);
                return false;
            }

            if (!mapMaker.HasStart() || !mapMaker.HasEnd())
            {
                onSaveFail?.Invoke(ErrorType.NoneStartOrEnd);
                return false;
            }

            if (saveManager.GetMapCapacity(objects) >= ISC._0._Scripts.Save.ExelData.SaveData.maxCapacity)
            {
                onSaveFail?.Invoke(ErrorType.ExceededMaxCapacity);
                return false;
            }

            for (int i = 0; i < objects.Count; i++)
            {
                if (i == 0)
                {
                    if (!isRegister) saveManager.IsVerified = false;

                    saveManager.SetMapName(_mapName);
                    saveManager.ClearObjects();
                }

                ObjectSaveEvent evt = MapMakerEvent.ObjectSaveEvent;
                evt.targetObject = objects[i];
                evt.isFinish = i == objects.Count - 1;
                mapMakerChannel.RaiseEvent(evt);
            }

            return true;
        }

        public void RegisterData()
        {
            if (!SaveData(true)) return;

            saveManager.RegisterMapData();
        }

        public int GetMapCapacity()
        {
            return saveManager.GetMapCapacity(mapMaker.GetAllObjects());
        }
    }
}