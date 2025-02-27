using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Input;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.UI;

namespace Work.HN.Code.MapMaker.ObjectManagement
{
    public class ObjectSpawner : EditableMono
    {
        public override EditType EditType => EditType.Spawn;
        
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private MapMakerCanvas mapMakerCanvas;
        [SerializeField] private MapMakerManager mapMakerManager;
        [SerializeField] private int startPointID = 4, endPointID = 5;

        public EditorObject TargetObject { get; private set; }
        private Vector2 _moveAmount;

        private void HandleClickEvent()
        {
            if(TargetObject == null) return;
            
            Vector2 mousePos = inputReader.MouseWorldPos;
            
            if(mapMakerCanvas.IsPointerOverUI(inputReader.MouseScreenPos)) return;
            
            Vector2 targetPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

            if (ContainsBlock(TargetObject.ID, targetPos)) return;
            if(IsAlreadySpawnedStartOrEnd()) return;
                
            SpawnObject(targetPos);
        }

        private bool IsAlreadySpawnedStartOrEnd()
        {
            bool alreadyHasStart = TargetObject.ID == startPointID && mapMakerManager.HasStart();
            bool alreadyHasEnd = TargetObject.ID == endPointID && mapMakerManager.HasEnd();
            
            return alreadyHasStart || alreadyHasEnd;
        }

        private void SpawnObject(Vector2 targetPos)
        {
            EditorObject spawnedObj = Instantiate(TargetObject, targetPos, Quaternion.identity);

            ObjectSpawnEvent spawnEvent = MapMakerEvent.ObjectSpawnEvent;
            spawnEvent.spawnedObject = spawnedObj;
            mapMakerChannel.RaiseEvent(spawnEvent);

            RegisterHistoryEvent historyEvent = MapMakerEvent.RegisterHistoryEvent;
            historyEvent.history = new SpawnHistory(mapMakerChannel, TargetObject, spawnedObj, targetPos);
            mapMakerChannel.RaiseEvent(historyEvent);
        }

        private void HandleBlockSelected(ObjectSelectEvent evt)
        {
            EditorObject selectedBlock = evt.selectedObject;

            if (selectedBlock == null)
            {
                Debug.LogWarning("target block is null");
                
                return;
            }
            
            TargetObject = selectedBlock;
        }

        protected override void OnActive(bool isActive)
        {
            base.OnActive(isActive);

            if (isActive)
            {
                mapMakerChannel.AddListener<ObjectSelectEvent>(HandleBlockSelected);
                inputReader.OnClickEvent += HandleClickEvent;
            }
            else
            {
                mapMakerChannel.RemoveListener<ObjectSelectEvent>(HandleBlockSelected);
                inputReader.OnClickEvent -= HandleClickEvent;
            }
        }

        private bool ContainsBlock(int blockId, Vector2 targetPos)
        {
            List<EditorObject> objectList = mapMakerManager.GetObjects(blockId);
            
            if(objectList == null) return false;
            
            foreach (EditorObject editorObject in objectList)
            {
                Vector2 position = editorObject.transform.position;
                
                if(EqualsBlock(position, targetPos)) return true;
            }
            
            return false;
        }

        private bool EqualsBlock(Vector2 pos1, Vector2 pos2)
        {
            return Mathf.Approximately(pos1.x, pos2.x) && Mathf.Approximately(pos1.y, pos2.y);
        }
    }
}
