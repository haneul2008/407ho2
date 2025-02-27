using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public class AlphaTriggerPanel : TriggerEditUI
    {
        public override TriggerType TriggerType => TriggerType.Alpha;

        [SerializeField] private TMP_InputField endValueField, durationField;
        
        private AlphaInfo _alphaInfo;

        public override void Initialize(GameEventChannelSO mapMakerChannel)
        {
            base.Initialize(mapMakerChannel);
            
            endValueField.onValueChanged.AddListener(HandleEndValueChanged);
            durationField.onValueChanged.AddListener(HandleDurationChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            endValueField.onValueChanged.RemoveListener(HandleEndValueChanged);
            durationField.onValueChanged.RemoveListener(HandleDurationChanged);
        }

        private void HandleDurationChanged(string value)
        {
            if (_isWithoutNotify) return;

            if (int.TryParse(value, out int duration))
            {
                RaiseEvents(_alphaInfo, () =>
                {
                    _alphaInfo.duration = duration;
                    return _alphaInfo;
                });
            }
        }

        private void HandleEndValueChanged(string value)
        {
            if (_isWithoutNotify) return;

            if (int.TryParse(value, out int endValue))
            {
                RaiseEvents(_alphaInfo, () =>
                {
                    _alphaInfo.endValue = endValue;
                    return _alphaInfo;
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
            _alphaInfo = targetTrigger.GetInfo<AlphaInfo>();

            _isWithoutNotify = true;
            
            endValueField.text = _alphaInfo.endValue.ToString();
            durationField.text = _alphaInfo.duration.ToString();
            
            _isWithoutNotify = false;
        }

        protected override void OnChangeID(int id)
        {
            RaiseEvents(_alphaInfo, () =>
            {
                _alphaInfo.ID = id;
                return _alphaInfo;
            });
        }
    }
}