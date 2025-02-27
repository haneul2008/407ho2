using System;
using UnityEngine;
using Work.HN.Code.MapMaker.Core;

namespace Work.HN.Code.MapMaker.Objects.Triggers
{
    [Serializable]
    public struct AlphaInfo : ITriggerInfo
    {
        public int ID { get; set; }
        public float endValue;
        public float duration;
    }
    
    public class EditorAlphaTrigger : EditorTrigger
    {
        public override void Spawn()
        {
            base.Spawn();

            _myInfo = new AlphaInfo()
            {
                ID = _targetTriggerId,
                endValue = 0f,
                duration = 0f
            };
        }
        
        public override void SetData(ITriggerInfo info)
        {
            base.SetData(info);

            SetMyInfo<AlphaInfo>(info);
        }
    }
}