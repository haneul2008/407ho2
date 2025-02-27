using UnityEngine.EventSystems;
using Work.HN.Code.EventSystems;

namespace Work.HN.Code.MapMaker.UI.Spawn
{
    public class ObjectSpawnUI : ObjectSpawnableUI
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            ObjectSelectEvent selectEvt = MapMakerEvent.ObjectSelectEvent;
            selectEvt.selectedObject = _spawnable;
            mapMakerChannel.RaiseEvent(selectEvt);
        }
    }
}