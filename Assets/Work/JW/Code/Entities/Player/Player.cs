using System;
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
        private StateMachine _stateMachine;

        
        protected override void InitializeCompo()
        {
            base.InitializeCompo();

            _stateMachine = new StateMachine(this, stateList);
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
                OnHit?.Invoke();
            }
        }

        public EntityState ChangeState(string newStateName)
        {
            return _stateMachine.ChangeState(newStateName);
        }
    }
}