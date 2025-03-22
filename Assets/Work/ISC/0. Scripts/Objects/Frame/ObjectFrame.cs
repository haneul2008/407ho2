using System.Collections.Generic;
using UnityEngine;
using Work.JW.Code.Entities;
using Work.JW.Code.TriggerSystem;

namespace Work.ISC._0._Scripts.Objects.Frame
{
    public class ObjectFrame : MonoBehaviour, ITriggerEvent
    {
        protected int _id;

        public int ID
        {
            get => _id;
            set
            {
                if (_id != value) IDSetting(value);
                _id = value;
            }
        }

        protected void IDSetting(int id)
        {
            _objSO = ObjectSOList[id - 1];

            InitializeObject();
        }

        [field: SerializeField] public List<ObjectFrameSO> ObjectSOList { get; private set; }

        [SerializeField] protected ObjectFrameSO _objSO;


        public SpriteRenderer SpriteCompo { get; protected set; }
        public Collider2D ColliderCompo { get; protected set; }

        private void Awake()
        {
            SpriteCompo = GetComponent<SpriteRenderer>();

            InitializeObject();
        }

        private void InitializeObject()
        {
            if (_objSO == null) return;

            _objSO.InitializeObject();

            AfterInitObj();
        }

        protected virtual void Start()
        {
            if (_objSO.GetType() == typeof(StartPointObjectSO))
            {
                StartPointObjectSO startPointSO = (StartPointObjectSO)_objSO;
                startPointSO.TargetToPosition(transform);
            }
            else if (_objSO is ChainsawFramSO chainsawData)
            {
                Animator animator = gameObject.AddComponent<Animator>();
                animator.runtimeAnimatorController = chainsawData.animationController;
            }
        }

        public void TriggerEvent(Entity entity)
        {
            _objSO.ObjectUse(entity);
        }

        private void AfterInitObj()
        {
            ColliderCompo = _objSO.colliderType switch
            {
                ColliderEnum.Box => gameObject.AddComponent<BoxCollider2D>(),
                ColliderEnum.Circle => gameObject.AddComponent<CircleCollider2D>(),
                _ => null
            };

            gameObject.name = _objSO.name;
            gameObject.layer = _objSO.ObjectLayer;
            SpriteCompo.sprite = _objSO.ObjectImage;
            ColliderCompo.offset = _objSO.ColliderOffset;
            ColliderCompo.isTrigger = _objSO.isTrigger;

            if (ColliderCompo is BoxCollider2D boxCollider)
            {
                boxCollider.size = _objSO.ColliderSize;
            }
            else if (ColliderCompo is CircleCollider2D circleCollider)
            {
                circleCollider.radius = _objSO.CircleColliderRadius;
            }
        }
    }
}
