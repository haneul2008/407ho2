using System.Threading.Tasks;
using Ami.BroAudio;
using Code.Network;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Save.ExelData;

namespace Work.ISC._0._Scripts.UI.Data
{
    
    public class DataPanel : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI mapNameText;
        [SerializeField] private SoundID clickSoundID;

        public string _mapName;
        
        public void DataSetup(string name)
        {
            _mapName = name;
            mapNameText.text = name;
        }

        public async void Click()
        {
            BroAudio.Play(clickSoundID);
            
            DataReceiver.Instance.SetPlayUserMapData(_mapName);
            
            if (AuthenticationService.Instance.IsSignedIn)
            {
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