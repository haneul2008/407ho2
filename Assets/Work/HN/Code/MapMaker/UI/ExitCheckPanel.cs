using System;
using Ami.BroAudio;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Work.HN.Code.Save;

namespace Work.HN.Code.MapMaker.UI
{
    public class ExitCheckPanel : MonoBehaviour
    {
        [SerializeField] private ObjectInvoker objectInvoker;
        [SerializeField] private TextMeshProUGUI saveAndExitBtnText;
        [SerializeField] private SoundID clickSoundID;
        
        private string _originText;

        private void Awake()
        {
            _originText = saveAndExitBtnText.text;
            
            gameObject.SetActive(false);
        }

        public void SaveAndExit()
        {
            BroAudio.Play(clickSoundID);
            
            if (objectInvoker.SaveData(false))
            {
                SceneManager.LoadScene("TitleHN");
            }
            else
            {
                saveAndExitBtnText.text = "저장 실패";
                
                DOVirtual.DelayedCall(1f, () => saveAndExitBtnText.text = _originText);
            }
        }

        public void Exit()
        {
            BroAudio.Play(clickSoundID);

            SceneManager.LoadScene("TitleHN");
        }

        public void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}