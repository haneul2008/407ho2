using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.History.Histories;

namespace Work.HN.Code.MapMaker.UI.Color
{
    public class ColorSlider : HistoryRegistrySlider
    {
        private ColorType _colorType;
        
        public void Initialize(GameEventChannelSO mapMakerChannel, float minValue, float maxValue, ColorType colorType)
        {
            base.Initialize(mapMakerChannel, minValue, maxValue);
            
            _colorType = colorType;
        }

        protected override void SetOriginValue()
        {
            UnityEngine.Color color = _currentObject.GetColor();

            _originValue = _colorType switch
            {
                ColorType.Red => color.r,
                ColorType.Green => color.g,
                ColorType.Blue => color.b,
                _ => 0f
            };
        }

        protected override void RaiseHistoryEvent(float value)
        {
            UnityEngine.Color originColor = GetColor(_originValue);
            UnityEngine.Color afterColor = _currentObject.GetColor();
            
            RegisterHistoryEvent evt = MapMakerEvent.RegisterHistoryEvent;
            evt.history = new ColorHistory(_mapMakerChannel, _currentObject, originColor, afterColor);
            _mapMakerChannel.RaiseEvent(evt);
        }

        private UnityEngine.Color GetColor(float targetValue)
        {
            UnityEngine.Color color = _currentObject.GetColor();

            return _colorType switch
            {
                ColorType.Red => new UnityEngine.Color(targetValue, color.g, color.b),
                ColorType.Green => new UnityEngine.Color(color.r, targetValue, color.b),
                ColorType.Blue => new UnityEngine.Color(color.r, color.g, targetValue),
                _ => UnityEngine.Color.white
            };
        }
    }
}