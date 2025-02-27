using UnityEngine;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Save.ExelData;

namespace Work.ISC._0._Scripts.UI.Data
{
    public class DataPanelsLoader : MonoBehaviour
    {
        [SerializeField] private SaveData saveData;
        [SerializeField] private DataPanel dataPanel;
        
        
        public void PanelLoad()
        {
            saveData.DataLoad("B1:B1000", SplitData);
        }

        private void SplitData(string obj)
        {
            string[] datas = obj.Split("\n");

            foreach (string data in datas)
            {
                DataPanel panel = Instantiate(dataPanel, transform);
                panel.DataSetup(data, ConvertName(data));
            }
        }

        private string ConvertName(string data)
        {
            MapData mapData = JsonUtility.FromJson<MapData>(data);

            return mapData.mapName;
        }
    }
}