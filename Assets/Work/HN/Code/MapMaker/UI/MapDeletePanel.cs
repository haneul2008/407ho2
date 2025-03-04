using System;
using System.IO;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Save;

namespace Work.HN.Code.MapMaker.UI
{
    public class MapDeletePanel : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO titleChannel;

        private string _targetMapName;

        private void Awake()
        {
            titleChannel.AddListener<DeleteRequestEvent>(HandleDeleteRequest);
            Active(false);
        }

        private void OnDestroy()
        {
            titleChannel.RemoveListener<DeleteRequestEvent>(HandleDeleteRequest);
        }

        private void HandleDeleteRequest(DeleteRequestEvent evt)
        {
            Active(true);
            _targetMapName = evt.mapName;
        }

        public void DeleteMap()
        {
            UserBuiltInData userData = DataReceiver.Instance.GetUserMapData();
            MapData mapData = GetMapData(userData, _targetMapName);

            if (mapData == null) return;

            userData.userMapList.Remove(mapData);

            SaveUserData(userData);

            MapDeleteEvent deleteEvt = TitleEvent.MapDeleteEvent;
            deleteEvt.mapData = mapData;
            titleChannel.RaiseEvent(deleteEvt);
        }

        private void SaveUserData(UserBuiltInData userData)
        {
            string path = DataReceiver.Instance.Path;
            string json = JsonUtility.ToJson(userData);

            File.WriteAllText(path, json);
        }

        private MapData GetMapData(UserBuiltInData userData, string targetMapName)
        {
            foreach(MapData mapData in userData.userMapList)
            {
                if(mapData.mapName == targetMapName)
                {
                    return mapData;
                }
            }

            return null;
        }

        public void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}