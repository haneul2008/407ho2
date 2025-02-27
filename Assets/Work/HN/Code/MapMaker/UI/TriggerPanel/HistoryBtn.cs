using UnityEngine;
using Work.HN.Code.EventSystems;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public class HistoryBtn : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private bool isUndo; //true : undo, false : redo
        
        public void OnClick()
        {
            UndoOrRedoEvent evt = MapMakerEvent.UndoOrRedoEvent;
            evt.isUndo = isUndo;
            evt.isRedo = !isUndo;
            mapMakerChannel.RaiseEvent(evt);
        }
    }
}