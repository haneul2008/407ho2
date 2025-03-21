using System;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Work.ISC._0._Scripts.Save.ExelData;
using Work.ISC._0._Scripts.Save.Firebase;

namespace Work.HN.Code.Save
{
    public class InGameLoaderNetwork : InGameLoader
    {
        private string _userMapName;
        
        private void Awake()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleSetMapDataToClient;
        }

        protected override void Start()
        {
            string userMapName = DataReceiver.Instance.UserMapName;
            string editedMapName = DataReceiver.Instance.PlayEditedMapName;

            if (string.IsNullOrEmpty(editedMapName))
            {
                _userMapName = userMapName;
                if (NetworkManager.Singleton.IsHost)
                {
                    GetUserMapDataClientRpc(_userMapName);
                }
            }
            else if (string.IsNullOrEmpty(userMapName))
            {
                GetEditedMapData(editedMapName);
            }
        }

        public override void OnDestroy()
        {
            if(NetworkManager.Singleton != null)
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleSetMapDataToClient;
            if (IsSpawned)
            {
                NetworkObject.Despawn();
            }
            base.OnDestroy();
        }

        private void HandleSetMapDataToClient(ulong obj)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                GetUserMapDataClientRpc(_userMapName);
            }
        }

        [ClientRpc]
        private void GetUserMapDataClientRpc(string mapName)
        {
            GetUserMapData(mapName);
        }
    }
}