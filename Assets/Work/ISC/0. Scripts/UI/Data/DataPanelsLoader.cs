using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Save.Firebase;

namespace Work.ISC._0._Scripts.UI.Data
{
    public class DataPanelsLoader : MonoBehaviour
    {
        [SerializeField] private FirebaseData saveData;
        [SerializeField] private DataPanel dataPanel;

        private List<DataPanel> _panels;
        private Dictionary<string, MapData> _mapDataPairs = new Dictionary<string, MapData>();

        private void Awake()
        {
            _panels = new List<DataPanel>();
        }

        public void PanelLoad()
        {
            saveData.LoadAllData(HandleDataListLoaded);
        }

        private void HandleDataListLoaded()
        {
            ClearPanels();

            _mapDataPairs.Clear();

            foreach (MapData mapData in saveData.MapDataList)
            {
                SpawnPanel(mapData);
                print(mapData.mapName);
                _mapDataPairs.Add(mapData.mapName, mapData);
            }
        }

        private void SpawnPanel(MapData mapData)
        {
            DataPanel panel = Instantiate(dataPanel, transform);
            panel.DataSetup(mapData.mapName);
            _panels.Add(panel);
        }

        private void RemoveAllData()
        {
            _panels.Clear();

            foreach (Transform trm in transform)
            {
                Destroy(trm.gameObject);
            }
        }

        public void Search(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                ClearPanels();

                foreach(MapData mapData in _mapDataPairs.Values)
                {
                    SpawnPanel(mapData);
                }
            }
            else
            {
                if (_mapDataPairs.TryGetValue(text, out MapData mapData))
                {
                    ClearPanels();
                    SpawnPanel(mapData);
                }
            }
        }

        private void ClearPanels()
        {
            foreach (DataPanel panel in _panels)
            {
                Destroy(panel.gameObject);
            }

            _panels.Clear();
        }
    }
}