using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public class SortingOrderHistory : ObjectHistory
    {
        private readonly int _beforeOrder;
        private readonly int _afterOrder;
        
        public SortingOrderHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObject, int beforeOrder, int afterOrder) : base(mapMakerChannel, targetObject)
        {
            _afterOrder = afterOrder;
            _beforeOrder = beforeOrder;
        }

        public override void Undo()
        {
            RaiseSortingOrderEvent(_beforeOrder);
        }

        public override void Redo()
        {
            RaiseSortingOrderEvent(_afterOrder);
        }

        private void RaiseSortingOrderEvent(int order)
        {
            SortingOrderChangeEvent evt = MapMakerEvent.SortingOrderChangeEvent;
            evt.targetObject = _targetObject;
            evt.sortingOrder = order;
            evt.isHistory = true;
            _mapMakerChannel.RaiseEvent(evt);
        }
    }
}