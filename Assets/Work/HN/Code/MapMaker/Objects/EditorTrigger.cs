using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.HN.Code.MapMaker.UI;

namespace Work.HN.Code.MapMaker.Objects
{   
    public abstract class EditorTrigger : EditorObject
    {
        public TriggerType TriggerType => triggerData.triggerType;

        [SerializeField] private TriggerDataSO triggerData;
        [SerializeField] private TMP_Text triggerIDText, triggerNameText;

        protected MeshRenderer idTextRenderer, nameTextRenderer;
        protected ITriggerInfo _myInfo;
        protected int _targetTriggerId;
        
        public override void Spawn()
        {
            base.Spawn();

            triggerIDText.text = _targetTriggerId.ToString();
            triggerNameText.text = TriggerType.ToString();
            
            idTextRenderer = triggerIDText.GetComponent<MeshRenderer>();
            nameTextRenderer = triggerNameText.GetComponent<MeshRenderer>();
        }

        public virtual void SetData(ITriggerInfo info)
        {
            SetTriggerID(info.ID);
        }
        
        protected virtual void SetTriggerID(int triggerID)
        {
            _targetTriggerId = triggerID;
            triggerIDText.text = _targetTriggerId.ToString();
        }

        public T GetInfo<T>() where T : ITriggerInfo
        {
            if (_myInfo == null)
            {
                Debug.LogError("Info was not set");
            }

            if (_myInfo is T targetInfo)
            {
                return targetInfo;
            }
            else
            {
                Debug.LogWarning("type is invalid");
                return default;
            }
        }

        protected void SetMyInfo<T>(ITriggerInfo info) where T : ITriggerInfo
        {
            if (info is T myInfo)
            {
                _myInfo = myInfo;
            }
        }

        protected override void HandleInfoChange(InfoType type, object info)
        {
            base.HandleInfoChange(type, info);
            
            if(notChangeableInfos.Contains(type)) return;

            if (type == InfoType.SortingOrder)
            {
                idTextRenderer.sortingOrder = (int)info;
                nameTextRenderer.sortingOrder = (int)info;
            }
        }

        [ContextMenu("Set data")]
        public void SetData()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer.color = triggerData.color;
            
            triggerNameText.text = TriggerType.ToString();
        }
    }
}