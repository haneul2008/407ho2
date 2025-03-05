using Work.JW.Code.Animators;
using Work.JW.Code.Entities.FSM;

namespace Work.JW.Code.Entities.Player.PlayerFSM
{
    public class PlayerFallState : PlayerAirState
    {
        public PlayerFallState(Entity entity, AnimParamSO stateParam) : base(entity, stateParam)
        {
        }

        public override void Update()
        {
            base.Update();
            if (_mover.IsGroundDetected()) _player.ChangeState("IDLE");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            _mover.AddVelocityY(-0.14f);
        }
    }
}