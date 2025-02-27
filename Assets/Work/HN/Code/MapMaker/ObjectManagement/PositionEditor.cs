using System;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class PositionEditor : EditableMono
    {
        public override EditType EditType => EditType.Position;

        public override void Initialize()
        {
            base.Initialize();
            
            mapMakerChannel.AddListener<MoveEvent>(HandleMoveEvent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<MoveEvent>(HandleMoveEvent);
        }

        private void HandleMoveEvent(MoveEvent evt)
        {
            EditorObject targetObject = evt.targetObject;
            
            if(targetObject == null) return;
            
            Vector3 beforePos = targetObject.GetPosition();
            Vector3 newPos = beforePos + evt.moveAmount;
            targetObject.InfoManager.ChangeInfo(InfoType.Position, newPos);

            if (!evt.isHistory)
            {
                RaiseHistoryEvent(evt.moveAmount);
            }
        }

        private void RaiseHistoryEvent(Vector2 moveAmount)
        {
            RegisterHistoryEvent evt = MapMakerEvent.RegisterHistoryEvent;
            evt.history = new PositionHistory(mapMakerChannel, _currentObject, moveAmount);
            mapMakerChannel.RaiseEvent(evt);
        }
    }
}