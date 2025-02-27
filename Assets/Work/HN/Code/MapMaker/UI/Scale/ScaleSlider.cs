using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.UI.Scale
{
    public class ScaleSlider : HistoryRegistrySlider
    {
        protected override void SetOriginValue()
        {
            _originValue = _currentObject.GetSize().x;
        }

        protected override void RaiseHistoryEvent(float value)
        {
            RegisterHistoryEvent evt = MapMakerEvent.RegisterHistoryEvent;
            evt.history = new ScaleHistory(_mapMakerChannel, _currentObject, _originValue, value);
            _mapMakerChannel.RaiseEvent(evt);
        }
    }
}