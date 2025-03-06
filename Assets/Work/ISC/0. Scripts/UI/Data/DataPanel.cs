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
        [SerializeField] private TextMeshProUGUI mapNameText;
        [SerializeField] private SoundID clickSoundID;

        public string _mapName;
        
        public void DataSetup(string name)
        {
            _mapName = name;
            mapNameText.text = name;
        }

        public void Click()
        {
            BroAudio.Play(clickSoundID);
            
            DataReceiver.Instance.SetPlayUserMapData(_mapName);
            SceneManager.LoadScene("JW");
        }
        
    }
}