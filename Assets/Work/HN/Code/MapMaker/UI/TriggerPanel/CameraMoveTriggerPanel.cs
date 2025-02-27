using UnityEngine;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public class CameraMoveTriggerPanel : MoveTriggerPanel<MoveInfo>
    {
        public override TriggerType TriggerType => TriggerType.CameraMove;
        
        protected override void OnChangeID(int id)
        {
            RaiseEvents(_moveInfo, () =>
            {
                _moveInfo.ID = id;
                return _moveInfo;
            });
        }

        protected override MoveInfo OnDurationChanged(float duration)
        {
            _moveInfo.duration = duration;
            
            return _moveInfo;
        }

        protected override MoveInfo OnYChanged(float yAmount)
        {
            _moveInfo.moveAmount.y = yAmount;
            
            return _moveInfo;
        }

        protected override MoveInfo OnXChanged(float xAmount)
        {
            _moveInfo.moveAmount.x = xAmount;

            return _moveInfo;
        }

        protected override void SetInputFieldText(EditorTrigger targetTrigger)
        {
            _moveInfo = targetTrigger.GetInfo<MoveInfo>();
            
            _isWithoutNotify = true;
            
            xAmountField.text = _moveInfo.moveAmount.x.ToString();
            yAmountField.text = _moveInfo.moveAmount.y.ToString();
            durationField.text = _moveInfo.duration.ToString();
            
            _isWithoutNotify = false;
        }
    }
}