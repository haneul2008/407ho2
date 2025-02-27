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
        
        public bool SaveData()
        {
            List<EditorObject> objects = mapMaker.GetAllObjects();

            if (objects.Count == 0)
            {
                Debug.LogWarning("No objects found!");
                return false;
            }

            for (int i = 0; i < objects.Count; i++)
            {
                ObjectSaveEvent evt = MapMakerEvent.ObjectSaveEvent;
                evt.targetObject = objects[i];
                evt.isInitialize = i == 0;
                evt.isFinish = i == objects.Count - 1;
                mapMakerChannel.RaiseEvent(evt);
            }
            
            return true;
        }
        
        public void RegisterData()
        {
            if(!SaveData()) return;
            
            saveManager.RegisterMapData();
        }
    }
}