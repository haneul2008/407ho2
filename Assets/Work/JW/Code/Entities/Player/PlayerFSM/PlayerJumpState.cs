using UnityEngine;
using Work.JW.Code.Animators;
using Work.JW.Code.Entities.FSM;

namespace Work.JW.Code.Entities.Player.PlayerFSM
{
    public class PlayerJumpState : PlayerAirState
    {
        private bool _isJumpKeyPressed;
        
        public PlayerJumpState(Entity entity, AnimParamSO stateParam) : base(entity, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _mover.AddJump();
            
            _player.InputReader.OnJumpReleaseEvent += HandleJumpReleaseEvent;
            
            _isJumpKeyPressed = true;
            _mover.AddMaxJumpTime(Time.time);
            _mover.AddMinJumpTimeOut(Time.time);
        }

        private void HandleJumpReleaseEvent()
        {
            _isJumpKeyPressed = false;
        }

        public override void Update()
        {
            base.Update();
            
            if (_isJumpKeyPressed && !_mover.IsMaxJumpTimeOver() && _mover.IsMinJumpTimeOutOver())
            {
                _mover.AddForce(new Vector2(0, _mover.GetJumpPower()) * Time.deltaTime);
            }
            
            if (_mover.GetVelocity().y < -0.05f) _player.ChangeState("FALL");
        }

        public override void Exit()
        {
            _player.InputReader.OnJumpReleaseEvent -= HandleJumpReleaseEvent;
            
            base.Exit();
        }
    }
}