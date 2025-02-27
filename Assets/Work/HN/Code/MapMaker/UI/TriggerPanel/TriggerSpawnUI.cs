using UnityEngine.EventSystems;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.UI.Spawn;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public class TriggerSpawnUI : ObjectSpawnableUI
    {
        private EditorTrigger _trigger;

        public override void Initialize(EditorObject editorObject)
        {
            base.Initialize(editorObject);
            
            _trigger = editorObject as EditorTrigger;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            TriggerSelectEvent evt = MapMakerEvent.TriggerSelectEvent;
            evt.selectedTrigger = _trigger;
            mapMakerChannel.RaiseEvent(evt);
        }
    }
}