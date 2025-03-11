using System;
using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Save.Firebase;

namespace Work.HN.Code.Save
{
    public class SaveModerator : DelegateModerator<Action<ErrorType>>
    {
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private MapMakerManager mapMaker;
        [SerializeField] private GameEventChannelSO mapMakerChannel;

        private MapNameContainer _mapNameContainer;
        private Dictionary<Func<bool>, ErrorType> _errorPairs;

        private void Awake()
        {
            _errorPairs = new Dictionary<Func<bool>, ErrorType>()
            {
                { IsEmptyObject(), ErrorType.EmptyObject },
                { IsEmptyName(), ErrorType.EmptyName },
                { IsDuplicatedName(), ErrorType.DuplicatedMyMapName },
                { IsNoneStartOrEnd(), ErrorType.NoneStartOrEnd },
                { IsExceededMaxCapacity(), ErrorType.ExceededMaxCapacity },
            };
        }

        private Func<bool> IsExceededMaxCapacity()
        {
            return () => saveManager.GetMapCapacity(mapMaker.GetAllObjects()) >= FirebaseData.maxCapacity;
        }

        private Func<bool> IsNoneStartOrEnd()
        {
            return () => !mapMaker.HasStart() || !mapMaker.HasEnd();
        }

        private Func<bool> IsDuplicatedName()
        {
            return () => !saveManager.IsDuplicatedName(_mapNameContainer.MapName);
        }

        private Func<bool> IsEmptyName()
        {
            return () => string.IsNullOrEmpty(_mapNameContainer.MapName);
        }

        private Func<bool> IsEmptyObject()
        {
            return () => mapMaker.GetAllObjects().Count == 0;
        }

        private void OnDestroy()
        {
            _mapNameContainer?.Dispose();
        }

        public void HandleMapLoaded(MapData mapData)
        {
            _mapNameContainer = new MapNameContainer(saveManager.GetMapName(), mapMakerChannel);
        }

        public override bool Execute()
        {
            foreach(Func<bool> func in _errorPairs.Keys)
            {
                if (func())
                {
                    ErrorType errorType = _errorPairs[func];
                    return InvokeActionAndReturnFalse(errorType);
                }
            }

            return true;
        }

        private bool InvokeActionAndReturnFalse(ErrorType errorType)
        {
            _action?.Invoke(errorType);
            return false;
        }
    }
}