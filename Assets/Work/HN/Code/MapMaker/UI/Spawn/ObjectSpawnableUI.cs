using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.UI.Spawn
{
    public abstract class ObjectSpawnableUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] protected GameEventChannelSO mapMakerChannel;
        [SerializeField] protected Image objectImage;

        protected EditorObject _spawnable;
        protected int _id;
        
        public virtual void Initialize(EditorObject editorObject) 
        {
            _id = editorObject.ID;
            
            _spawnable = editorObject;
            
            SpriteRenderer targetRenderer = editorObject.GetComponent<SpriteRenderer>();//나중에 SO로 변경
            
            objectImage.sprite = targetRenderer.sprite;
            objectImage.color = targetRenderer.color;
        }
        
        public abstract void OnPointerClick(PointerEventData eventData);
    }
}