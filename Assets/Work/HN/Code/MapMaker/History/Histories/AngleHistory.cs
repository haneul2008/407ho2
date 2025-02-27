using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public class AngleHistory : ObjectHistory
    {
        private readonly float _beforeAngle;
        private readonly float _afterAngle;
        
        public AngleHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObject, float beforeAngle, float afterAngle) : base(mapMakerChannel, targetObject)
        {
            _beforeAngle = beforeAngle;
            _afterAngle = afterAngle;
        }

        public override void Undo()
        {
            RaiseAngleEvent(_beforeAngle);
        }

        public override void Redo()
        {
            RaiseAngleEvent(_afterAngle);
        }

        private void RaiseAngleEvent(float angle)
        {
            AngleChangeEvent evt = MapMakerEvent.AngleChangeEvent;
            evt.targetObject = _targetObject;
            evt.angle = angle;
            evt.isHistory = true;
            _mapMakerChannel.RaiseEvent(evt);
        }
    }
}