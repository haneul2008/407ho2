using Unity.Cinemachine;
using UnityEngine;
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

        public void SetData(float power, float duration)
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
            
            shakeDuration = duration;
            shakePower = power;
            
            _impulseSource.ImpulseDefinition.AmplitudeGain = power;
            _impulseSource.ImpulseDefinition.TimeEnvelope.SustainTime = duration;
        }
    }
}