using System;
using Ami.BroAudio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Work.HN.Code.Save;

namespace Work.HN.Code.Test
{
    public class TestMapUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mapNameText, registeredText;
        [SerializeField] private GameObject editBtn;
        [SerializeField] private SoundID clickSoundID;

        private string _mapName;

        public void Initialize(MapData mapData)
        {
            _mapName = mapData.mapName;
            
            mapNameText.text = _mapName;
            registeredText.text = mapData.isRegistered ? "맵이 등록됨" : "";
            
            editBtn.SetActive(!mapData.isRegistered);
        }
        
        public void OnEditBtnClick()
        {
            BroAudio.Play(clickSoundID);
            
            DataReceiver.Instance.SetMapEditData(_mapName);
            SceneManager.LoadScene("HN");
        }

        public void OnPlayBtnClick()
        {
            DataReceiver.Instance.SetPlayMapData(_mapName);
            SceneManager.LoadScene("JW");
        }
    }
}