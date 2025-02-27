using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public abstract class MoveTriggerPanel<T> : TriggerEditUI where T : ITriggerInfo
    {
        [SerializeField] protected TMP_InputField xAmountField, yAmountField, durationField;

        protected T _moveInfo;
        
        public override void Initialize(GameEventChannelSO mapMakerChannel)
        {
            base.Initialize(mapMakerChannel);
            
            xAmountField.onValueChanged.AddListener(HandleXAmountChanged);
            yAmountField.onValueChanged.AddListener(HandleYAmountChanged);
            durationField.onValueChanged.AddListener(HandleDurationChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            xAmountField.onValueChanged.RemoveListener(HandleXAmountChanged);
            yAmountField.onValueChanged.RemoveListener(HandleYAmountChanged);
            durationField.onValueChanged.RemoveListener(HandleDurationChanged);
        }

        private void HandleDurationChanged(string value)
        {
            if (_isWithoutNotify) return;
            
            if (float.TryParse(value, out float duration))
            {
                RaiseEvents(_moveInfo, () =>
                {
                    _moveInfo = OnDurationChanged(duration);
                    return _moveInfo;
                });
            }
        }

        protected abstract T OnDurationChanged(float duration);

        private void HandleYAmountChanged(string value)
        {
            if (_isWithoutNotify) return;
                
            if (float.TryParse(value, out float yAmount))
            {
                RaiseEvents(_moveInfo, () =>
                {
                    _moveInfo = OnYChanged(yAmount);
                    return _moveInfo;
                });
            }
        }
        
        protected abstract T OnYChanged(float yAmount);

        private void HandleXAmountChanged(string value)
        {
            if (_isWithoutNotify) return;
                
            if (float.TryParse(value, out float xAmount))
            {
                RaiseEvents(_moveInfo, () =>
                {
                    _moveInfo = OnXChanged(xAmount);
                    return _moveInfo;
                });
            }
        }
        
        protected abstract T OnXChanged(float xAmount);

        public override void SetTrigger(EditorTrigger targetTrigger)
        {
            base.SetTrigger(targetTrigger);

            SetInputFieldText(targetTrigger);
        }

        protected abstract void SetInputFieldText(EditorTrigger targetTrigger);
    }
}