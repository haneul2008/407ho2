using System;
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

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Active(bool isActive)
        {
            print("in");
            if (isActive)
            {
                if (objectInvoker.SaveData())
                {
                    descText.text = "저장 성공!";
                }
                else
                {
                    descText.text = "저장 실패";
                }
                
                checkBtn.SetActive(true);
            }
            
            gameObject.SetActive(isActive);
        }
    }
}