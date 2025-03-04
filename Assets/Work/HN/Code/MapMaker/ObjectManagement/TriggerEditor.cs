using System;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Input;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.UI;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class TriggerEditor : EditableMono
    {
        public override EditType EditType => EditType.Trigger;

        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private MapMakerCanvas mapMakerCanvas;
        [SerializeField] private MapMakerManager mapMaker;

        private EditorTrigger _selectedTrigger;
        private EditorTrigger _currentTrigger;

        public override void Initialize()
        {
            base.Initialize();
            
            mapMakerChannel.AddListener<CurrentObjectChangeEvent>(HandleCurrentObjectChanged);
            mapMakerChannel.AddListener<TriggerInfoChangeEvent>(HandleTriggerInfoChanged);
            mapMakerChannel.AddListener<TriggerSelectEvent>(HandleTriggerSelectEvent);
            inputReader.OnClickEvent += HandleClickEvent;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<CurrentObjectChangeEvent>(HandleCurrentObjectChanged);
            mapMakerChannel.RemoveListener<TriggerInfoChangeEvent>(HandleTriggerInfoChanged);
            mapMakerChannel.RemoveListener<TriggerSelectEvent>(HandleTriggerSelectEvent);
            inputReader.OnClickEvent -= HandleClickEvent;
        }

        private void HandleTriggerInfoChanged(TriggerInfoChangeEvent evt)
        {
            EditorTrigger targetTrigger = evt.targetTrigger;
            
            if(targetTrigger == null) return;

            print($"Trigger Info Changed info id = {evt.info.ID}");
            
            targetTrigger.SetData(evt.info);
        }

        private void HandleCurrentObjectChanged(CurrentObjectChangeEvent evt)
        {
            EditorObject targetObj = evt.currentObject;
            
            bool isTargetNull = targetObj == null;

            if (isTargetNull || targetObj is not EditorTrigger trigger) return;
            
            _currentTrigger = trigger;
        }

        private void HandleTriggerSelectEvent(TriggerSelectEvent evt)
        {
            if(!_isActive) return;
            
            _selectedTrigger = evt.selectedTrigger;
        }

        private void HandleClickEvent()
        {
            if(!_isActive || _selectedTrigger == null || mapMakerCanvas.IsPointerOverUI(inputReader.MouseScreenPos)) return;
            
            Vector2 mousePos = inputReader.MouseWorldPos;
            Vector2 targetPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));
            
            EditorTrigger spawnedObj = Instantiate(_selectedTrigger, targetPos, Quaternion.identity);
            
            RaiseSpawnEvent(spawnedObj);
            RaiseHistoryEvent(spawnedObj, targetPos);
        }

        private void RaiseSpawnEvent(EditorTrigger spawnedObj)
        {
            ObjectSpawnEvent spawnEvent = MapMakerEvent.ObjectSpawnEvent;
            spawnEvent.spawnedObject = spawnedObj;
            mapMakerChannel.RaiseEvent(spawnEvent);
        }

        private void RaiseHistoryEvent(EditorTrigger spawnedObj, Vector2 targetPos)
        {
            ObjectSpawner spawner = mapMaker.GetEditor(EditType.Spawn) as ObjectSpawner;
            
            RegisterHistoryEvent historyEvent = MapMakerEvent.RegisterHistoryEvent;
            historyEvent.history = new SpawnHistory(mapMakerChannel, spawner.TargetObject, spawnedObj, targetPos);
            mapMakerChannel.RaiseEvent(historyEvent);
        }
    }
}