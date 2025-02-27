using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class SortingOrderEditor : EditableMono
    {
        public override EditType EditType => EditType.SortingOrder;

        public override void Initialize()
        {
            base.Initialize();
            
            mapMakerChannel.AddListener<SortingOrderChangeEvent>(HandleSortingOrderChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<SortingOrderChangeEvent>(HandleSortingOrderChanged);
        }

        private void HandleSortingOrderChanged(SortingOrderChangeEvent evt)
        {
            EditorObject targetObject = evt.targetObject;
            
            if(targetObject == null) return;

            int beforeOrder = targetObject.GetSortingOrder();
            
            targetObject.InfoManager.ChangeInfo(InfoType.SortingOrder, evt.sortingOrder);

            if (!evt.isHistory)
            {
                RaiseHistoryEvent(beforeOrder, targetObject);
            }
        }

        private void RaiseHistoryEvent(int beforeOrder, EditorObject targetObject)
        {
            RegisterHistoryEvent evt = MapMakerEvent.RegisterHistoryEvent;
            evt.history = new SortingOrderHistory(mapMakerChannel, targetObject, beforeOrder, targetObject.GetSortingOrder());
            mapMakerChannel.RaiseEvent(evt);
        }
    }
}