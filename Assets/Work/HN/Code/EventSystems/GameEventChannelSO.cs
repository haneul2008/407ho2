using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.HN.Code.EventSystems
{
    public abstract class GameEvent
    {
    }

    [CreateAssetMenu(fileName = "EventChannel", menuName = "SO/Event/Channel", order = 0)]
    public class GameEventChannelSO : ScriptableObject
    {
        private readonly Dictionary<Type, Action<GameEvent>> _events = new();
        private readonly Dictionary<Delegate, Action<GameEvent>> _lookUpTable = new();

        public void AddListener<T>(Action<T> handler) where T : GameEvent
        {
            if (_lookUpTable.ContainsKey(handler))
            {
                Debug.Log($"{typeof(T)} is already registered");
                return;
            }

            Action<GameEvent> castHandler = evt => handler.Invoke(evt as T);
            _lookUpTable[handler] = castHandler;

            var evtType = typeof(T);
            if (_events.ContainsKey(evtType))
                _events[evtType] += castHandler;
            else
                _events[evtType] = castHandler;
        }

        public void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
            var evtType = typeof(T);
            if (_lookUpTable.TryGetValue(handler, out var castHandler))
            {
                if (_events.TryGetValue(evtType, out var internalHandler))
                {
                    internalHandler -= castHandler;
                    if (internalHandler == null)
                        _events.Remove(evtType);
                    else
                        _events[evtType] = internalHandler;
                }

                _lookUpTable.Remove(handler);
            }
        }

        public void RaiseEvent(GameEvent evt)
        {
            if (_events.TryGetValue(evt.GetType(), out var castHandler)) castHandler?.Invoke(evt);
        }

        public void Clear()
        {
            _events.Clear();
            _lookUpTable.Clear();
        }
    }
}