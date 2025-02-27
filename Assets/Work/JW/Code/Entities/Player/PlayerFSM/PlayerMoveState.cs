using UnityEngine;
using Work.JW.Code.Animators;
using Work.JW.Code.Entities.FSM;

namespace Work.JW.Code.Entities.Player.PlayerFSM
{
    public class PlayerMoveState : PlayerGroundState
    {
        public PlayerMoveState(Entity entity, AnimParamSO stateParam) : base(entity, stateParam)
        {
        }

        public override void Update()
        {
            base.Update();
            float xMovement = _player.InputReader.MoveDir.x;
            float facingDir = _renderer.FacingDirection;
            
            _mover.SetMovementX(xMovement);
            
            if (Mathf.Approximately(xMovement, 0) || _mover.IsWallDetected(facingDir))
            {
                _player.ChangeState("IDLE");
            }
        }
    }
}