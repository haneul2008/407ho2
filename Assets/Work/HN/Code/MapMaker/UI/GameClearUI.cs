using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Work.HN.Code.Save;

namespace Work.HN.Code.MapMaker.UI
{
    public class GameClearUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mapNameText;
        [SerializeField] private float yPosInActive = 0, duration = 0.5f;

        private float yPosInDeactivate;
        private RectTransform _rectTrm;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
            yPosInDeactivate = _rectTrm.anchoredPosition.y;
        }

        public void Active(bool isActive)
        {
            if (isActive)
            {
                _rectTrm.DOAnchorPosY(yPosInActive, duration);
            }
            else
            {
                _rectTrm.DOAnchorPosY(yPosInDeactivate, duration);
            }
        }
        
        public void HandleMapLoaded(MapData mapData)
        {
            mapNameText.text = mapData.mapName;
        }
    }
}