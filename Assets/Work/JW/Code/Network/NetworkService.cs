using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Network
{
    public class NetworkService : MonoBehaviour
    {
        public UnityEvent OnErrorFromJoinCode;
        private string _joinCode;

        private async void Start()
        {
            // DontDestroyOnLoad(gameObject);
            
            await UnityServices.InitializeAsync();
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
                Debug.LogError(e);
                OnErrorFromJoinCode?.Invoke();
                return;
            }
        }

        public void WriteCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            
            _joinCode = code;
        }
    }
}