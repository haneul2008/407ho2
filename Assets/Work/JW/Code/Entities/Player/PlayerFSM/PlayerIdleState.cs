using UnityEngine;
using Work.JW.Code.Animators;
using Work.JW.Code.Entities.FSM;

namespace Work.JW.Code.Entities.Player.PlayerFSM
{
    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(Entity entity, AnimParamSO stateParam) : base(entity, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _mover.StopImmediately(false);
        }

        public override void Update()
        {
            float xMovement = _player.InputReader.MoveDir.x;
            float facingDir = _renderer.FacingDirection;

            if (Mathf.Abs(facingDir + xMovement) > 1.5f && _mover.IsWallDetected(facingDir)) return; 

            if (Mathf.Abs(xMovement) > 0)
            {
                _player.ChangeState("MOVE");
            }
        }
    }
}