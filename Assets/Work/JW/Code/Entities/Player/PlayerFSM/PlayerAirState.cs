using Work.JW.Code.Animators;
using Work.JW.Code.Entities.FSM;

namespace Work.JW.Code.Entities.Player.PlayerFSM
{
    public class PlayerAirState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;
        
        public PlayerAirState(Entity entity, AnimParamSO stateParam) : base(entity, stateParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            
            _mover.StopImmediately(true);
            _mover.SetMoveSpeedMultiplier(0.7f);
        }

        public override void Update()
        {
            base.Update();
            
            float xMovement = _player.InputReader.MoveDir.x;
            
            _mover.SetMovementX(xMovement);
        }

        public override void Exit()
        {
            _mover.SetMoveSpeedMultiplier(1f);
            base.Exit();
        }
    }
}