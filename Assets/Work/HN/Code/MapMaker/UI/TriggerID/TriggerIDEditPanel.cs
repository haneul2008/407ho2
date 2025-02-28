using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.ObjectManagement;

namespace Work.HN.Code.MapMaker.UI.TriggerID
{
    public class TriggerIDEditPanel : ValueChangeablePanel<TriggerIDEditor>
    {
        [SerializeField] private TMP_InputField idField;

        protected override void Awake()
        {
            base.Awake();

            idField.text = "None";
            
            mapMakerChannel.AddListener<TriggerIDChangeEvent>(HandleTriggerIDChanged);
            idField.onValueChanged.AddListener(HandleValueChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<TriggerIDChangeEvent>(HandleTriggerIDChanged);
            idField.onValueChanged.RemoveListener(HandleValueChanged);
        }

        private void HandleTriggerIDChanged(TriggerIDChangeEvent evt)
        {
            if (evt.isHistory)
            {
                if(_currentObject != evt.targetObject) return;
                
                if (!IsEqualsValue(evt.id))
                {
                    _isWithoutNotify = true;
                    SetIdField(evt.id);
                }
            }
        }

        private void HandleValueChanged(string value)
        {
            if (_isWithoutNotify)
            {
                _isWithoutNotify = false;
                return;
            }
            
            if (int.TryParse(value, out int id))
            {
                RaiseTriggerIDEvent(id);
            }
            else if (value == "None")
            {
                RaiseTriggerIDEvent(null);
            }
        }

        protected override void HandleCurrentObjectChanged(CurrentObjectChangeEvent evt)
        {
            base.HandleCurrentObjectChanged(evt);

            if(_currentObject == null) return;
            
            int? triggerId = evt.currentObject.GetTriggerID();

            if (!IsEqualsValue(triggerId))
            {
                _isWithoutNotify = true;
                SetIdField(triggerId);
            }
        }

        private void SetIdField(int? triggerId)
        {
            if (triggerId.HasValue)
            {
                idField.text = triggerId.Value.ToString();
            }
            else
            {
                idField.text = "None";
            }
        }

        public void DeleteID()
        {
            if(_currentObject == null) return;
            
            idField.text = "None";
        }

        private void RaiseTriggerIDEvent(int? id)
        {
            TriggerIDChangeEvent evt = MapMakerEvent.TriggerIDChangeEvent;
            evt.targetObject = _currentObject;
            evt.id = id;
            evt.isHistory = false;
            mapMakerChannel.RaiseEvent(evt);
        }

        private bool IsEqualsValue(int? target)
        {
            if (!target.HasValue)
            {
                return idField.text == "None";
            }
            
            return int.TryParse(idField.text, out int id) && id == target;
        }
    }
}