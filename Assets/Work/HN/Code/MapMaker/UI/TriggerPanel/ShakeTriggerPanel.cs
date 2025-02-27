using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public class ShakeTriggerPanel : TriggerEditUI
    {
        public override TriggerType TriggerType => TriggerType.Shake;
        
        [SerializeField] private TMP_InputField strengthField, durationField;
        
        private ShakeInfo _shakeInfo;

        public override void Initialize(GameEventChannelSO mapMakerChannel)
        {
            base.Initialize(mapMakerChannel);
            
            strengthField.onValueChanged.AddListener(HandleStrengthChanged);
            durationField.onValueChanged.AddListener(HandleDurationChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            strengthField.onValueChanged.RemoveListener(HandleStrengthChanged);
            durationField.onValueChanged.RemoveListener(HandleDurationChanged);
        }

        private void HandleDurationChanged(string value)
        {
            if(_isWithoutNotify) return;

            if (int.TryParse(value, out int strength))
            {
                RaiseEvents(_shakeInfo, () =>
                {
                    _shakeInfo.strength = strength;
                    return _shakeInfo;
                });
            }
        }

        private void HandleStrengthChanged(string value)
        {
            if(_isWithoutNotify) return;

            if (int.TryParse(value, out int duration))
            {
                RaiseEvents(_shakeInfo, () =>
                {
                    _shakeInfo.duration = duration;
                    return _shakeInfo;
                });
            }
        }

        public override void SetTrigger(EditorTrigger targetTrigger)
        {
            base.SetTrigger(targetTrigger);

            SetInputFieldText(targetTrigger);
        }

        private void SetInputFieldText(EditorTrigger targetTrigger)
        {
            _shakeInfo = targetTrigger.GetInfo<ShakeInfo>();

            _isWithoutNotify = true;
            
            strengthField.text = _shakeInfo.strength.ToString();
            durationField.text = _shakeInfo.duration.ToString();
            
            _isWithoutNotify = false;
        }

        protected override void OnChangeID(int id)
        {
            RaiseEvents(_shakeInfo, () =>
            {
                _shakeInfo.ID = id;
                return _shakeInfo;
            });
        }
    }
}