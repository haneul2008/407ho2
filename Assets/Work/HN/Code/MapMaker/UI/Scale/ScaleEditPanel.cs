using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.ObjectManagement;

namespace Work.HN.Code.MapMaker.UI.Scale
{
    public class ScaleEditPanel : ValueChangeablePanel<ScaleEditor>
    {
        [SerializeField] private ScaleSlider scaleSlider;
        [SerializeField] private float minScale, maxScale;

        private TextMeshProUGUI _valueText;

        protected override void Awake()
        {
            base.Awake();

            _valueText = GetComponentInChildren<TextMeshProUGUI>();
            
            scaleSlider.Initialize(mapMakerChannel, minScale, maxScale);

            scaleSlider.OnValueChanged += HandleValueChanged;
            mapMakerChannel.AddListener<ScaleChangeEvent>(HandleScaleChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            scaleSlider.OnValueChanged -= HandleValueChanged;
            mapMakerChannel.RemoveListener<ScaleChangeEvent>(HandleScaleChanged);
        }
        
        private void HandleScaleChanged(ScaleChangeEvent evt)
        {
            if(!evt.isHistory) return;
            
            _isWithoutNotify = true;

            scaleSlider.SetValue(evt.scale);
        }

        protected override void HandleCurrentObjectChanged(CurrentObjectChangeEvent evt)
        {
            base.HandleCurrentObjectChanged(evt);
            
            if (evt.currentObject == null)
            {
                scaleSlider.SetValue(1f);
                return;
            }
            
            _isWithoutNotify = true;
            
            Vector3 objSize = evt.currentObject.GetSize();
            scaleSlider.SetValue(objSize.x);
            scaleSlider.SetCurrentObject(_currentObject);
        }

        private void HandleValueChanged(float value)
        {
            _valueText.text = $"x{value}";
            
            if (_isWithoutNotify)
            {
                _isWithoutNotify = false;
                return;
            }

            RaiseScaleEvent(value);
        }

        private void RaiseScaleEvent(float value)
        {
            ScaleChangeEvent scaleEvt = MapMakerEvent.ScaleChangeEvent;
            scaleEvt.targetObject = _currentObject;
            scaleEvt.scale = value;
            scaleEvt.isHistory = false;
            mapMakerChannel.RaiseEvent(scaleEvt);
        }
    }
}