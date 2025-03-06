using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Save.ExelData;
using Work.ISC._0._Scripts.Save.Firebase;

namespace Work.ISC._0._Scripts.UI.Data
{
    public class DataPanelsLoader : MonoBehaviour
    {
        [SerializeField] private FirebaseData saveData;
        [SerializeField] private DataPanel dataPanel;

        public static int Id = 1;

        private List<DataPanel> panels;

        private void Awake()
        {
            panels = new List<DataPanel>();
        }

        public void PanelLoad()
        {
            saveData.LoadAllData(HandleDataListLoaded);
        }

        private void HandleDataListLoaded()
        {
            foreach (MapData mapData in saveData.MapDataList)
            {
                DataPanel panel = Instantiate(dataPanel, transform);
                panel.DataSetup(mapData.mapName);
                panels.Add(panel);
            }
        }

        private void RemoveAllData()
        {
            panels.Clear();

            foreach (Transform trm in transform)
            {
                Destroy(trm.gameObject);
            }
        }
    }
}