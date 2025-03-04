using System;
using Ami.BroAudio;
using UnityEngine;

namespace Work.JW.Code.MapLoad.UI
{
    public class NetworkFailPanel : MonoBehaviour
    {
        [SerializeField] private SoundID clickSoundID;
        [SerializeField] private SoundID failSoundID;
        
        private void Awake()
        {
            Active(false);
        }

        public void FailNetwork()
        {
            BroAudio.Play(failSoundID);
        }

        public void ClickAudio()
        {
            BroAudio.Play(clickSoundID);
        }

        public void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}