using System;
using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.Save;
using Work.HN.Code.Test;

namespace Work.ISC._0._Scripts.UI.Data
{
    public class MyDataPanelsLoader : MonoBehaviour
    {
        [SerializeField] private TestMapUI mapUIPrefab;

        private List<TestMapUI> _currentMyMaps;

        private void Awake()
        {
            _currentMyMaps = new List<TestMapUI>(); 
        }

        public void LoadData()
        {
            UserBuiltInData userData = DataReceiver.Instance.GetUserMapData();

            if (userData == null) return;

            if (_currentMyMaps.Count > 0)
            {
                Clear();
            }
            
            foreach (MapData mapData in userData.userMapList)
            {
                TestMapUI mapUI = Instantiate(mapUIPrefab, transform);
                mapUI.Initialize(mapData);
                
                _currentMyMaps.Add(mapUI);
            }
        }

        private void Clear()
        {
            _currentMyMaps.Clear();

            foreach (Transform trm in transform)
            {
                Destroy(trm.gameObject);
            }
        }
    }
}