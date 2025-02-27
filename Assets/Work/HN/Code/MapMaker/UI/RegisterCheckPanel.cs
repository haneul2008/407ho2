using System;
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
        [SerializeField] private GameEventChannelSO mapMakerChannel;

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
            gameObject.SetActive(isActive);
        }

        public void OnClick()
        {
            if(_isRegistered) return;
            
            if (!objectInvoker.SaveData(type => print(type)))
            {
                failText.text = "내보내기 실패";
                
                DOVirtual.DelayedCall(1f, () => failText.text = _originText);
                
                return;
            }
            
            objectInvoker.RegisterData();
            _isRegistered = true;
        }
    }
}