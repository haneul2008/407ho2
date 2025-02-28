using Ami.BroAudio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Save.ExelData;

namespace Work.ISC._0._Scripts.UI.Data
{
    
    public class DataPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mapName;
        [SerializeField] private SoundID clickSoundID;

        private int _id;
        
        public string _jsonData { get; private set; }
        
        public void DataSetup(string jsonData, string name, int id)
        {
            _jsonData = jsonData;
            mapName.text = name;
            _id = id;
        }

        public void Click()
        {
            BroAudio.Play(clickSoundID);
            
            DataReceiver.Instance.SetPlayUserMapData(_id);
            SceneManager.LoadScene("JW");
        }
        
    }
}