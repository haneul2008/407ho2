using Work.JW.Code.Animators;

namespace Work.JW.Code.Entities.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;
        protected AnimParamSO _param;
        
        protected EntityRenderer _renderer;
        
        public EntityState(Entity entity, AnimParamSO stateParam)
        {
            _entity = entity;
            _param = stateParam;
            _renderer = entity.GetCompo<EntityRenderer>();
        }

        public virtual void Enter()
        {
            _renderer.SetParam(_param, true);
        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }

        public virtual void Exit()
        {
            _renderer.SetParam(_param, false);
        }
    }
}