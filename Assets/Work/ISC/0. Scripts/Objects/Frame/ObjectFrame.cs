using System.Collections.Generic;
using UnityEngine;
using Work.JW.Code.Entities;
using Work.JW.Code.Entities.Player;
using Work.JW.Code.TriggerSystem;

namespace Work.ISC._0._Scripts.Objects.Frame
{
    public class ObjectFrame : MonoBehaviour, ITriggerEvent
    { 
        private int _id;

        public int ID
        {
            get => _id;
            set
            {
                if (_id != value) IDSetting(value);
                _id = value;
            }
        }
        
        private void IDSetting(int id)
        {
            _objSO = ObjectSOList[id - 1];
            
            InitializeObject();
        }
        
        [field : SerializeField] public List<ObjectFrameSO> ObjectSOList { get; private set; }

        [SerializeField] private ObjectFrameSO _objSO;
        
        
        public SpriteRenderer SpriteCompo { get; protected set; }
        public BoxCollider2D ColliderCompo { get; protected set; }

        private void Awake()
        {
            SpriteCompo = GetComponent<SpriteRenderer>();
            ColliderCompo = GetComponent<BoxCollider2D>();
            
            InitializeObject();
        }

        private void InitializeObject()
        {
            if (_objSO == null) return;

            if (_objSO.GetType() == typeof(StartPointObjectSO))
            {
                StartPointObjectSO startPointSO = (StartPointObjectSO)_objSO;
                startPointSO.OnGameStartEvent += SpawnPos;
            }
            
            _objSO.InitializeObject();

            AfterInitObj();
        }

        private void SpawnPos( Entity player)
        {
            player.transform.position = transform.position;
        }

        public void TriggerEvent(Entity entity)
        {
            _objSO.ObjectUse(entity);
        }

        private void AfterInitObj()
        { 
            gameObject.name = _objSO.name;
            gameObject.layer = _objSO.ObjectLayer;
            SpriteCompo.sprite = _objSO.ObjectImage;
            ColliderCompo.offset = _objSO.ColliderOffset;
            ColliderCompo.size = _objSO.ColliderSize;
        }
    }
}