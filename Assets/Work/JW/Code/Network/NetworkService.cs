﻿using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Network
{
    public class NetworkService : NetworkBehaviour
    {
        public UnityEvent OnErrorFromJoinCode;
        private string _joinCode;
        public static NetworkService Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private async void Start()
        {
            await UnityServices.InitializeAsync();
        }

        public override void OnNetworkSpawn()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                Shutdown();
                
                if (!NetworkManager.Singleton.IsConnectedClient)
                {
                    Debug.LogWarning("연결이 해제된 상태에서는 메시지를 보낼 수 없습니다.");
                }
            }
            
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
            base.OnNetworkSpawn();
        }

        public override void OnDestroy()
        {
            if(NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient)
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            base.OnDestroy();
        }

        public async void StartOnline()
        {
            //이미 로그인 되었는지 확인
            if (AuthenticationService.Instance.IsSignedIn) return;
            
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
            NetworkManager.Singleton.NetworkConfig.TickRate = 60;
        }

        public void Shutdown()
        {
            NetworkManager.Singleton.Shutdown();
            AuthenticationService.Instance.SignOut();
        }

        [ClientRpc]
        private void AllShutdownClientRpc()
        {
            if(!NetworkManager.Singleton.IsHost)
                Shutdown();
        }

        public async Task CreateRelay()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                PlayerPrefs.SetString("CurrentJoinCodeWithHost", joinCode);
                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key,
                    allocation.ConnectionData
                    );

                NetworkManager.Singleton.StartHost();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                throw;
            }
        }

        public async void JoinRelay()
        {
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(_joinCode);
                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                    joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData
                );
            
                NetworkManager.Singleton.StartClient();
            }
            catch (RelayServiceException e)
            {
                Debug.LogWarning(e);
                OnErrorFromJoinCode?.Invoke();
                return;
            }
        }

        public void WriteCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            
            _joinCode = code;
        }
        
        private void OnClientDisconnectCallback(ulong obj)
        {
            if (!NetworkManager.Singleton.IsServer && NetworkManager.Singleton.DisconnectReason != string.Empty)
            {
                Debug.Log($"Approval Declined Reason: {NetworkManager.Singleton.DisconnectReason}");
                OnErrorFromJoinCode?.Invoke();
            }
        }
    }
}