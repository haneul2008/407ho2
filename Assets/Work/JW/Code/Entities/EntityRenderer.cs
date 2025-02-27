using UnityEngine;
using Work.JW.Code.Animators;

namespace Work.JW.Code.Entities
{
    public class EntityRenderer : MonoBehaviour, IEntityComponent
    {
        [field: SerializeField] public float FacingDirection { get; set; } = 1;

        private Entity _entity;
        public SpriteRenderer Spriter { get; private set; }
        private Animator _animCompo;

        public void Initialize(Entity entity)
        {
            Spriter = entity.GetComponentInChildren<SpriteRenderer>();
            _animCompo = entity.GetComponentInChildren<Animator>();
            _entity = entity;
        }

        #region Param section

        public void SetParam(AnimParamSO param, bool value) => _animCompo.SetBool(param.hashValue, value);
        public void SetParam(AnimParamSO param, int value) => _animCompo.SetInteger(param.hashValue, value);
        public void SetParam(AnimParamSO param, float value) => _animCompo.SetFloat(param.hashValue, value);
        public void SetParam(AnimParamSO param) => _animCompo.SetTrigger(param.hashValue);

        #endregion
        

        #region Flip section

        public void Flip()
        {
            FacingDirection *= -1;
            _entity.transform.Rotate(0, 180f, 0);
        }

        public void FlipController(float direction)
        {
            var xMove = Mathf.Approximately(direction, 0) ? 0 : Mathf.Sign(direction);
            if (Mathf.Abs(xMove + FacingDirection) < 0.5f) //바라보는 방향과 진행방향이 다르다면 플립
                Flip();
        }

        #endregion
    }
}