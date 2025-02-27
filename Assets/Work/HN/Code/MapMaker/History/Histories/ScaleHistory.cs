using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public class ScaleHistory : ObjectHistory
    {
        private readonly float _beforeScale;
        private readonly float _afterScale;
        
        public ScaleHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObject, float beforeScale, float afterScale) : base(mapMakerChannel, targetObject)
        {
            _beforeScale = beforeScale;
            _afterScale = afterScale;
        }

        public override void Undo()
        {
            RaiseScaleEvent(_beforeScale);
        }

        public override void Redo()
        {
            RaiseScaleEvent(_afterScale);
        }

        private void RaiseScaleEvent(float afterScale)
        {
            ScaleChangeEvent evt = MapMakerEvent.ScaleChangeEvent;
            evt.targetObject = _targetObject;
            evt.scale = afterScale;
            evt.isHistory = true;
            _mapMakerChannel.RaiseEvent(evt);
        }
    }
}