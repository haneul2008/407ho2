using Work.HN.Code.EventSystems;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public abstract class History : IHistory
    {
        protected readonly GameEventChannelSO _mapMakerChannel;
        
        protected History(GameEventChannelSO mapMakerChannel)
        {
            _mapMakerChannel = mapMakerChannel;
        }
        
        public abstract void Undo();

        public abstract void Redo();

        public virtual void OnDestroy()
        {
        }
    }
}