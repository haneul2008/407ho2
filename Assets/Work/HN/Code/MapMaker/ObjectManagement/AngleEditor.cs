using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class AngleEditor : EditableMono
    {
        public override EditType EditType => EditType.Rotation;

        public override void Initialize()
        {
            base.Initialize();
            
            mapMakerChannel.AddListener<AngleChangeEvent>(HandleAngleChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<AngleChangeEvent>(HandleAngleChanged);
        }
        
        private void HandleAngleChanged(AngleChangeEvent evt)
        {
            EditorObject targetObject = evt.targetObject;
            
            if(targetObject == null) return;
            
            float angle = evt.angle;
            
            targetObject.InfoManager.ChangeInfo(InfoType.Angle, angle);
        }
    }
}