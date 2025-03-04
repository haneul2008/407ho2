using System;
using Ami.BroAudio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Save;

namespace Work.HN.Code.Test
{
    public class TestMapUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mapNameText, registeredText;
        [SerializeField] private GameEventChannelSO titleChannel;
        [SerializeField] private GameObject editBtn, deleteBtn;
        [SerializeField] private SoundID clickSoundID;

        private string _mapName;

        public void Initialize(MapData mapData)
        {
            _mapName = mapData.mapName;
            
            mapNameText.text = _mapName;
            registeredText.text = mapData.isRegistered ? "맵이 등록됨" : "";
            
            editBtn.SetActive(!mapData.isRegistered);
            deleteBtn.SetActive(!mapData.isRegistered);

            titleChannel.AddListener<MapDeleteEvent>(HandleMapDelete);
        }

        private void OnDestroy()
        {
            titleChannel.RemoveListener<MapDeleteEvent>(HandleMapDelete);
        }

        private void HandleMapDelete(MapDeleteEvent evt)
        {
            if (_mapName == evt.mapData.mapName)
                Destroy(gameObject);
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

        public void OnDeleteBtnClick()
        {
            DeleteRequestEvent requestEvt = TitleEvent.DeleteRequestEvent;
            requestEvt.mapName = _mapName;
            titleChannel.RaiseEvent(requestEvt);
        }
    }
}