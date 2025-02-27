using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Work.HN.Code.Save;

namespace Work.HN.Code.MapMaker.UI
{
    public class RegisterCheckPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI failText;
        [SerializeField] private ObjectInvoker objectInvoker;

        private string _originText;
        private bool _isRegistered;
        
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            gameObject.SetActive(false);
            
            _originText = failText.text;
        }

        public void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void OnClick()
        {
            if(_isRegistered) return;
            
            if (!objectInvoker.SaveData())
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