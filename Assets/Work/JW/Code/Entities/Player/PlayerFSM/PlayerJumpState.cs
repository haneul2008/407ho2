using UnityEngine;
using Work.JW.Code.Animators;
using Work.JW.Code.Entities.FSM;

namespace Work.JW.Code.Entities.Player.PlayerFSM
{
    public class PlayerJumpState : EntityState
    {
        private Player _player;
        private EntityMover _mover;

        private bool _isJumpKeyPressed;
        
        public PlayerJumpState(Entity entity, AnimParamSO stateParam) : base(entity, stateParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            
            _player.InputReader.OnJumpReleaseEvent += HandleJumpReleaseEvent;

            _mover.StopImmediately(true);
            _mover.SetMoveSpeedMultiplier(0.7f);
            _mover.AddJump();
            
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
            
            float xMovement = _player.InputReader.MoveDir.x;
            
            _mover.SetMovementX(xMovement);
            
            if (_mover.IsGroundDetected() && _mover.GetVelocity().y < 0) _player.ChangeState("IDLE");
            
            if (_isJumpKeyPressed && !_mover.IsMaxJumpTimeOver() && _mover.IsMinJumpTimeOutOver())
            {
                _mover.AddForce(new Vector2(0, _mover.GetJumpPower()) * Time.deltaTime);
            }
        }

        public override void Exit()
        {
            _mover.SetMoveSpeedMultiplier(1f);
            
            _player.InputReader.OnJumpReleaseEvent -= HandleJumpReleaseEvent;
            
            base.Exit();
        }
    }
}