using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Work.HN.Code.Save;
using Newtonsoft.Json;
using System.Data;

namespace Work.ISC._0._Scripts.Save.Firebase
{
    public class FirebaseData : MonoBehaviour
    {
        [SerializeField] private string _data;
        [SerializeField] private List<MapData> testMapDatas = new List<MapData>();

        private DatabaseReference _databaseReference;

        private Action<bool> DeleteCallback;

        private Dictionary<string, IDictionary> _mapData;

        public UnityEvent<MapData> OnMapDataLoaded;

        public List<MapData> MapDataList { get; private set; }

        public const int maxCapacity = 200000;

        private void Start()
        {
            MapDataList = new List<MapData>();
            _mapData = new Dictionary<string, IDictionary>();
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void SaveData(string mapName, string data, Action OnComplete = null)
        {
            _databaseReference.Child(mapName).SetRawJsonValueAsync(data).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    OnComplete?.Invoke();
                    Debug.Log("저장");
                }
            });
        }

        private void LoadData(string data, Action<bool> OnIsNull = null, Action<string> OnSuccess = null)
        {
            _data = data;

            _databaseReference.Child(data).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Debug.Log("로드 실패");
                else if (task.IsCanceled)
                    Debug.Log("로드 취소");
                else
                {
                    var dataSnapshot = task.Result;

                    string dataString = "";
                    foreach (var json in dataSnapshot.Children)
                    {
                        dataString += json.Key + " " + json.Value + "\n";
                        Debug.Log(dataString);
                    }

                    bool isNullOrEmpty = String.IsNullOrEmpty(dataString);

                    if (isNullOrEmpty || String.IsNullOrEmpty(data))
                        Debug.Log("로드 실패. 데이터값이 잘못되었거나 항목이 비어있습니다.");
                    else
                    {
                        OnSuccess?.Invoke(dataString);

                    }

                    OnIsNull?.Invoke(isNullOrEmpty);
                }
            });
        }

        public void Load(string data)
        {
            LoadData(data, OnIsNull: null, jsonData => LoadJsonData(jsonData));
        }

        [ContextMenu("All Load")]
        public void LoadAll()
        {
            LoadAllData();
        }

        private void LoadAllData(Action loadComplete = null)
        {
            _databaseReference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    };

                    DataSnapshot snapshot = task.Result;
                    IDictionary dataPairs = (IDictionary)snapshot.Value;

                    string json = string.Empty;
                    foreach (var value in dataPairs.Values)
                    {
                        json = JsonConvert.SerializeObject(value);
                        MapData mapData = JsonConvert.DeserializeObject<MapData>(json, settings);

                        testMapDatas.Add(mapData);
                        json = string.Empty;
                    }

                    /*var dataSnapshot = task.Result;
                    string json = dataSnapshot.ToString();

                    IDictionary data = null;

                    foreach (var item in dataSnapshot.Children)
                    {
                        data = (IDictionary)item.Value;
                        var key = item.Key;

                        _mapData.Add(key, data);
                    }
                    DictionaryForEach(_mapData);*/
                }
            });
        }

        public void DictionaryForEach(Dictionary<string, IDictionary> dic)
        {
            dic.ToList().ForEach(kv => print($"Key : {kv.Key} \nValue : {kv.Value}"));
        }

        private void LoadJsonData(string jsonData)
        {
            var data = JsonUtility.FromJson<MapData>(jsonData);

            OnMapDataLoaded?.Invoke(data);
        }

        public void DeleteData()
        {
            bool isNull = true;

            DeleteCallback += Delete;

            LoadData(_data, isNullOrEmpty => DataChange(out isNull, isNullOrEmpty));
        }

        private void Delete(bool data)
        {
            if (String.IsNullOrEmpty(_data) || data)
            {
                Debug.Log("실패");
            }
            else
            {
                _databaseReference.Child(_data).RemoveValueAsync().ContinueWith(task =>
                {
                    if (task.IsCanceled)
                        Debug.Log("취소");
                    else if (task.IsCompleted)
                        Debug.Log("성공");
                });
            }

            DeleteCallback -= Delete;
        }

        private void DataChange(out bool from, bool to)
        {
            from = to;

            DeleteCallback?.Invoke(from);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            Load("Test6");
        }
    }
}