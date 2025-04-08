using System.Threading.Tasks;
using Ami.BroAudio;
using Code.Network;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Save.ExelData;
using Work.JW.Code.Network;

namespace Work.ISC._0._Scripts.UI.Data
{
    
    public class DataPanel : NetworkBehaviour
    {
        [SerializeField] private GameEventChannelSO networkChannel;
        [SerializeField] private TextMeshProUGUI mapNameText;
        [SerializeField] private SoundID clickSoundID;

        private string _key;
        
        public void DataSetup(string key, string mapName)
        {
            _key = key;
            mapNameText.text = mapName;
        }

        public async void Click()
        {
            BroAudio.Play(clickSoundID);
            
            DataReceiver.Instance.SetPlayUserMapData(_key);
            
            if (AuthenticationService.Instance.IsSignedIn)
            {
                networkChannel.RaiseEvent(NetworkEvents.LoadingEvent);
                await CreateGameHost();
                NetworkManager.Singleton.SceneManager.LoadScene("NetworkMap", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("JW");
            }
        }

        async Task CreateGameHost()
        {
            await FindAnyObjectByType<NetworkService>().CreateRelay();
        }
        
    }
}