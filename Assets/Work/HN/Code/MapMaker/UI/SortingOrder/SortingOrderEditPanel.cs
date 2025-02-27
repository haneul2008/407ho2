using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.ObjectManagement;

namespace Work.HN.Code.MapMaker.UI.SortingOrder
{
    public class SortingOrderEditPanel : ValueChangeablePanel<SortingOrderEditor>
    {
        [SerializeField] private TMP_InputField orderField;

        protected override void Awake()
        {
            base.Awake();
            
            mapMakerChannel.AddListener<SortingOrderChangeEvent>(HandleSortingOrderChanged);
            orderField.onValueChanged.AddListener(HandleValueChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<SortingOrderChangeEvent>(HandleSortingOrderChanged);
            orderField.onValueChanged.RemoveListener(HandleValueChanged);
        }

        private void HandleSortingOrderChanged(SortingOrderChangeEvent evt)
        {
            if (evt.isHistory)
            {
                if(_currentObject != evt.targetObject) return;

                int sortingOrder = evt.sortingOrder;
                
                if (!IsEqualsValue(sortingOrder))
                {
                    _isWithoutNotify = true;
                    orderField.text = sortingOrder.ToString();
                }
            }
        }

        protected override void HandleCurrentObjectChanged(CurrentObjectChangeEvent evt)
        {
            base.HandleCurrentObjectChanged(evt);
            
            if(_currentObject == null) return;
            
            int sortingOrder = _currentObject.GetSortingOrder();

            if (!IsEqualsValue(sortingOrder))
            {
                _isWithoutNotify = true;
                orderField.text = sortingOrder.ToString();
            }
        }

        private void HandleValueChanged(string value)
        {
            if (_isWithoutNotify)
            {
                _isWithoutNotify = false;
                return;
            }

            if (int.TryParse(value, out int sortingOrder))
            {
                RaiseSortingOrderEvent(sortingOrder);
            }
        }

        private void RaiseSortingOrderEvent(int sortingOrder)
        {
            if(_currentObject == null) return;

            SortingOrderChangeEvent evt = MapMakerEvent.SortingOrderChangeEvent;
            evt.targetObject = _currentObject;
            evt.sortingOrder = sortingOrder;
            evt.isHistory = false;
            mapMakerChannel.RaiseEvent(evt);
        }

        private bool IsEqualsValue(int sortingOrder)
        {
            return int.TryParse(orderField.text, out int value) && sortingOrder == value;
        }
    }
}