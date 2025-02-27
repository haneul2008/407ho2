using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.UI.Delete
{
    public class DeleteBtn : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image objectImage;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        
        private EditorObject _targetObject;
        
        public void Initialize(EditorObject targetObject)
        {
            _targetObject = targetObject;
            
            SpriteRenderer targetRenderer = _targetObject.GetComponent<SpriteRenderer>();//나중에 SO로 변경
            
            objectImage.sprite = targetRenderer.sprite;
            objectImage.color = targetRenderer.color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            BulkDeleteEvent bulkDeleteEvent = MapMakerEvent.BulkDeleteEvent;
            bulkDeleteEvent.targetObject = _targetObject;
            mapMakerChannel.RaiseEvent(bulkDeleteEvent);
        }
    }
}