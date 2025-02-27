using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Objects.Frame;

namespace Work.ISC._0._Scripts.Save.ExelData
{
    public class DatasLoad : MonoBehaviour
    {
        [SerializeField] private ObjectFrame loadObj;
        [SerializeField] private SaveData saveData;

        [ContextMenu("Load Data")]
        public void LodingData()
        {
            saveData.DataLoad("B2", DatasSettings);
        }

        private void DatasSettings(string jsonData)
        { 
           MapData datas =  JsonUtility.FromJson<MapData>(jsonData);
           
           foreach (ObjectData data in datas.objectList)
           {
               ObjectFrame obj = Instantiate(loadObj);

               obj.ID = data.objectId;
               obj.transform.position = data.position;
               obj.transform.localScale = data.scale;
               obj.transform.localRotation = Quaternion.Euler(0, 0, data.angle);
               obj.SpriteCompo.color = data.color;
               obj.SpriteCompo.sortingOrder = data.sortingOrder;
           }
        }
    }
}