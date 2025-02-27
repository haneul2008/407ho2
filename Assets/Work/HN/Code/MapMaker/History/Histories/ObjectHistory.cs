using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public abstract class ObjectHistory : History
    {
        protected EditorObject _targetObject;
        
        protected ObjectHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObject) : base(mapMakerChannel)
        {
            _targetObject = targetObject;
            
            _mapMakerChannel.AddListener<ChangeSubstanceInHistoryEvent>(HandleSubstanceChanged);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            _mapMakerChannel.RemoveListener<ChangeSubstanceInHistoryEvent>(HandleSubstanceChanged);
        }

        protected virtual void HandleSubstanceChanged(ChangeSubstanceInHistoryEvent evt)
        {
            if (evt.beforeObj == _targetObject)
                _targetObject = evt.afterObj;
        }
    }
}