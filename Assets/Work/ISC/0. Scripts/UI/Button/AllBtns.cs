using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Work.ISC._0._Scripts.UI.Button
{
    public class AllBtns : MonoBehaviour
    {
        [SerializeField] private Image subPanel;

        public void Play()
        {
            Vector3 dir = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
            float yPos = dir.y;
            subPanel.transform.DOMoveY(yPos, 0.5f);
        }

        public void Exit()
        {
            Vector3 dir = Camera.main.WorldToScreenPoint(new Vector3(0, -17, 0));
            float yPos = dir.y;
            subPanel.transform.DOMoveY(yPos, 0.5f);
        }
    }
}