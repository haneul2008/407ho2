using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class ScaleEditor : EditableMono
    {
        public override EditType EditType => EditType.Scale;

        public override void Initialize()
        {
            base.Initialize();
            
            mapMakerChannel.AddListener<ScaleChangeEvent>(HandleScaleChange);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<ScaleChangeEvent>(HandleScaleChange);
        }

        private void HandleScaleChange(ScaleChangeEvent evt)
        {
            EditorObject targetObject = evt.targetObject;
            
            if (targetObject == null) return;

            float scale = evt.scale;
            Vector3 newScale = new Vector3(scale, scale, 1);
            targetObject.InfoManager.ChangeInfo(InfoType.Size, newScale);
        }
    }
}