using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class ColorEditor : EditableMono
    {
        public override EditType EditType => EditType.Color;

        protected override void OnActive(bool isActive)
        {
            base.OnActive(isActive);
            
            if(isActive)
                mapMakerChannel.AddListener<ColorChangeEvent>(HandleColorChanged);
            else
                mapMakerChannel.RemoveListener<ColorChangeEvent>(HandleColorChanged);
        }

        private void HandleColorChanged(ColorChangeEvent evt)
        {
            EditorObject targetObject = evt.targetObject;
            
            if(targetObject == null) return;
            
            targetObject.InfoManager.ChangeInfo(InfoType.Color, evt.color);
        }
    }
}