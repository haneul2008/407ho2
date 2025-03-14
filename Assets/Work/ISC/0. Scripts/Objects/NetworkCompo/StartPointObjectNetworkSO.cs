using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Work.JW.Code.Entities.Player;

namespace Work.ISC._0._Scripts.Objects
{
    [CreateAssetMenu(fileName = "StartPointObjectNetworkSO", menuName = "SO/Object/StartPointNet", order = 0)]
    public class StartPointObjectNetworkSO : StartPointObjectSO
    {
        private NetworkObject _target;

        public override void InitializeObject()
        {
            ulong userId = NetworkManager.Singleton.LocalClientId;

            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(userId, out var client))
            {
                _target = client.PlayerObject;
            }
            else
            {
                Debug.LogError("No client connected to server");
            }
        }

        public override void TargetToPosition(Transform target)
        {
            if (_target.IsOwner)
                _target.transform.position = target.position;
        }
    }
}