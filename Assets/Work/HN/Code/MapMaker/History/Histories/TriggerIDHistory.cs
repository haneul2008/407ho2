using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public class TriggerIDHistory : ObjectHistory
    {
        private readonly int? _beforeID;
        private readonly int? _afterID;
        
        public TriggerIDHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObject, int? beforeID, int? afterID) : base(mapMakerChannel, targetObject)
        {
            _beforeID = beforeID;
            _afterID = afterID;
        }

        public override void Undo()
        {
            RaiseTriggerIDEvent(_beforeID);
        }

        public override void Redo()
        {
            RaiseTriggerIDEvent(_afterID);
        }

        private void RaiseTriggerIDEvent(int? id)
        {
            TriggerIDChangeEvent triggerIDEvent = MapMakerEvent.TriggerIDChangeEvent;
            triggerIDEvent.targetObject = _targetObject;
            triggerIDEvent.id = id;
            triggerIDEvent.isHistory = true;
            _mapMakerChannel.RaiseEvent(triggerIDEvent);
        }
    }
}