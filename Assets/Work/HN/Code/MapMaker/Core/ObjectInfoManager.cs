using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.HN.Code.MapMaker.Core
{
    public enum InfoType
    {
        Size,
        Position,
        Angle,
        Color,
        TriggerID,
        SortingOrder
    }
    
    public class ObjectInfoManager
    {
        public event Action<InfoType, object> OnInfoChange;
            
        private readonly Dictionary<InfoType, object> _infoPairs = new Dictionary<InfoType, object>();
        private readonly List<InfoType> _notChangeableInfos = new List<InfoType>();
        
        public ObjectInfoManager(Vector3 size, Vector3 position, float angle, Color color, int? triggerId, int sortingOrder, List<InfoType> notChangeableInfos)
        {
            _infoPairs.Add(InfoType.Size, size);
            _infoPairs.Add(InfoType.Position, position);
            _infoPairs.Add(InfoType.Angle, angle);
            _infoPairs.Add(InfoType.Color, color);
            _infoPairs.Add(InfoType.TriggerID, triggerId);
            _infoPairs.Add(InfoType.SortingOrder, sortingOrder);

            foreach (InfoType type in notChangeableInfos)
            {
                _notChangeableInfos.Add(type);
            }
        }

        public bool ChangeInfo(InfoType type, object info)
        {
            if(!IsCanChangeInfo(type, info) || _notChangeableInfos.Contains(type)) return false;
            
            _infoPairs[type] = info;
            OnInfoChange?.Invoke(type, info);
            
            return true;
        }

        private bool IsCanChangeInfo(InfoType type, object info)
        {
            return type switch
            {
                InfoType.Position or InfoType.Size => info is Vector3,
                InfoType.Angle => info is float,
                InfoType.Color => info is Color,
                InfoType.TriggerID => info is int?,
                InfoType.SortingOrder => info is int,
                _ => false
            };
        }

        public object GetInfo(InfoType type)
        {
            return _infoPairs.GetValueOrDefault(type);
        }
    }
}