using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.ObjectManagement;

namespace Work.HN.Code.MapMaker.UI.Color
{
    public enum ColorType
    {
        Red,
        Green,
        Blue
    }
    
    public class ColorEditPanel : ValueChangeablePanel<ColorEditor>
    {
        [SerializeField] private Image previewImage;
        
        private List<ColorEditContainer> _containers = new List<ColorEditContainer>();

        protected override void Awake()
        {
            base.Awake();

            mapMakerChannel.AddListener<ColorChangeEvent>(HandleColorChanged);
            
            _containers = GetComponentsInChildren<ColorEditContainer>().ToList();
            _containers.ForEach(container => container.Initialize());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<ColorChangeEvent>(HandleColorChanged);
        }

        private void HandleColorChanged(ColorChangeEvent evt)
        {
            previewImage.color = evt.color;
        }

        protected override void HandleCurrentObjectChanged(CurrentObjectChangeEvent evt)
        {
            base.HandleCurrentObjectChanged(evt);
            
            foreach (ColorEditContainer container in _containers)
            {
                container.SetCurrentObject(_currentObject);
            }

            if (_currentObject == null) return;
            
            previewImage.color = _currentObject.GetColor();
        }
    }
}