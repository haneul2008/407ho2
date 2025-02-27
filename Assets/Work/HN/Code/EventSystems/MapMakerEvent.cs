using UnityEngine;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.UI;
using Work.JW.Code.TriggerSystem;

namespace Work.HN.Code.EventSystems
{
    public enum EditType
    {
        Spawn,
        Position,
        Scale,
        Rotation,
        Color,
        Trigger,
        Delete,
        TriggerID,
        SortingOrder
    }

    public static class MapMakerEvent
    {
        public static readonly ObjectSelectEvent ObjectSelectEvent = new ObjectSelectEvent();
        public static readonly TriggerSelectEvent TriggerSelectEvent = new TriggerSelectEvent();
        public static readonly EditModeChangeEvent EditModeChangeEvent = new EditModeChangeEvent();
        public static readonly MoveEvent MoveEvent = new MoveEvent();
        public static readonly ObjectSpawnEvent ObjectSpawnEvent = new ObjectSpawnEvent();
        public static readonly CurrentObjectChangeEvent CurrentObjectChangeEvent = new CurrentObjectChangeEvent();
        public static readonly ScaleChangeEvent ScaleChangeEvent = new ScaleChangeEvent();
        public static readonly AngleChangeEvent AngleChangeEvent = new AngleChangeEvent();
        public static readonly AngleDragEvent AngleDragEvent = new AngleDragEvent();
        public static readonly ColorChangeEvent ColorChangeEvent = new ColorChangeEvent();
        public static readonly TriggerInfoChangeEvent TriggerInfoChangeEvent = new TriggerInfoChangeEvent();
        public static readonly BulkDeleteEvent BulkDeleteEvent = new BulkDeleteEvent();
        public static readonly ObjectDeleteEvent ObjectDeleteEvent = new ObjectDeleteEvent();
        public static readonly RegisterHistoryEvent RegisterHistoryEvent = new RegisterHistoryEvent();
        public static readonly ChangeSubstanceInHistoryEvent ChangeSubstanceInHistoryEvent = new ChangeSubstanceInHistoryEvent();
        public static readonly UndoOrRedoEvent UndoOrRedoEvent = new UndoOrRedoEvent();
        public static readonly ObjectSaveEvent ObjectSaveEvent = new ObjectSaveEvent();
        public static readonly CameraMoveEvent CameraMoveEvent = new CameraMoveEvent();
        public static readonly CameraZoomInEvent CameraZoomInEvent = new CameraZoomInEvent();
        public static readonly TriggerIDChangeEvent TriggerIDChangeEvent = new TriggerIDChangeEvent();
        public static readonly SortingOrderChangeEvent SortingOrderChangeEvent = new SortingOrderChangeEvent();
        public static readonly MapNameChangeEvent MapNameChangeEvent = new MapNameChangeEvent();
    }

    public class ObjectSelectEvent : GameEvent //in object spawn mode
    {
        public EditorObject selectedObject;
    }

    public class TriggerSelectEvent : GameEvent //in trigger edit mode
    {
        public EditorTrigger selectedTrigger;
    }

    public class EditModeChangeEvent : GameEvent
    {
        public EditType editType;
    }

    public class MoveEvent : GameEvent
    {
        public EditorObject targetObject;
        public Vector3 moveAmount;
        public bool isHistory;
    }

    public class ObjectSpawnEvent : GameEvent
    {
        public EditorObject spawnedObject;
    }

    public class CurrentObjectChangeEvent : GameEvent
    {
        public EditorObject currentObject;
    }

    public class ScaleChangeEvent : GameEvent
    {
        public EditorObject targetObject;
        public float scale;
        public bool isHistory;
    }

    public class AngleChangeEvent : GameEvent
    {
        public EditorObject targetObject;
        public float angle;
        public bool isHistory;
    }
    
    public class AngleDragEvent : GameEvent
    {
        public float angle;
        public bool isPointerUp;
    }

    public class ColorChangeEvent : GameEvent
    {
        public EditorObject targetObject;
        public Color color;
        public bool isHistory;
    }

    public class TriggerInfoChangeEvent : GameEvent
    {
        public EditorTrigger targetTrigger;
        public ITriggerInfo info;
        public bool isHistory;
    }

    public class BulkDeleteEvent : GameEvent
    {
        public EditorObject targetObject;
    }

    public class ObjectDeleteEvent : GameEvent
    {
        public EditorObject targetObject;
    }

    public class RegisterHistoryEvent : GameEvent
    {
        public IHistory history;
    }

    public class ChangeSubstanceInHistoryEvent : GameEvent
    {
        public EditorObject beforeObj;
        public EditorObject afterObj;
    }

    public class UndoOrRedoEvent : GameEvent
    {
        public bool isUndo;
        public bool isRedo;
    }

    public class ObjectSaveEvent : GameEvent
    {
        public bool isInitialize;
        public bool isFinish;
        public EditorObject targetObject;
    }

    public class CameraMoveEvent : GameEvent
    {
        public Vector3 moveAmount;
    }
    
    public class CameraZoomInEvent : GameEvent
    {
        public float orthographic;
    }

    public class TriggerIDChangeEvent : GameEvent
    {
        public EditorObject targetObject;
        public int? id;
        public bool isHistory;
    }

    public class SortingOrderChangeEvent : GameEvent
    {
        public EditorObject targetObject;
        public int sortingOrder;
        public bool isHistory;
    }

    public class MapNameChangeEvent : GameEvent
    {
        public string mapName;
    }
}
