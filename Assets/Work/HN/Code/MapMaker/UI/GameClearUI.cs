using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            SceneManager.LoadScene("TitleHN");
        }
    }
}