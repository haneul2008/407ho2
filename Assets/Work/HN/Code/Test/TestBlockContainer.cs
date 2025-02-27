using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.Test
{
    public class TestBlockContainer : MonoBehaviour
    {
        [SerializeField] private List<EditorObject> testBlocks = new List<EditorObject>();
        [SerializeField] private int testIndex;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        
        [ContextMenu("Change block")]
        public void ChangeBlock()
        {
            ObjectSelectEvent selectEvent = MapMakerEvent.ObjectSelectEvent;
            selectEvent.selectedObject = testBlocks[testIndex];
            print(testBlocks[testIndex]);
            
            mapMakerChannel.RaiseEvent(selectEvent);
        }
    }
}