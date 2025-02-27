using System;
using UnityEngine;
using Work.HN.Code.Save;

namespace Work.HN.Code.Test
{
    public class TestUserMapPanel : MonoBehaviour
    {
        [SerializeField] private TestMapUI mapUIPrefab;
        
        private void Start()
        {
            UserBuiltInData userData = DataReceiver.Instance.GetUserMapData();
            
            if(userData == null) return;
            
            foreach (MapData mapData in userData.userMapList)
            {
                TestMapUI mapUI = Instantiate(mapUIPrefab, transform);
                mapUI.Initialize(mapData);
            }
        }
    }
}