using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class DeleteEditor : EditableMono
    {
        public override EditType EditType => EditType.Delete;

        [SerializeField] private MapMakerManager mapMaker;

        public override void Initialize()
        {
            base.Initialize();
            
            mapMakerChannel.AddListener<BulkDeleteEvent>(HandleBulkDeleted);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<BulkDeleteEvent>(HandleBulkDeleted);
        }

        private void HandleBulkDeleted(BulkDeleteEvent evt)
        {
            List<EditorObject> targetObjects = mapMaker.GetObjects(evt.targetObject.ID);
            
            if(targetObjects == null) return;
            
            foreach (EditorObject editorObject in targetObjects)
            {
                DeleteObject(editorObject);
            }
        }

        protected override void HandleCurrentObjectChange(CurrentObjectChangeEvent evt)
        {
            base.HandleCurrentObjectChange(evt);

            if (_isActive)
            {
                DeleteObject(_currentObject);
            }
        }

        private void DeleteObject(EditorObject targetObject)
        {
            RaiseObjectDeleteEvent(targetObject);
            RaiseHistoryEvent(targetObject);
            
            Destroy(targetObject.gameObject);
        }

        private void RaiseHistoryEvent(EditorObject targetObject)
        {
            ObjectSpawner spawner = mapMaker.GetEditor(EditType.Spawn) as ObjectSpawner;
            
            RegisterHistoryEvent evt = MapMakerEvent.RegisterHistoryEvent;
            evt.history = new DeleteHistory(mapMakerChannel, spawner.TargetObject, targetObject, targetObject.transform.position);
            mapMakerChannel.RaiseEvent(evt);
        }

        private void RaiseObjectDeleteEvent(EditorObject targetObject)
        {
            ObjectDeleteEvent evt = MapMakerEvent.ObjectDeleteEvent;
            evt.targetObject = targetObject;
            mapMakerChannel.RaiseEvent(evt);
        }
    }
}