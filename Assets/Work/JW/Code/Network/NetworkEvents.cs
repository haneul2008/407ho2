using Work.HN.Code.EventSystems;

namespace Work.JW.Code.Network
{
    public static class NetworkEvents
    {
        public static LoadingEvent LoadingEvent = new LoadingEvent();
    }
    
    public class LoadingEvent : GameEvent
    {
    }
}