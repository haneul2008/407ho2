using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Work.HN.Code.Save;

namespace Work.ISC._0._Scripts.Save.ExelData
{
    public class SaveData : MonoBehaviour
    {
        private readonly string _savePath = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScRGhhXMlSvI7_GmmOj_sevSXpNKjHuEnwxeEactqgC1iZJTQ/formResponse";

        private readonly string _loadPath = "https://docs.google.com/spreadsheets/d/1kz1M84oW3nPViQVUkclT1XkeLQTZhVLQOq5iO0UjI6I";

        public const int maxCapacity = 31000;
        
        public UnityWebRequest Data { get; private set; }

        public void DataSave(string data, Action<ErrorType> onFailed = null, Action onComplete = null) 
        {
            StartCoroutine(UploadData(data, onFailed, onComplete));
        }
        
        public void DataLoad(string column, Action<string> onComplete = null)
        {
            StartCoroutine(LoadData(column, onComplete));
        }
        
        private IEnumerator UploadData(string data, Action<ErrorType> onFailed = null, Action onComplete = null)
        {
            WWWForm form = new WWWForm();
            
            int dataLength = data.Length;
            
            if (dataLength >= maxCapacity)
            {
                Debug.LogWarning("max capacity exceeded");
                onFailed?.Invoke(ErrorType.ExceededMaxCapacity);
                yield break;
            }
            
            form.AddField("entry.256987994", data);

            UnityWebRequest www = UnityWebRequest.Post(_savePath, form);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Save Success");
                onComplete?.Invoke();
            }
            else
            {
                Debug.LogError("Save Error. Check your savePath or form FieldName");
                Debug.LogError($"Save Error: {www.error}");
                onFailed?.Invoke(ErrorType.FailRequest);
            }
        }

        private IEnumerator LoadData(string column, Action<string> onComplete = null)
        {
            Data = UnityWebRequest.Get(GetAddress(_loadPath, column));
            yield return Data.SendWebRequest();
            
            onComplete?.Invoke(Data.downloadHandler.text);
        }

        private string GetAddress(string loadPath, string column)
        {
            return $"{loadPath}/export?format=tsv&range={column}&gid=1313274376#gid=1313274376";
        }
    }
}