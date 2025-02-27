using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Input;

namespace Work.HN.Code.MapMaker.History
{
    public class HistoryManager : MonoBehaviour
    {
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private int _maxCnt;

        private HistoryStack<IHistory> _undoStack;
        private HistoryStack<IHistory> _redoStack;
        
        private void Awake()
        {
            _undoStack = new HistoryStack<IHistory>(_maxCnt);
            _redoStack = new HistoryStack<IHistory>(_maxCnt);
            
            inputReader.OnUndoEvent += HandleUndoEvent;
            inputReader.OnRedoEvent += HandleRedoEvent;
            
            mapMakerChannel.AddListener<RegisterHistoryEvent>(HandleRegisterEvent);
            mapMakerChannel.AddListener<UndoOrRedoEvent>(HandleUndoOrRedoEvent);
        }

        private void OnDestroy()
        {
            PopAllHistory();
            
            inputReader.OnUndoEvent -= HandleUndoEvent;
            inputReader.OnRedoEvent -= HandleRedoEvent;
            
            mapMakerChannel.RemoveListener<RegisterHistoryEvent>(HandleRegisterEvent);
            mapMakerChannel.RemoveListener<UndoOrRedoEvent>(HandleUndoOrRedoEvent);
        }

        private void HandleUndoOrRedoEvent(UndoOrRedoEvent evt)
        {
            if (evt.isUndo)
            {
                HandleUndoEvent();
            }
            else if (evt.isRedo)
            {
                HandleRedoEvent();
            }
        }

        private void PopAllHistory()
        {
            for (int i = 0; i < _undoStack.Count; i++)
            {
                IHistory history = _undoStack.Pop();
                history.OnDestroy();
            }
            
            for (int i = 0; i < _redoStack.Count; i++)
            {
                IHistory history = _redoStack.Pop();
                history.OnDestroy();
            }
        }

        private void HandleRegisterEvent(RegisterHistoryEvent evt)
        {
            _undoStack.Push(evt.history);
        }

        private void HandleUndoEvent()
        {
            if(_undoStack.Count == 0) return;
            
            IHistory history = _undoStack.Pop();
            history.Undo();
            _redoStack.Push(history);
        }
        
        private void HandleRedoEvent()
        {
            if(_redoStack.Count == 0) return;
            
            IHistory history = _redoStack.Pop();
            history.Redo();
            _undoStack.Push(history);
        }

        [ContextMenu("View Stack")]
        private void ViewStack()
        {
            print("undo stack");
            _undoStack.ViewStack();
            print("----------");
            
            print("redo stack");
            _redoStack.ViewStack();
            print("----------");
        }
    }
}