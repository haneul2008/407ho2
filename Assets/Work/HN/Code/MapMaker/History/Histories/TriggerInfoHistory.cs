using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public class TriggerInfoHistory : ObjectHistory
    {
        private EditorTrigger _targetTrigger;
        private readonly ITriggerInfo _beforeInfo;
        private readonly ITriggerInfo _afterInfo;
        
        public TriggerInfoHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObject, ITriggerInfo beforeInfo, ITriggerInfo afterInfo) : base(mapMakerChannel, targetObject)
        {
            if (targetObject is EditorTrigger trigger)
            {
                _targetTrigger = trigger;
            }
            else
            {
                Debug.LogError("not trigger type");
                return;
            }
            
            _beforeInfo = beforeInfo;
            _afterInfo = afterInfo;
        }

        public override void Undo()
        {
            RaiseTriggerInfoEvent(_beforeInfo);
        }

        public override void Redo()
        {
            RaiseTriggerInfoEvent(_afterInfo);
        }

        private void RaiseTriggerInfoEvent(ITriggerInfo info)
        {
            TriggerInfoChangeEvent evt = MapMakerEvent.TriggerInfoChangeEvent;
            evt.targetTrigger = _targetTrigger;
            evt.info = info;
            evt.isHistory = true;
            _mapMakerChannel.RaiseEvent(evt);
        }

        protected override void HandleSubstanceChanged(ChangeSubstanceInHistoryEvent evt)
        {
            if (evt.beforeObj is EditorTrigger beforeTrigger && evt.afterObj is EditorTrigger afterTrigger)
            {
                if (_targetTrigger == beforeTrigger)
                {
                    _targetTrigger = afterTrigger;
                }
            }
        }
    }
}