using System;
using UnityEngine;
using Work.HN.Code.MapMaker.Core;

namespace Work.HN.Code.MapMaker.Objects.Triggers
{
    [Serializable]
    public struct ShakeInfo : ITriggerInfo
    {
        public int ID { get; set; }
        public float strength;
        public float duration;
    }
    
    public class EditorShakeTrigger : EditorTrigger
    {
        public override void Spawn()
        {
            base.Spawn();

            _myInfo = new ShakeInfo
            {
                ID = _targetTriggerId,
                strength = 0f,
                duration = 0f
            };
        }

        public override void SetData(ITriggerInfo info)
        {
            base.SetData(info);
            
            SetMyInfo<ShakeInfo>(info);
        }
    }
}