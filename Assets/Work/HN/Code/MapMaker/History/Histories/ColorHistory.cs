using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public class ColorHistory : ObjectHistory
    {
        private readonly Color _beforeColor;
        private readonly Color _afterColor;
        
        public ColorHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObject, Color beforeColor, Color afterColor) : base(mapMakerChannel, targetObject)
        {
            _beforeColor = beforeColor;
            _afterColor = afterColor;
        }

        public override void Undo()
        {
            RaiseColorEvent(_beforeColor);
        }

        public override void Redo()
        {
            RaiseColorEvent(_afterColor);
        }

        private void RaiseColorEvent(Color color)
        {
            ColorChangeEvent evt = MapMakerEvent.ColorChangeEvent;
            evt.targetObject = _targetObject;
            evt.color = color;
            evt.isHistory = true;
            _mapMakerChannel.RaiseEvent(evt);
        }
    }
}