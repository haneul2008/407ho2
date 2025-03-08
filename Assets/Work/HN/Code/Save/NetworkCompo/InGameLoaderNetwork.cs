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
        protected override void Start()
        {
            string userMapName = DataReceiver.Instance.UserMapName;
            string editedMapName = DataReceiver.Instance.PlayEditedMapName;

            if (string.IsNullOrEmpty(editedMapName))
            {
                if (NetworkManager.Singleton.IsHost)
                {
                    GetUserMapDataClientRpc(userMapName);
                }
            }
            else if (string.IsNullOrEmpty(userMapName))
            {
                GetEditedMapData(editedMapName);
            }
        }

        [ClientRpc]
        private void GetUserMapDataClientRpc(string mapName)
        {
            GetUserMapData(mapName);
        }
    }
}