using System;
using Ami.BroAudio;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Save;

namespace Work.HN.Code.MapMaker.UI
{
    public class RegisterCheckPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI failText;
        [SerializeField] private ObjectInvoker objectInvoker;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        
        [SerializeField] private SoundID clickSoundID;

        private string _originText;
        private bool _isRegistered;
        
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            gameObject.SetActive(false);
            
            _originText = failText.text;
            
            mapMakerChannel.AddListener<SaveFailEvent>(HandleSaveFail);
        }

        private void OnDestroy()
        {
            mapMakerChannel.RemoveListener<SaveFailEvent>(HandleSaveFail);
        }

        private void HandleSaveFail(SaveFailEvent evt)
        {
            _isRegistered = false;
            
            print(evt.errorType);
            
            if (evt.errorType == ErrorType.ExceededMaxCapacity)
            {
                failText.text = "맵 크기가 최대 크기보다 큽니다.";
            }
            else
            {
                failText.text = "내보내기 실패";
            }
                
            DOVirtual.DelayedCall(1f, () => failText.text = _originText);
        }

        public void Active(bool isActive)
        {
            BroAudio.Play(clickSoundID);
            
            gameObject.SetActive(isActive);
        }

        public void OnClick()
        {
            BroAudio.Play(clickSoundID);
            
            if(_isRegistered) return;

            if (!objectInvoker.SaveData(true, type => print(type)))
            {
                TweenFailText("내보내기 실패");
                
                return;
            }

            if (!saveManager.IsVerified)
            {
                TweenFailText("맵을 클리어해주세요.");

                return;
            }

            objectInvoker.RegisterData();
            _isRegistered = true;
        }

        private void TweenFailText(string desc)
        {
            failText.text = desc;

            DOVirtual.DelayedCall(1f, () => failText.text = _originText);
        }
    }
}