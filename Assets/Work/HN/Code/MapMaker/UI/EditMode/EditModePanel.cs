using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;

namespace Work.HN.Code.MapMaker.UI.EditMode
{
    public class EditModePanel : MonoBehaviour
    {
        [SerializeField] private EditModeBtn btnPrefab;
        [SerializeField] private Vector2 padding;
        [SerializeField] private List<EditBtnDataSO> btnDataList = new List<EditBtnDataSO>();
        
        private readonly List<EditModeBtn> _btnList = new List<EditModeBtn>();
        
        private void Awake()
        {
            btnDataList = btnDataList.OrderBy(data => data.editType).ToList();

            int i = 0;
            
            foreach (EditType type in Enum.GetValues(typeof(EditType)))
            {
                EditModeBtn btn = Instantiate(btnPrefab, transform);
                _btnList.Add(btn);
                btn.Initialize(type, btnDataList[i]);
                i++;
            }
            
            Vector2 btnSize = btnPrefab.GetComponent<RectTransform>().sizeDelta;
            
            RectTransform rectTrm = transform as RectTransform;
            
            int btnCnt = _btnList.Count;
            
            HorizontalLayoutGroup layoutGroup = GetComponent<HorizontalLayoutGroup>();
            float spacing = layoutGroup.spacing;
            
            float width = btnSize.x * btnCnt + spacing * (btnCnt - 1) + padding.x * 2;
            float height = btnSize.y + padding.y * 2;
            
            rectTrm.sizeDelta = new Vector2(width, height);
        }
    }
}