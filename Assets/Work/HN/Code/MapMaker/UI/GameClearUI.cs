using System;
using Ami.BroAudio;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Objects;

namespace Work.HN.Code.MapMaker.UI
{
    public class GameClearUI : MonoBehaviour
    {
        public UnityEvent OnClear;
        
        [SerializeField] private TextMeshProUGUI mapNameText;
        [SerializeField] private float yPosInActive = 0, duration = 0.5f;
        [SerializeField] private ArrivalPointObjectSO endPointSO;
        [SerializeField] private SoundID clickSoundID;
        [SerializeField] private SoundID clearSoundID;

        
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
            BroAudio.Play(clearSoundID);
            DataReceiver.Instance.TryVerify();
            
            Active(true);

            OnClear?.Invoke();
        }

        public void Active(bool isActive)
        {
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
            BroAudio.Play(clickSoundID);
            
            Time.timeScale = 1f;
            SceneManager.LoadScene("TitleHN");
        }

        public void SetTimeScale(bool isActive)
        {
            int timescale = isActive ? 1 : 0;
            Time.timeScale = timescale;
        }
    }
}