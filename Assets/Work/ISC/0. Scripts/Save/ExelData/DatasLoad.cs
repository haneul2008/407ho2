using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Objects.Frame;

namespace Work.ISC._0._Scripts.Save.ExelData
{
    public class DatasLoad : MonoBehaviour
    {
        public UnityEvent<MapData> OnMapDataLoadEvent;
        [SerializeField] private SaveData saveData;

        [ContextMenu("Load Data")]
        public void LodingData()
        {
            saveData.DataLoad("B2", DatasSettings);
        }

        private void DatasSettings(string jsonData)
        { 
           MapData datas =  JsonUtility.FromJson<MapData>(jsonData);
           
           OnMapDataLoadEvent?.Invoke(datas);
        }
    }
}