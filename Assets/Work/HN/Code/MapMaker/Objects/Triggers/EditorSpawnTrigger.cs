using System;
using UnityEngine;
using Work.HN.Code.MapMaker.Core;

namespace Work.HN.Code.MapMaker.Objects.Triggers
{
    [Serializable]
    public struct SpawnOrDestroyInfo : ITriggerInfo
    {
        public int ID { get; set; }
    }
    
    public class EditorSpawnTrigger : EditorTrigger
    {   
        public override void Spawn()
        {
            base.Spawn();

            _myInfo = new SpawnOrDestroyInfo
            {
                ID = _targetTriggerId
            };
        }

        public override void SetData(ITriggerInfo info)
        {
            base.SetData(info);
            
            SetMyInfo<SpawnOrDestroyInfo>(info);
        }
    }
}