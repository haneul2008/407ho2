using System;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class TriggerIDEditor : EditableMono
    {
        public override EditType EditType => EditType.TriggerID;

        public override void Initialize()
        {
            base.Initialize();
            
            mapMakerChannel.AddListener<TriggerIDChangeEvent>(HandleTriggerIDChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<TriggerIDChangeEvent>(HandleTriggerIDChanged);
        }

        private void HandleTriggerIDChanged(TriggerIDChangeEvent evt)
        {
            EditorObject targetObject = evt.targetObject;
            
            if(targetObject == null) return;

            int? beforeId = targetObject.GetTriggerID();
            
            targetObject.InfoManager.ChangeInfo(InfoType.TriggerID, evt.id);

            if (!evt.isHistory)
            {
                RaiseHistoryEvent(beforeId, targetObject);
            }
        }
        
        private void RaiseHistoryEvent(int? beforeId, EditorObject targetObject)
        {
            RegisterHistoryEvent evt = MapMakerEvent.RegisterHistoryEvent;
            evt.history = new TriggerIDHistory(mapMakerChannel, targetObject, beforeId, targetObject.GetTriggerID());
            mapMakerChannel.RaiseEvent(evt);
        }
    }
}