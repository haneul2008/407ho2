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
        private bool isGameStarted;
        
        
        private void Awake()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                readyBtn.onClick.AddListener(StartOnlineGame);
                NetworkManager.Singleton.OnClientConnectedCallback += HandleAddClient;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleRemoveClient;
                NetworkManager.Singleton.ConnectionApprovalCallback += HandleApprovalCheck;
                
                HandleAddClient(NetworkManager.Singleton.LocalClientId);
            }
            else
            {
                currentClientText.text = "Waiting for clients...";
            }
        }

        private void HandleApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.Log("in");
            if (isGameStarted)
            {
                response.Approved = false;
                response.Reason = "게임이 이미 시작되었습니다.";
            }
            else
            {
                response.Approved = true;
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

        private void HandleAddClient(ulong clientId)
        {
            if (!NetworkManager.Singleton.IsHost) return;
            
            NetworkObject client = Instantiate(playerPrefab);
            client.SpawnAsPlayerObject(clientId);
            
            _currentClientCount++;

            currentClientText.text = $"{_currentClientCount} / {maxClientCount}";
            
            if (_currentClientCount > 1)
                readyBtn.gameObject.SetActive(true);
        }
        
        private void HandleRemoveClient(ulong clientId)
        {
#if UNITY_EDITOR
            Debug.Log($"클라이언트 {clientId}의 연결이 해제되었습니다.");
#endif
            CleanupClient(clientId);
        }
        
        private void CleanupClient(ulong clientId)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
            {
                var clientObject = client.PlayerObject;
                if (clientObject != null)
                {
                    Destroy(clientObject.gameObject);
                }
            }
        }

        public void StartOnlineGame()
        {
            GameStartClientRpc();
            isGameStarted = true;
        }

        [ClientRpc]
        private void GameStartClientRpc()
        {
            networkCanvas.gameObject.SetActive(false);
            OnGameStart?.Invoke();
        }

        public override void OnNetworkDespawn()
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleAddClient;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleRemoveClient;
            NetworkManager.Singleton.ConnectionApprovalCallback -= HandleApprovalCheck;
            
            base.OnNetworkDespawn();
        }
    }
}