using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.UI.Color
{
    public class ColorEditContainer : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private ColorType colorType;
        [SerializeField] private TextMeshProUGUI valueText;
        
        private ColorSlider _slider;
        private bool _isWithoutNotify;
        private EditorObject _currentObject;

        public void Initialize()
        {
            _slider = GetComponentInChildren<ColorSlider>();
            _slider.Initialize(mapMakerChannel, 0f, 1f, colorType);
            
            _slider.OnValueChanged += HandleValueChanged;
            mapMakerChannel.AddListener<ColorChangeEvent>(HandleColorChanged);
        }

        private void OnDestroy()
        {
            _slider.OnValueChanged -= HandleValueChanged;
            mapMakerChannel.RemoveListener<ColorChangeEvent>(HandleColorChanged);
        }

        private void HandleColorChanged(ColorChangeEvent evt)
        {
            if(!evt.isHistory) return;

            float value = colorType switch
            {
                ColorType.Red => evt.color.r,
                ColorType.Green => evt.color.g,
                ColorType.Blue => evt.color.b,
                _ => 0f
            };
            
            _isWithoutNotify = true;
            _slider.SetValue(value);
        }

        public void SetCurrentObject(EditorObject currentObject)
        {
            _currentObject = currentObject;
            
            if(_currentObject == null) return;
            
            _slider.SetCurrentObject(_currentObject);
            
            _isWithoutNotify = true;
            
            UnityEngine.Color targetColor = _currentObject.GetColor();
            float sliderValue;
            
            switch (colorType)
            {
                case ColorType.Red:
                    sliderValue = targetColor.r;
                    break;
                
                case ColorType.Green:
                    sliderValue = targetColor.g;
                    break;
                
                case ColorType.Blue:
                    sliderValue = targetColor.b;
                    break;
                
                default:
                    sliderValue = 0;
                    break;
            }
            
            _slider.SetValue(sliderValue);
        }
        
        private void HandleValueChanged(float value)
        {
            valueText.text = value.ToString();
            
            if (_isWithoutNotify)
            {
                _isWithoutNotify = false;
                return;
            }
            
            if(_currentObject == null) return;

            ColorChangeEvent colorEvt = MapMakerEvent.ColorChangeEvent;
            colorEvt.targetObject = _currentObject;
            colorEvt.isHistory = false;
            
            UnityEngine.Color color = _currentObject.GetColor();

            switch (colorType)
            {
                case ColorType.Red:
                    colorEvt.color = new UnityEngine.Color(value, color.g, color.b);
                    break;
                
                case ColorType.Green:
                    colorEvt.color = new UnityEngine.Color(color.r, value, color.b);
                    break;
                
                case ColorType.Blue:
                    colorEvt.color = new UnityEngine.Color(color.r, color.g, value);
                    break;
                
                default:
                    colorEvt.color = UnityEngine.Color.white;
                    break;
            }
            
            mapMakerChannel.RaiseEvent(colorEvt);
        }
    }
}