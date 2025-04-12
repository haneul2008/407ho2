using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Work.HN.Code.Save;

namespace Work.ISC._0._Scripts.Save.Firebase
{
    public class FirebaseData : MonoBehaviour
    {
        public UnityEvent<MapData> OnMapDataLoaded;

        public List<MapData> MapDataList { get; private set; }

        public const int MaxCapacity = 200000;

        private DatabaseReference _databaseReference;
        private Dictionary<string, IDictionary> _mapData;
        private Action<bool> DeleteCallback;
        private Dictionary<MapData, string> _keyPairs = new Dictionary<MapData, string>();

        private JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        private void Start()
        {
            MapDataList = new List<MapData>();
            _mapData = new Dictionary<string, IDictionary>();
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void SaveData(string mapName, string data, Action onComplete = null)
        {
            DatabaseReference newRef = _databaseReference.Child(mapName).Push();
            string key  = newRef.Key;

            StartCoroutine(SaveDataCoroutine(key, data, onComplete));
        }

        private IEnumerator SaveDataCoroutine(string key, string data, Action onComplete = null)
        {
            var task = _databaseReference.Child(key).SetRawJsonValueAsync(data);
            
            yield return new WaitUntil(() => task.IsCompleted);
            
            onComplete?.Invoke();
            Debug.Log("저장");
        }

        public void LoadData(string path, Action<bool> onIsNull = null, Action<MapData> onSuccess = null)
        {
            StartCoroutine(LoadDataCoroutine(path, onIsNull, onSuccess));
        }

        private IEnumerator LoadDataCoroutine(string path, Action<bool> onIsNull = null, Action<MapData> onSuccess = null)
        {
            var task = _databaseReference.Child(path).GetValueAsync();

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.IsFaulted)
                Debug.Log("로드 실패");
            else if (task.IsCanceled)
                Debug.Log("로드 취소");
            else
            {
                DataSnapshot dataSnapshot = task.Result;
                IDictionary dataPairs = (IDictionary)dataSnapshot.Value;

                string registerJson = GetJson(dataPairs["isRegistered"]);
                string verifyJson = GetJson(dataPairs["isVerified"]);
                string mapNameJson = GetJson(dataPairs["mapName"]);
                string objectListJson = GetJson(dataPairs["objectList"]);

                string json = GetFormatJson(registerJson, verifyJson, mapNameJson, objectListJson);

                MapData mapData = JsonConvert.DeserializeObject<MapData>(json, _settings);
                bool isNullOrEmpty = string.IsNullOrEmpty(json);

                if (isNullOrEmpty || string.IsNullOrEmpty(path))
                    Debug.Log("로드 실패. 데이터값이 잘못되었거나 항목이 비어있습니다.");
                else
                {
                    onSuccess?.Invoke(mapData);
                }

                onIsNull?.Invoke(isNullOrEmpty);
            }
        }

        private string GetJson(object value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        private string GetFormatJson(string isRegistered, string isVerified, string mapName, string objectList)
        {
            return @$"{{""isRegistered"":{isRegistered},""isVerified"":{isVerified},""mapName"":{mapName},""objectList"":{objectList}}}";
        }

        [ContextMenu("All Load")]
        public void LoadAll()
        {
            LoadAllData();
        }

        public void LoadAllData(Action loadComplete = null)
        {
            StartCoroutine(LoadAllDataCoroutine(loadComplete));
        }

        private IEnumerator LoadAllDataCoroutine(Action loadComplete)
        {
            var task = _databaseReference.GetValueAsync();

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                IDictionary dataPairs = (IDictionary)snapshot.Value;

                if (dataPairs == null) yield break;

                MapDataList.Clear();

                List<object> keyList = dataPairs.Keys.Cast<object>().ToList();
                int keyCnt = 0;

                foreach (var value in dataPairs.Values)
                {
                    string json = JsonConvert.SerializeObject(value);
                    MapData mapData = JsonConvert.DeserializeObject<MapData>(json, _settings);

                    MapDataList.Add(mapData);
                    
                    string keyString = JsonConvert.SerializeObject(keyList[keyCnt]).Trim('"');
                    _keyPairs.TryAdd(mapData, keyString);
                    keyCnt++;
                }

                loadComplete?.Invoke();
            }
            else if(task.IsFaulted)
                print("is faulted");
            else if(task.IsCanceled)
                print("is canceled");
            else
                print("not completed");
        }

        public string GetKey(MapData mapData) => _keyPairs.GetValueOrDefault(mapData);

        /*public void DeleteData()
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
        }*/

        private void DataChange(out bool from, bool to)
        {
            from = to;

            DeleteCallback?.Invoke(from);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            LoadData("Test6");
        }
    }
}