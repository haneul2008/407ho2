using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public abstract class EditableMono : MonoBehaviour
    {
        [SerializeField] protected GameEventChannelSO mapMakerChannel;
        
        protected EditorObject _currentObject;

        public abstract EditType EditType { get; }
        protected bool _isActive;

        public virtual void Initialize()
        {
            mapMakerChannel.AddListener<EditModeChangeEvent>(HandleEditModeChange);
            mapMakerChannel.AddListener<CurrentObjectChangeEvent>(HandleCurrentObjectChange);
        }

        protected virtual void OnDestroy()
        {
            mapMakerChannel.RemoveListener<EditModeChangeEvent>(HandleEditModeChange);
            mapMakerChannel.RemoveListener<CurrentObjectChangeEvent>(HandleCurrentObjectChange);
        }

        protected virtual void HandleCurrentObjectChange(CurrentObjectChangeEvent evt)
        {
            _currentObject = evt.currentObject;
        }

        private void HandleEditModeChange(EditModeChangeEvent evt)
        {
            bool isValueChange = _isActive != (evt.editType == EditType);
            
            if (isValueChange)
            {
                OnActive(!_isActive);
                _isActive = !_isActive;
            }
        }

        protected virtual void OnActive(bool isActive)
        {
        }
    }
}