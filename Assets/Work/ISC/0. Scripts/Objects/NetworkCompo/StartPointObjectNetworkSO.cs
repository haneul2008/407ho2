using System;
using System.Collections.Generic;
using System.Linq;
using Ami.BroAudio;
using Unity.Netcode;
using UnityEngine;
using Work.ISC._0._Scripts.Objects.Frame;
using Work.JW.Code.Entities;
using Work.JW.Code.Entities.Player;

namespace Work.ISC._0._Scripts.Objects
{
    [CreateAssetMenu(fileName = "StartPointObjectNetworkSO", menuName = "SO/Object/StartPointNet", order = 0)]
    public class StartPointObjectNetworkSO : StartPointObjectSO
    {
        private List<NetworkClient> _players = new List<NetworkClient>();
        public override void InitializeObject()
        {
            _players = NetworkManager.Singleton.ConnectedClients.Values.ToList();
            Debug.Log(_players.Count);
        }

        public override void TargetToPosition(Transform target)
        {
            if(NetworkManager.Singleton.IsHost)
                TargetToPositionServerRpc(target.position);
        }

        [ServerRpc]
        private void TargetToPositionServerRpc(Vector3 pos)
        {
            TargetToPositionClientRpc(pos);
        }

        [ClientRpc]
        private void TargetToPositionClientRpc(Vector3 pos)
        {
            foreach (var item in _players)
            {
                item.PlayerObject.transform.position = pos;
            }
        }
    }
}