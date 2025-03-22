using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Network.UI
{
    public class NetworkManagerUI : NetworkBehaviour
    {
        [SerializeField] Button startServerButton;
        [SerializeField] Button hostButton;
        [SerializeField] Button clientButton;

        private void Awake()
        {
            startServerButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
            hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
            clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        }

        public override void OnDestroy()
        {
            startServerButton.onClick.RemoveListener(() => NetworkManager.Singleton.StartServer());
            hostButton.onClick.RemoveListener(() => NetworkManager.Singleton.StartHost());
            clientButton.onClick.RemoveListener(() => NetworkManager.Singleton.StartClient());
            
            base.OnDestroy();
        }
    }
}