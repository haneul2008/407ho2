using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.History.Histories
{
    public abstract class SpawnOrDestroyHistory : ObjectHistory
    {
        private readonly EditorObject _targetObjectPrefab;
        private readonly Vector2 _spawnPos;

        protected SpawnOrDestroyHistory(GameEventChannelSO mapMakerChannel, EditorObject targetObjectPrefab, EditorObject targetObject, Vector2 spawnPos) : base(mapMakerChannel, targetObject)
        {
            _targetObjectPrefab = targetObjectPrefab;
            _spawnPos = spawnPos;
        }

        protected void DeleteObj()
        {
            RaiseObjectDeleteEvent(_targetObject);

            GameObject.Destroy(_targetObject.gameObject);
        }

        protected void SpawnObj()
        {
            EditorObject spawnedObject = GameObject.Instantiate(_targetObjectPrefab, _spawnPos, Quaternion.identity);

            RaiseSubstanceChangeEvent(spawnedObject);
            RaiseSpawnEvent(spawnedObject);
            
            _targetObject = spawnedObject;
        }

        private void RaiseSubstanceChangeEvent(EditorObject spawnedObject)
        {
            ChangeSubstanceInHistoryEvent evt = MapMakerEvent.ChangeSubstanceInHistoryEvent;
            evt.beforeObj = _targetObject;
            evt.afterObj = spawnedObject;
            _mapMakerChannel.RaiseEvent(evt);
        }

        private void RaiseSpawnEvent(EditorObject spawnedObject)
        {
            ObjectSpawnEvent objectSpawnEvent = MapMakerEvent.ObjectSpawnEvent;
            objectSpawnEvent.spawnedObject = spawnedObject;
            _mapMakerChannel.RaiseEvent(objectSpawnEvent);
        }

        private void RaiseObjectDeleteEvent(EditorObject targetObject)
        {
            ObjectDeleteEvent evt = MapMakerEvent.ObjectDeleteEvent;
            evt.targetObject = targetObject;
            _mapMakerChannel.RaiseEvent(evt);
        }
    }
}