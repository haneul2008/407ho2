using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.History;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.UI
{
    public abstract class HistoryRegistrySlider : MonoBehaviour, IPointerUpHandler
    {
        public event Action<float> OnValueChanged;
        
        protected GameEventChannelSO _mapMakerChannel;
        protected Slider _slider;
        protected EditorObject _currentObject;
        protected float _originValue;

        public virtual void Initialize(GameEventChannelSO mapMakerChannel, float minValue, float maxValue)
        {
            _slider = GetComponent<Slider>();
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
            _mapMakerChannel = mapMakerChannel;
            
            _slider.onValueChanged.AddListener(HandleValueChanged);
        }
        
        protected virtual void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(HandleValueChanged);
        }
        
        public void SetValue(float value)
        {
            _slider.value = value;
        }

        public virtual void SetCurrentObject(EditorObject currentObject)
        {
            _currentObject = currentObject;
            
            SetOriginValue();
        }
        
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if(_currentObject == null) return;
            
            RaiseHistoryEvent(_slider.value);
            
            _originValue = _slider.value;
        }
        
        protected virtual void HandleValueChanged(float value)
        {
            if(_currentObject == null) return;

            OnValueChanged?.Invoke(value);
        }

        protected abstract void SetOriginValue();
        
        protected abstract void RaiseHistoryEvent(float value);
    }
}