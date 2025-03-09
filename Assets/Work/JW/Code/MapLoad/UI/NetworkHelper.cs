using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Work.JW.Code.MapLoad.UI
{
    public class NetworkHelper : NetworkBehaviour
    {
        public UnityEvent OnGameStart;
        [SerializeField] private NetworkObject playerPrefab;
        [SerializeField] private TextMeshProUGUI currentClientText;
        [SerializeField] private TextMeshProUGUI joinCodeText;
        [SerializeField] private Button readyBtn;
        [SerializeField] private Canvas networkCanvas;
        [SerializeField] private int maxClientCount;
        private int _currentClientCount = 0;

        private void Awake()
        {
            
            if (NetworkManager.Singleton.IsHost)
            {
                readyBtn.onClick.AddListener(StartOnlineGame);
                NetworkManager.Singleton.OnClientConnectedCallback += HandleAddClient;
                
                HandleAddClient(NetworkManager.Singleton.LocalClientId);
            }
            else
            {
                currentClientText.text = "Waiting for clients...";
            }
        }

        private void Start()
        {
            if(NetworkManager.Singleton.IsHost)
            {
                string curJoinCode = PlayerPrefs.GetString("CurrentJoinCodeWithHost", "00000");
                joinCodeText.text = $"Join : {curJoinCode}";
            }
            else
            {
                joinCodeText.gameObject.SetActive(false);
            }
            
            readyBtn.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (NetworkManager.Singleton.IsHost)
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleAddClient;
        }

        private void HandleAddClient(ulong clientId)
        {
            NetworkObject client = Instantiate(playerPrefab);
            client.SpawnAsPlayerObject(clientId);
            
            _currentClientCount++;

            currentClientText.text = $"{_currentClientCount} / {maxClientCount}";
            
            if (_currentClientCount > 1)
                readyBtn.gameObject.SetActive(true);
        }

        public void StartOnlineGame()
        {
            GameStartClientRpc();
        }

        [ClientRpc]
        private void GameStartClientRpc()
        {
            networkCanvas.gameObject.SetActive(false);
            OnGameStart?.Invoke();
        }
    }
}