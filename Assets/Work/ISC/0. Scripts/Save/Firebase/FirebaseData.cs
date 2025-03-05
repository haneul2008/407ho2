using System;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;
using Work.HN.Code.Save;

namespace Work.ISC._0._Scripts.Save.Firebase
{
    public class FirebaseData : MonoBehaviour
    {
        [SerializeField] private string _data;
        
        private DatabaseReference _databaseReference;

        private Action<bool> DeleteCallback;
        
        public UnityEvent<MapData> OnMapDataLoaded;

        public const int maxCapacity = 100000;

        private void Start()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        
        public void SaveData(string data)
        {
            string jsonData = JsonUtility.ToJson(data);
            _databaseReference.Child(_data).SetRawJsonValueAsync(jsonData);
            
            Debug.Log("저장");
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
                    var dataSnapshot  = task.Result;

                    string dataString = "";
                    foreach (var json in dataSnapshot.Children)
                    {
                        dataString += json.Key + " " + json.Value + "\n";
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
    }
}