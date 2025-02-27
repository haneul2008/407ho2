using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.ObjectManagement;

namespace Work.HN.Code.MapMaker.UI.Position
{
    public class PositionEditPanel : ObjectEditPanel<PositionEditor>
    {
        [SerializeField] private List<PositionEditBtnDataSO> uiDataList = new List<PositionEditBtnDataSO>();
        [SerializeField] private PositionEditBtn btnPrefab;

        private readonly List<PositionEditBtn> btnList = new List<PositionEditBtn>();
        
        protected override void Awake()
        {
            base.Awake();

            foreach (PositionEditBtnDataSO uiData in uiDataList)
            {
                PositionEditBtn uiBtn = Instantiate(btnPrefab, transform);
                uiBtn.Initialize(mapMakerChannel, uiData);
                btnList.Add(uiBtn);
            }
            
            mapMakerChannel.AddListener<CurrentObjectChangeEvent>(HandleCurrentObjectChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<CurrentObjectChangeEvent>(HandleCurrentObjectChanged);
        }

        private void HandleCurrentObjectChanged(CurrentObjectChangeEvent evt)
        {
            foreach (PositionEditBtn uiBtn in btnList)
            {
                uiBtn.SetCurrentObject(evt.currentObject);
            }
        }
    }
}