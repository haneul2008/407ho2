using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.ObjectManagement;

namespace Work.HN.Code.MapMaker.UI.Angle
{
    public class AngleEditPanel : ValueChangeablePanel<AngleEditor>
    {
        [SerializeField] private TMP_InputField angleField;

        private float _originAngle;
        private bool _isDrag;
        
        protected override void Awake()
        {
            base.Awake();
            
            angleField.onValueChanged.AddListener(HandleValueChanged);
            mapMakerChannel.AddListener<AngleDragEvent>(HandleAngleDragEvent);
            mapMakerChannel.AddListener<AngleChangeEvent>(HandleAngleChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            angleField.onValueChanged.RemoveListener(HandleValueChanged);
            mapMakerChannel.RemoveListener<AngleDragEvent>(HandleAngleDragEvent);
            mapMakerChannel.RemoveListener<AngleChangeEvent>(HandleAngleChanged);
        }

        private void HandleAngleChanged(AngleChangeEvent evt)
        {
            if(!evt.isHistory) return;
            
            _isWithoutNotify = true;
            angleField.text = evt.angle.ToString();
        }

        private void HandleAngleDragEvent(AngleDragEvent evt)
        {
            if(!_isActive || _currentObject == null) return;

            if (evt.isPointerUp)
            {
                RaiseHistoryEvent(evt.angle);
                _originAngle = evt.angle;
            }
            
            _isDrag = true;
            SetRoundedAngle(evt.angle);
        }

        protected override void HandleCurrentObjectChanged(CurrentObjectChangeEvent evt)
        {
            base.HandleCurrentObjectChanged(evt);
            
            if (_currentObject == null)
            {
                angleField.text = string.Empty;
                return;
            }
            
            _isWithoutNotify = true;

            float angle = _currentObject.GetAngle();

            _originAngle = angle;
            
            SetRoundedAngle(angle);
        }

        private void HandleValueChanged(string value)
        {
            if (_isWithoutNotify)
            {
                _isWithoutNotify = false;
                return;
            }
            
            if (float.TryParse(value, out float angle))
            {
                RaiseAngleChangeEvent(angle);

                if (angle >= 360)
                {
                    _isWithoutNotify = true;
                    angleField.text = $"{angle - 360}";
                }

                if (_isDrag)
                {
                    _isDrag = false;
                }
                else
                {
                    RaiseHistoryEvent(angle);
                }
            }
            else
            {
                Debug.LogWarning("Invalid angle value");
                angleField.text = string.Empty;
            }
        }

        private void RaiseAngleChangeEvent(float angle)
        {
            AngleChangeEvent angleEvt = MapMakerEvent.AngleChangeEvent;
            angleEvt.targetObject = _currentObject;
            angleEvt.angle = angle;
            angleEvt.isHistory = false;
            mapMakerChannel.RaiseEvent(angleEvt);
        }

        public void AddValue(float value)
        {
            if (float.TryParse(angleField.text, out float angle))
            {
                _originAngle = angle;
                
                float afterAngle = angle + value;
                
                SetRoundedAngle(afterAngle);
            }
        }

        private void SetRoundedAngle(float angle)
        {
            float roundedAngle = Mathf.RoundToInt(angle);
            angleField.text = roundedAngle.ToString();
        }

        private void RaiseHistoryEvent(float angle)
        {
            if(_currentObject == null) return;
            
            RegisterHistoryEvent evt = MapMakerEvent.RegisterHistoryEvent;
            evt.history = new AngleHistory(mapMakerChannel, _currentObject, _originAngle, angle);
            mapMakerChannel.RaiseEvent(evt);
        }
    }
}