using System;
using Work.HN.Code.EventSystems;

public class MapNameContainer : IDisposable
{
    public Action OnNameChanged;
    public string MapName { get; private set; }

    private GameEventChannelSO _mapMakerChannel;

    public MapNameContainer(string mapName, GameEventChannelSO mapMakerChannel)
    {
        MapName = mapName;
        _mapMakerChannel = mapMakerChannel;
        _mapMakerChannel.AddListener<MapNameChangeEvent>(HandleMapNameChanged);
    }

    private void HandleMapNameChanged(MapNameChangeEvent evt)
    {
        MapName = evt.mapName;

        OnNameChanged?.Invoke();
    }

    public void Dispose() => _mapMakerChannel.RemoveListener<MapNameChangeEvent>(HandleMapNameChanged);
}
