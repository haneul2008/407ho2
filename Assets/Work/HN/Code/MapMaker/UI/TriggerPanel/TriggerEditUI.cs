using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.History.Histories;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public abstract class TriggerEditUI : MonoBehaviour
    {
        [SerializeField] private float xPosInDeactivate = 400f;
        [SerializeField] private float moveDelay = 0.3f;
        [SerializeField] private TMP_InputField idField;

        public abstract TriggerType TriggerType { get; }
        
        protected bool _isWithoutNotify;
        private RectTransform _rectTrm;
        private bool _isActive;
        private GameEventChannelSO _mapMakerChannel;
        private EditorTrigger _beforeTrigger;
        private EditorTrigger _editorTrigger;
        private float _xPosInActive;
        private bool _idChangeWithoutNotify;

        public virtual void Initialize(GameEventChannelSO mapMakerChannel)
        {
            _rectTrm = transform as RectTransform;
            _xPosInActive = _rectTrm.anchoredPosition.x;
            
            _rectTrm.anchoredPosition = new Vector2(xPosInDeactivate, _rectTrm.anchoredPosition.y);
            
            _mapMakerChannel = mapMakerChannel;
            
            idField.onValueChanged.AddListener(HandleIDChanged);
            _mapMakerChannel.AddListener<TriggerInfoChangeEvent>(HandleTriggerInfoChanged);
        }

        protected virtual void OnDestroy()
        {
            idField.onValueChanged.RemoveListener(HandleIDChanged);
            _mapMakerChannel.RemoveListener<TriggerInfoChangeEvent>(HandleTriggerInfoChanged);
        }

        private void HandleTriggerInfoChanged(TriggerInfoChangeEvent evt)
        {
            if(!evt.isHistory) return;

            EditorTrigger targetTrigger = evt.targetTrigger;
            
            if (TriggerType == targetTrigger.TriggerType)
            {
                SetTrigger(targetTrigger);
            }
        }

        private void HandleIDChanged(string id)
        {
            if (int.TryParse(id, out int num))
            {
                if (_idChangeWithoutNotify)
                {
                    _idChangeWithoutNotify = false;
                    return;
                }

                OnChangeID(num);
            }
            else
            {
                Debug.LogWarning("id is not an integer");
            }
        }

        internal void SetActive(bool isActive)
        {
            _isActive = isActive;

            if (_isActive && !Mathf.Approximately(_rectTrm.anchoredPosition.x, _xPosInActive))
            {
                _rectTrm.DOAnchorPosX(_xPosInActive, moveDelay);
            }
            else if(!_isActive && !Mathf.Approximately(_rectTrm.anchoredPosition.x, xPosInDeactivate))
            {
                _rectTrm.DOAnchorPosX(xPosInDeactivate, moveDelay);
            }
            
            OnActive(isActive);
        }

        protected virtual void OnActive(bool isActive)
        {
        }

        public virtual void SetTrigger(EditorTrigger targetTrigger)
        {
            _beforeTrigger = _editorTrigger;
            _editorTrigger = targetTrigger;

            int newID = targetTrigger.GetInfo<ITriggerInfo>().ID;
            bool isChangedID = int.TryParse(idField.text, out int id) && id != newID;
            
            if (string.IsNullOrEmpty(idField.text) || isChangedID)
            {
                _idChangeWithoutNotify = true;
            }
            
            idField.text = newID.ToString();
        }

        protected void RaiseEvents(ITriggerInfo beforeInfo, Func<ITriggerInfo> afterInfoFunc)
        {
            ITriggerInfo afterInfo = afterInfoFunc();
            
            RaiseInfoEvent(afterInfo);
            RaiseHistoryEvent(beforeInfo, afterInfo);
        }

        private void RaiseHistoryEvent(ITriggerInfo beforeInfo, ITriggerInfo afterInfo)
        {
            RegisterHistoryEvent evt = MapMakerEvent.RegisterHistoryEvent;
            evt.history = new TriggerInfoHistory(_mapMakerChannel, _editorTrigger, beforeInfo, afterInfo);
            _mapMakerChannel.RaiseEvent(evt);
        }

        private void RaiseInfoEvent(ITriggerInfo info)
        {
            TriggerInfoChangeEvent evt = MapMakerEvent.TriggerInfoChangeEvent;
            evt.targetTrigger = _editorTrigger;
            evt.info = info;
            evt.isHistory = false;
            _mapMakerChannel.RaiseEvent(evt);
        }

        protected abstract void OnChangeID(int id);
    }
}