using UnityEngine;
using Work.JW.Code.Animators;
using Work.JW.Code.Entities.FSM;

namespace Work.JW.Code.Entities.Player.PlayerFSM
{
    public class PlayerGroundState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;
        
        public PlayerGroundState(Entity entity, AnimParamSO stateParam) : base(entity, stateParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();

            _player.InputReader.OnJumpPressEvent += HandleJumpKeyPress;
        }

        public override void Update()
        {
            base.Update();
            if (_mover.IsGroundDetected() == false && _mover.CanMove)
            {
                _player.ChangeState("FALL");
            }
        }

        private void HandleJumpKeyPress()
        {
            if(_mover.IsGroundDetected())
                _player.ChangeState("JUMP");
        }

        public override void Exit()
        {
            _player.InputReader.OnJumpPressEvent -= HandleJumpKeyPress;
            base.Exit();
        }
    }
}