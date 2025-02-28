using System;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.Events;
using Work.HN.Code.Input;
using Work.JW.Code.Entities.FSM;
using Work.JW.Code.TriggerSystem;

namespace Work.JW.Code.Entities.Player
{
    public class Player : Entity
    {
        [field: SerializeField] public InputReaderSO InputReader { get; private set; }

        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private StateListSO stateList;

        [field: SerializeField] public SoundID JumpSoundID { get; private set; }
        [field: SerializeField] public SoundID DieSoundID { get; private set; }
        [field: SerializeField] public SoundID GetTriggerSoundID { get; private set; }

        private StateMachine _stateMachine;


        protected override void InitializeCompo()
        {
            base.InitializeCompo();

            _stateMachine = new StateMachine(this, stateList);

            InputReader.SetEnable(InputType.MapMaker, false);
            InputReader.SetEnable(InputType.Player, true);
        }

        private void Start()
        {
            ChangeState("IDLE");
        }

        private void OnDestroy()
        {
            InputReader.ClearPlayerAction();
        }

        private void Update()
        {
            _stateMachine.StateMachineUpdate();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ITriggerEvent trigger))
            {
                BroAudio.Play(GetTriggerSoundID);
                trigger.TriggerEvent(this);
            }

            if (other.gameObject.layer == 8)
            {
                OnHit?.Invoke();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                BroAudio.Play(DieSoundID);
                OnHit?.Invoke();
            }
        }

        public EntityState ChangeState(string newStateName)
        {
            return _stateMachine.ChangeState(newStateName);
        }
    }
}