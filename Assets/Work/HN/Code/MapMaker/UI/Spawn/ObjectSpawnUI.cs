using Ami.BroAudio;
using UnityEngine;
using UnityEngine.EventSystems;
using Work.HN.Code.EventSystems;

namespace Work.HN.Code.MapMaker.UI.Spawn
{
    public class ObjectSpawnUI : ObjectSpawnableUI
    {
        [SerializeField] private SoundID clickSoundID;
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            BroAudio.Play(clickSoundID);
            
            ObjectSelectEvent selectEvt = MapMakerEvent.ObjectSelectEvent;
            selectEvt.selectedObject = _spawnable;
            mapMakerChannel.RaiseEvent(selectEvt);
        }
    }
}