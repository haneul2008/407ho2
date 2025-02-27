using UnityEngine;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public abstract class SpawnOrDestroyPanel : TriggerEditUI
    {
        protected SpawnOrDestroyInfo _info;

        public override void SetTrigger(EditorTrigger targetTrigger)
        {
            base.SetTrigger(targetTrigger);
            
            _info = targetTrigger.GetInfo<SpawnOrDestroyInfo>();
        }

        protected override void OnChangeID(int id)
        {
            RaiseEvents(_info, () =>
            {
                _info.ID = id;
                return _info;
            });
        }
    }
}