using System;
using DG.Tweening;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.ObjectManagement;

namespace Work.HN.Code.MapMaker.UI
{
    public class ObjectEditPanel<T> : MonoBehaviour where T : EditableMono
    {
        [SerializeField] protected GameEventChannelSO mapMakerChannel;
        [SerializeField] protected MapMakerManager mapMaker;
        [SerializeField] protected float yPosInDeactivate = -500f;
        [SerializeField] protected float moveDelay = 0.3f;

        protected bool _isActive;
        private float _yPosInActive;

        protected virtual void Awake()
        {
            mapMakerChannel.AddListener<EditModeChangeEvent>(HandleEditModeChange);
            
            RectTransform rectTrm = transform as RectTransform;
            _yPosInActive = rectTrm.anchoredPosition.y;
            
            rectTrm.anchoredPosition = new Vector2(rectTrm.anchoredPosition.x, yPosInDeactivate);
        }

        protected virtual void OnDestroy()
        {
            mapMakerChannel.RemoveListener<EditModeChangeEvent>(HandleEditModeChange);
        }

        private void HandleEditModeChange(EditModeChangeEvent evt)
        {
            EditableMono targetEditor = mapMaker.GetEditor(evt.editType);
            RectTransform rectTrm = transform as RectTransform;
            bool isSpecifiedEditor = targetEditor.GetType() == typeof(T);
            
            if (!_isActive && isSpecifiedEditor && !Mathf.Approximately(rectTrm.anchoredPosition.y, _yPosInActive))
            {
                rectTrm.DOAnchorPosY(_yPosInActive, moveDelay);
                _isActive = true;
            }
            else if (_isActive && !isSpecifiedEditor && !Mathf.Approximately(rectTrm.anchoredPosition.y, yPosInDeactivate))
            {
                rectTrm.DOAnchorPosY(yPosInDeactivate, moveDelay);
                _isActive = false;
            }
        }
    }
}