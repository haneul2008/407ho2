using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.JW.Code.Entities.FSM
{
    public class StateMachine
    {
        public EntityState CurrentState { get; private set; }
        private Dictionary<string, EntityState> _states = new Dictionary<string, EntityState>();

        private Entity _owner;

        public StateMachine(Entity entity, StateListSO stateList)
        {
            _owner = entity;
            foreach (StateSO state in stateList.states)
            {
                Type type = Type.GetType(state.className);
                Debug.Assert(type != null, $"Type '{state.stateName}' does not exist");
               
                EntityState newState = Activator.CreateInstance(type, entity, state.param) as EntityState;
                _states.Add(state.stateName, newState);
            }
        }

        public EntityState ChangeState(string newStateName)
        {
            CurrentState?.Exit();
            
            EntityState newState = _states[newStateName];
            Debug.Assert(newState != null, $"State'{newStateName}' is null");
            
            CurrentState = newState;
            CurrentState?.Enter();

            return newState;
        }
        
        public void StateMachineUpdate()
        {
            CurrentState.Update();
        }

        public void StateMachineFixedUpdate()
        {
            CurrentState.FixedUpdate();
        }
    }
}