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
        }

        public EntityState ChangeState(string newStateName)
        {
            return _stateMachine.ChangeState(newStateName);
        }
    }
}