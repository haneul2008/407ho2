using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class ColorEditor : EditableMono
    {
        public override EditType EditType => EditType.Color;

        public override void Initialize()
        {
            base.Initialize();
            
            mapMakerChannel.AddListener<ColorChangeEvent>(HandleColorChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<ColorChangeEvent>(HandleColorChanged);
        }

        private void HandleColorChanged(ColorChangeEvent evt)
        {
            if(!_isActive) return;
            
            EditorObject targetObject = evt.targetObject;
            
            if(targetObject == null) return;
            
            targetObject.InfoManager.ChangeInfo(InfoType.Color, evt.color);
        }
    }
}