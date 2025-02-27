using System;
using DG.Tweening;
using Unity.Burst.Intrinsics;
using Unity.Cinemachine;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    public class CameraMoveTrigger : Trigger
    {
        [SerializeField] private CinemachineCamera targetCam;
        [SerializeField] private Vector2 movePos;
        [SerializeField] private float duration;
        [SerializeField] private Transform targetTrm;
        

        public override void TriggerEvent(Entity entity)
        {
            targetCam.Follow = null;
            targetCam.transform.DOMove(movePos, duration).SetEase(Ease.InCubic);
        }
        
        public void SetData(Vector2 pos, float time)
        {
            targetCam = FindAnyObjectByType<CinemachineCamera>();
            targetTrm = targetCam.Follow;
            
            movePos = pos;
            duration = time;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                targetCam.Follow = targetTrm;
            }
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (targetCam == null) return;
            
            Gizmos.color = Color.yellow;

            float height = targetCam.Lens.OrthographicSize * 2; //세로
            float width = height * targetCam.Lens.Aspect; //가로
            
            var size = new Vector2(width, height);
            Gizmos.DrawWireCube(movePos, size);
        }
#endif
    }
}