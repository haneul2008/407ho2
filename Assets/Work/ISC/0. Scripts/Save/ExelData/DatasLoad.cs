﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Work.HN.Code.Save;
using Newtonsoft.Json;

namespace Work.ISC._0._Scripts.Save.ExelData
{
    public class DatasLoad : MonoBehaviour
    {
        public UnityEvent<MapData> OnMapDataLoadEvent;
        
        [SerializeField] private SaveData saveData;
        
        [ContextMenu("Load Data")]
        public void LoadingData()
        {
            saveData.DataLoad("B2", DataSettings);
        }

        private void DataSettings(string jsonData)
        { 
           MapData datas = JsonUtility.FromJson<MapData>(jsonData);
           
           OnMapDataLoadEvent?.Invoke(datas);
        }
    }
}