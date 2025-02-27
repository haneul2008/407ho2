using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.UI.Position
{
    public class PositionEditBtn : MonoBehaviour
    {
        private GameEventChannelSO _mapMakerChannel;
        private PositionEditBtnDataSO _uiData;
        private TextMeshProUGUI _text;
        private EditorObject _currentObject;
        
        public void Initialize(GameEventChannelSO mapMakerChannel, PositionEditBtnDataSO uiData)
        {
            _mapMakerChannel = mapMakerChannel;
            _uiData = uiData;
            
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _text.text = _uiData.text;
            _text.fontSize *= _uiData.textSizeMultiplier;
        }

        public void SetCurrentObject(EditorObject currentObject)
        {
            _currentObject = currentObject;
        }
        
        public void OnClick()
        {
            if(_currentObject == null) return;
            
            MoveEvent moveEvt = MapMakerEvent.MoveEvent;
            
            moveEvt.targetObject = _currentObject;
            moveEvt.moveAmount = _uiData.GetMoveAmount();
            moveEvt.isHistory = false;
            _mapMakerChannel.RaiseEvent(moveEvt);
        }
    }
}