using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.ObjectManagement;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.HN.Code.MapMaker.UI.TriggerPanel
{
    public class TriggerEditPanel : ObjectEditPanel<TriggerEditor>
    {
        [SerializeField] private List<TriggerDataSO> triggerDataList = new List<TriggerDataSO>();
        [SerializeField] private ObjectListSO triggerListSO;
        [SerializeField] private TriggerSpawnUI spawnUIPrefab;
        
        private readonly Dictionary<TriggerDataSO, TriggerEditUI> _triggerUIPairs = new Dictionary<TriggerDataSO, TriggerEditUI>();
        private TriggerEditUI _currentEditUI;

        protected override void Awake()
        {
            base.Awake();

            SetListAndInit();
            SpawnUI();

            mapMakerChannel.AddListener<CurrentObjectChangeEvent>(HandleCurrentObjectChanged);
        }

        private void SpawnUI()
        {
            foreach (EditorObject trigger in triggerListSO.objects)
            {
                TriggerSpawnUI spawnUI = Instantiate(spawnUIPrefab, transform);
                spawnUI.Initialize(trigger);
            }
        }

        private void SetListAndInit()
        {
            triggerDataList = triggerDataList.OrderBy(data => data.triggerType).ToList();
            
            List<TriggerEditUI> editUIs = FindObjectsByType<TriggerEditUI>(FindObjectsSortMode.None).ToList();
            editUIs = editUIs.OrderBy(ui => ui.TriggerType).ToList();

            if (triggerDataList.Count != editUIs.Count)
            {
                Debug.LogWarning("triggers don't match");
                return;
            }

            for (int i = 0; i < triggerDataList.Count; i++)
            {
                _triggerUIPairs.Add(triggerDataList[i], editUIs[i]);
            }
            
            editUIs.ForEach(ui => ui.Initialize(mapMakerChannel));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            mapMakerChannel.RemoveListener<CurrentObjectChangeEvent>(HandleCurrentObjectChanged);
        }

        private void HandleCurrentObjectChanged(CurrentObjectChangeEvent evt)
        {
            EditorObject targetObj = evt.currentObject;
            
            bool isTargetNull = targetObj == null;

            if (isTargetNull || targetObj is not EditorTrigger trigger)
            {
                _currentEditUI?.SetActive(false);
                _currentEditUI = null;
                return;
            }
            
            EditorTrigger targetTrigger = trigger;
            TriggerType triggerType = targetTrigger.TriggerType;
            TriggerDataSO targetData = GetData(triggerType);

            if (_triggerUIPairs.TryGetValue(targetData, out TriggerEditUI editUI))
            {
                if (_currentEditUI == editUI)
                {
                    _currentEditUI?.SetTrigger(targetTrigger);
                }
                else
                {
                    _currentEditUI?.SetActive(false);
                
                    _currentEditUI = editUI;
                
                    _currentEditUI?.SetActive(true);
                    _currentEditUI?.SetTrigger(targetTrigger);
                }
            }
        }

        private TriggerDataSO GetData(TriggerType triggerType)
        {
            return triggerDataList.Find(data => data.triggerType == triggerType);
        }
    }
}