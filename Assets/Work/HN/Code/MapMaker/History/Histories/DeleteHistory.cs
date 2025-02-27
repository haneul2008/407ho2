using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public class DeleteHistory : SpawnOrDestroyHistory
    {
        public DeleteHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObjectPrefab, EditorObject targetObject, Vector2 spawnPos) : base(mapMakerChannel, targetObjectPrefab, targetObject, spawnPos)
        {
        }

        public override void Undo()
        {
            SpawnObj();
        }

        public override void Redo()
        {
            DeleteObj();
        }
    }
}