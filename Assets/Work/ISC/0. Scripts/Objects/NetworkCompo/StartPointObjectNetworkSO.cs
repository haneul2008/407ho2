using System;
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
        public override void InitializeObject()
        {
            Player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>();
        }

        public override void TargetToPosition(Transform target)
        {
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
            Player.transform.position = pos;
        }
    }
}