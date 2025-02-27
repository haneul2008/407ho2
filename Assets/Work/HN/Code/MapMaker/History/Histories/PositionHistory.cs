using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public class PositionHistory : ObjectHistory
    {
        private readonly Vector2 _moveAmount;

        public PositionHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObject, Vector2 moveAmount) : base(mapMakerChannel, targetObject)
        {
            _moveAmount = moveAmount;
        }

        public override void Undo()
        {
            RaiseMoveEvent(-_moveAmount);
        }

        public override void Redo()
        {
            RaiseMoveEvent(_moveAmount);
        }

        private void RaiseMoveEvent(Vector2 moveAmount)
        {
            MoveEvent evt = MapMakerEvent.MoveEvent;
            evt.targetObject = _targetObject;
            evt.moveAmount = moveAmount;
            evt.isHistory = true;
            _mapMakerChannel.RaiseEvent(evt);
        }
    }
}