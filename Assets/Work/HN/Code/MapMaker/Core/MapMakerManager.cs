using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Input;
using Work.HN.Code.MapMaker.ObjectManagement;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.UI;
using Work.JW.Code.TriggerSystem;

namespace Work.HN.Code.MapMaker.Core
{
    public struct ObjectInfo
    {
        public Vector2 position;
        public Vector3 size;
        public float angle;
        public Color color;
    }

    public class MapMakerManager : MonoBehaviour
    {   
        [SerializeField] private EditType startEditMode;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private MapMakerCanvas mapMakerCanvas;

        private bool _canSelectEditObject;
        private List<EditableMono> _editorList = new();
        private readonly Dictionary<EditorObject, ObjectInfo> _objectInfoPairs = new();
        private readonly Dictionary<Collider2D, EditorObject> _objectColliderPairs = new();
        private readonly Dictionary<int, List<EditorObject>> _objectIDPairs = new();
        private EditorObject _currentEditObject;
        private EditType _currentEditType;

        private void Awake()
        {
            _editorList = FindObjectsByType<EditableMono>(FindObjectsSortMode.None).ToList();
            _editorList = _editorList.OrderBy(editor => editor.EditType).ToList();
            
            foreach (EditableMono editableMono in _editorList) editableMono.Initialize();

            mapMakerChannel.AddListener<EditModeChangeEvent>(HandleEditModeChange);
            mapMakerChannel.AddListener<ObjectSpawnEvent>(HandleObjectSpawn);
            mapMakerChannel.AddListener<ObjectDeleteEvent>(HandleObjectDeleted);

            inputReader.OnClickEvent += HandleClickEvent;
        }

        private void Start()
        {
            inputReader.SetEnable(InputType.Player, false);
            
            EditModeChangeEvent editModeEvt = MapMakerEvent.EditModeChangeEvent;
            editModeEvt.editType = startEditMode;
            mapMakerChannel.RaiseEvent(editModeEvt);
        }

        private void OnDestroy()
        {
            mapMakerChannel.RemoveListener<EditModeChangeEvent>(HandleEditModeChange);
            mapMakerChannel.RemoveListener<ObjectSpawnEvent>(HandleObjectSpawn);
            mapMakerChannel.RemoveListener<ObjectDeleteEvent>(HandleObjectDeleted);
            inputReader.OnClickEvent -= HandleClickEvent;

            foreach (EditorObject editorObject in _objectInfoPairs.Keys) editorObject.OnInfoChange -= HandleInfoChange;
        }

        private void HandleObjectDeleted(ObjectDeleteEvent evt)
        {
            EditorObject targetObject = evt.targetObject;
            print("deleted");
            _objectInfoPairs.Remove(targetObject);
            _objectColliderPairs.Remove(targetObject.Collider);
            _objectIDPairs.Remove(targetObject.ID);
        }

        private void HandleObjectSpawn(ObjectSpawnEvent evt)
        {
            EditorObject targetObj = evt.spawnedObject;
            
            targetObj.Spawn();

            AddObjectToDictionary(targetObj);

            targetObj.OnInfoChange += HandleInfoChange;

            if(_currentEditType == EditType.Delete) return;
            
            RaiseCurrentObjEvent(targetObj);
        }

        private void AddObjectToDictionary(EditorObject targetObj)
        {
            _objectInfoPairs.Add(targetObj, GetObjectInfo(targetObj));
            _objectColliderPairs.Add(targetObj.Collider, targetObj);

            int id = targetObj.ID;

            if (_objectIDPairs.TryGetValue(id, out List<EditorObject> objectInfoList))
            {
                objectInfoList.Add(targetObj);
            }
            else
            {
                _objectIDPairs.Add(id, new List<EditorObject> { targetObj });
            }
        }

        private void HandleInfoChange(EditorObject obj, InfoType infoType)
        {
            _objectInfoPairs[obj] = GetObjectInfo(obj);
        }

        private ObjectInfo GetObjectInfo(EditorObject targetObj)
        {
            return new ObjectInfo
            {
                position = targetObj.GetPosition(),
                size = targetObj.GetSize(),
                angle = targetObj.GetAngle(),
                color = targetObj.GetColor()
            };
        }

        private void HandleClickEvent()
        {
            if (!_canSelectEditObject || mapMakerCanvas.IsPointerOverUI(inputReader.MouseScreenPos)) return;

            float z = Mathf.Abs(Camera.main.transform.position.z + 1f);

            RaycastHit2D hit = Physics2D.Raycast(inputReader.MouseWorldPos, transform.forward, z);
            
            if(hit.collider == null) return;
            
            EditorObject targetObj = _objectColliderPairs[hit.collider];
            _currentEditObject = targetObj;
            
            RaiseCurrentObjEvent(_currentEditObject);
        }

        private void HandleEditModeChange(EditModeChangeEvent evt)
        {
            _currentEditType = evt.editType;
            _canSelectEditObject = evt.editType != EditType.Spawn && evt.editType != EditType.Trigger;

            if (!_canSelectEditObject)
            {
                _currentEditObject = null;
                RaiseCurrentObjEvent(_currentEditObject);
            }
        }

        public EditableMono GetEditor(EditType editType)
        {
            return _editorList[(int)editType];
        }

        private void RaiseCurrentObjEvent(EditorObject targetObj)
        {
            CurrentObjectChangeEvent currentObjEvt = MapMakerEvent.CurrentObjectChangeEvent;
            currentObjEvt.currentObject = targetObj;
            mapMakerChannel.RaiseEvent(currentObjEvt);
        }

        public List<EditorObject> GetObjects(int id)
        {
            return _objectIDPairs.GetValueOrDefault(id);
        }

        public List<EditorObject> GetAllObjects()
        {
            return _objectInfoPairs.Keys.ToList();
        }
    }
}