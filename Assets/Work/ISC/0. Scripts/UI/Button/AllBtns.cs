using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MaximovInk.UI;
using UnityEngine.Events;

namespace Work.ISC._0._Scripts.UI.Button
{
    public class AllBtns : MonoBehaviour
    {
        public UnityEvent OnPlayEvent;
            
        [SerializeField] private RoundedPanel subPanel;
        [SerializeField] private Image optionPanel;
        
        public void Play()
        {
            Vector3 dir = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
            float yPos = dir.y;
            subPanel.transform.DOMoveY(yPos, 0.5f);

            OnPlayEvent?.Invoke();
        }

        public void Exit()
        {
            Vector3 dir = Camera.main.WorldToScreenPoint(new Vector3(0, -17, 0));
            float yPos = dir.y;
            subPanel.transform.DOMoveY(yPos, 0.5f);
        }

        public void Option()
        {
            Vector3 dir = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
            float xPos = dir.x;
            optionPanel.transform.DOMoveX(xPos, 0.5f);
        }

        public void OptionExit()
        {
            Vector3 dir = Camera.main.WorldToScreenPoint(new Vector3(30, 0, 0));
            float xPos = dir.x;
            optionPanel.transform.DOMoveX(xPos, 0.5f);
        }
    }
}