using UnityEngine;

namespace Work.HN.Code.MapMaker.UI.Angle
{
    public class AngleEditBtn : MonoBehaviour
    {
        [SerializeField] private float value;

        public void OnClick(AngleEditPanel angleEditPanel)
        {
            angleEditPanel.AddValue(value);
        }
    }
}