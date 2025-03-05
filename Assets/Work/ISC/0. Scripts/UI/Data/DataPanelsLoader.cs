using System;
using System.Collections.Generic;
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
            saveData.Load("");
        }

        private void SplitData(string obj)
        {
            if (string.IsNullOrEmpty(obj)) return;
            
            if (panels.Count > 0)
            {
                RemoveAllData();
            }
            Id = 1;
            string[] datas = obj.Split("\n");

            foreach (string data in datas)
            {
                Id++;
                DataPanel panel = Instantiate(dataPanel, transform);
                panel.DataSetup(data, ConvertName(data), Id);
                panels.Add(panel);
                Debug.Log(Id);
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


        private string ConvertName(string data)
        {
            MapData mapData = JsonUtility.FromJson<MapData>(data);
            
            return mapData.mapName;
        }
    }
}