using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Objects;

namespace Work.HN.Code.MapMaker.UI
{
    public class GameClearUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mapNameText;
        [SerializeField] private float yPosInActive = 0, duration = 0.5f;
        [SerializeField] private ArrivalPointObjectSO endPointSO;
        
        private float yPosInDeactivate;
        private RectTransform _rectTrm;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
            yPosInDeactivate = _rectTrm.anchoredPosition.y;
            endPointSO.OnClearEvent += HandleOnClear;
        }

        private void OnDestroy()
        {
            endPointSO.OnClearEvent -= HandleOnClear;
        }

        private void HandleOnClear()
        {
            Active(true);
        }

        public void Active(bool isActive)
        {
            int timescale = isActive ? 0 : 1;
            
            Time.timeScale = timescale;
            
            if (isActive)
            {
                _rectTrm.DOAnchorPosY(yPosInActive, duration).SetUpdate(true);
            }
            else
            {
                _rectTrm.DOAnchorPosY(yPosInDeactivate, duration).SetUpdate(true);
            }
        }
        
        public void HandleMapLoaded(MapData mapData)
        {
            mapNameText.text = mapData.mapName;
        }

        public void OnClick()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("TitleHN");
        }
    }
}