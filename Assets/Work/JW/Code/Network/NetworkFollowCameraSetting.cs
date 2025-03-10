using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace Code.Network
{
    public class NetworkFollowCameraSetting : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera followCam;
        private Transform _target;
        
        public void SetFollowTarget()
        {
            _target = NetworkManager.Singleton.LocalClient.PlayerObject.transform;
            followCam.Follow = _target;
        }
    }
}