using Unity.Cinemachine;
using UnityEngine;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.HN.Code.Save;
using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ShakeTrigger : Trigger
    {
        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField] private float shakePower = 1f;
        [SerializeField] private Vector2 shakeVelocity;
        
        private CinemachineImpulseSource _impulseSource;

        public override void TriggerEvent(Entity entity)
        {
            _impulseSource.DefaultVelocity = new Vector3(-1, -1, 0);
            _impulseSource.GenerateImpulse();
            
            base.TriggerEvent(entity);
        }

        public override void SetData(TriggerData data)
        {
            ShakeInfo info = data.shakeInfo;
            
            _impulseSource = GetComponent<CinemachineImpulseSource>();
            
            shakeDuration = info.duration;
            shakePower = info.strength;
            
            _impulseSource.ImpulseDefinition.AmplitudeGain = shakePower;
            _impulseSource.ImpulseDefinition.TimeEnvelope.SustainTime = shakeDuration;
        }
    }
}