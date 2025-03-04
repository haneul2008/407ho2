using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Save;

public static class TitleEvent
{
    public static readonly DeleteRequestEvent DeleteRequestEvent = new DeleteRequestEvent();
    public static readonly MapDeleteEvent MapDeleteEvent = new MapDeleteEvent();
}

public class DeleteRequestEvent : GameEvent
{
    public string mapName;
}

public class MapDeleteEvent : GameEvent
{
    public MapData mapData;
}
