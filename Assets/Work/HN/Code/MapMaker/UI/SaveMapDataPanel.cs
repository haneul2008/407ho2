using System;
using Ami.BroAudio;
using TMPro;
using UnityEngine;
using Work.HN.Code.Save;

namespace Work.HN.Code.MapMaker.UI
{
    public class SaveMapDataPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private ObjectInvoker objectInvoker;
        [SerializeField] private GameObject checkBtn;
        
        [SerializeField] private SoundID clickSoundID;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Active(bool isActive)
        {
            BroAudio.Play(clickSoundID);
            
            if (isActive)
            {
                if (objectInvoker.SaveData(HandleSaveFail))
                {
                    descText.text = "저장 성공!";
                }
                
                checkBtn.SetActive(true);
            }
            
            gameObject.SetActive(isActive);
        }

        private void HandleSaveFail(ErrorType type)
        {
            switch (type)
            {
                case ErrorType.SameName : descText.text = "같은 이름으로 저장된 맵이 존재합니다.";
                    break;
                
                case ErrorType.EmptyName : descText.text = "맵 이름이 비어있습니다.";
                    break;
                
                case ErrorType.NoneStartOrEnd : descText.text = "시작 지점 또는 도착 지점이 없습니다.";
                    break;
                
                case ErrorType.ExceededMaxCapacity : descText.text = "맵 크기가 최대 크기를 초과하였습니다.";
                    break;
            }
        }
    }
}