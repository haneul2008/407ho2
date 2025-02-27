using System.Collections.Generic;
using UnityEngine;

namespace Work.HN.Code.MapMaker.History
{
    public class HistoryStack<T>
    {
        public int Count => _list.Count;
        
        private readonly List<T> _list = new List<T>();
        private readonly int _maxCnt;
        
        public HistoryStack(int maxCnt)
        {
            _maxCnt = maxCnt;
        }

        public void Push(T obj)
        {
            _list.Add(obj);

            if (_list.Count >= _maxCnt)
            {
                _list.RemoveAt(0);
            }
        }

        public T Pop()
        {
            T targetObj = _list[^1];
            _list.RemoveAt(_list.Count - 1);
            return targetObj;
        }

        public void ViewStack()
        {
            foreach (T obj in _list)
            {
                Debug.Log(obj);
            }
        }
    }
}