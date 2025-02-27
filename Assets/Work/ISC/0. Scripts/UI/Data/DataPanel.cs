using TMPro;
using UnityEngine;
using Work.ISC._0._Scripts.Save.ExelData;

namespace Work.ISC._0._Scripts.UI.Data
{
    
    public class DataPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mapName;

        private int _id;
        
        public string _jsonData { get; private set; }
        
        public void DataSetup(string jsonData, string name, int id)
        {
            _jsonData = jsonData;
            mapName.text = name;
            _id = id;
        }
        
    }
}