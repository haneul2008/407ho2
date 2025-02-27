using UnityEngine;
using Work.HN.Code.MapMaker.Core;

namespace Work.HN.Code.MapMaker.Objects.Triggers
{
    public class EditorCameraMoveTrigger : EditorTrigger
    {
        public override void Spawn()
        {
            base.Spawn();

            _myInfo = new MoveInfo
            {
                ID = _targetTriggerId,
                moveAmount = Vector2.zero,
                duration = 0f
            };
        }

        public override void SetData(ITriggerInfo info)
        {
            base.SetData(info);

            SetMyInfo<MoveInfo>(info);
        }
    }
}