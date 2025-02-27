using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public class SpawnHistory : SpawnOrDestroyHistory
    {
        public SpawnHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObjectPrefab, EditorObject targetObject, Vector2 spawnPos) : base(mapMakerChannel, targetObjectPrefab, targetObject, spawnPos)
        {
        }

        public override void Undo()
        {
            DeleteObj();
        }

        public override void Redo()
        {
            SpawnObj();
        }
    }
}