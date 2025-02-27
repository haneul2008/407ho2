using System;
using UnityEngine;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.UI;

namespace Work.HN.Code.MapMaker.Objects.Triggers
{
    [Serializable]
    public struct MoveInfo : ITriggerInfo
    {
        public int ID { get; set; }
        public Vector2 moveAmount;
        public float duration;
    }
    
    public class EditorObjectMoveTrigger : EditorTrigger
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