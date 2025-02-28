using Ami.BroAudio;
using UnityEngine;

namespace Work.HN.Code.MapMaker.UI.Angle
{
    public class AngleEditBtn : MonoBehaviour
    {
        [SerializeField] private float value;
        [SerializeField] private SoundID clickSoundID;

        public void OnClick(AngleEditPanel angleEditPanel)
        {
            BroAudio.Play(clickSoundID);
            
            angleEditPanel.AddValue(value);
        }
    }
}