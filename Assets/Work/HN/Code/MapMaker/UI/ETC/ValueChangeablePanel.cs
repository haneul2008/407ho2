using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.ObjectManagement;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.UI
{
    public class ValueChangeablePanel<T> : ObjectEditPanel<T> where T : EditableMono
    {
        protected bool _isWithoutNotify;
        protected EditorObject _currentObject;

        protected override void Awake()
        {
            base.Awake();
            
            mapMakerChannel.AddListener<CurrentObjectChangeEvent>(HandleCurrentObjectChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<CurrentObjectChangeEvent>(HandleCurrentObjectChanged);
        }

        protected virtual void HandleCurrentObjectChanged(CurrentObjectChangeEvent evt)
        {
            _currentObject = evt.currentObject;
        }
    }
}