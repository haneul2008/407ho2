using System;
using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.Save
{
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

        public bool SaveData()
        {
            List<EditorObject> objects = mapMaker.GetAllObjects();

            if (objects.Count == 0)
            {
                Debug.LogWarning("No objects found!");
                return false;
            }

            if (!saveManager.CanSaveData(_mapName)) return false;
            
            for (int i = 0; i < objects.Count; i++)
            {
                ObjectSaveEvent evt = MapMakerEvent.ObjectSaveEvent;
                evt.targetObject = objects[i];
                evt.isInitialize = i == 0;
                evt.isFinish = i == objects.Count - 1;
                mapMakerChannel.RaiseEvent(evt);
            }
            
            saveManager.SetMapName(_mapName);
            
            return true;
        }
        
        public void RegisterData()
        {
            if(!SaveData()) return;
            
            saveManager.RegisterMapData();
        }
    }
}