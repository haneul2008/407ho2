using System.Collections.Generic;
using UnityEngine;
using Work.JW.Code.Entities;
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

        private ObjectFrameSO _objSO;
        
        
        public SpriteRenderer SpriteCompo { get; protected set; }

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
        
        public void TriggerEvent(Entity entity)
        {
            _objSO.ObjectUse(entity);
        }

        private void AfterInitObj()
        {
            gameObject.name = _objSO.name;
            SpriteCompo.sprite = _objSO.ObjectImage;
        }
    }
}