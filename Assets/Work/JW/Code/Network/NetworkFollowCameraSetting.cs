using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using Work.HN.Code.ETC;

namespace Code.Network
{
    public class NetworkFollowCameraSetting : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera followCam;
        [SerializeField] private CameraMover camMover;
        private Transform _target;
        
        public void SetFollowTarget()
        {
            _target = NetworkManager.Singleton.LocalClient.PlayerObject.transform;
            followCam.Follow = _target;
        }

        public void ClearFollowTarget()
        {
            followCam.Follow = null;
        }

        public void SetCamMover(bool enable)
        {
            camMover.enabled = enable;
        }
    }
}