using System;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Input;

namespace Work.HN.Code.ETC
{
    public class CameraMover : MonoBehaviour
    {   
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private float divideValue;

        private void Update()
        {
            Vector3 moveAmount = inputReader.CameraMoveAmount;
            
            if(Approximately(moveAmount, Vector3.zero)) return;

            Vector3 result = moveAmount / divideValue;
            
            transform.position += result;

            CameraMoveEvent evt = MapMakerEvent.CameraMoveEvent;
            evt.moveAmount = result;
            mapMakerChannel.RaiseEvent(evt);
        }

        private bool Approximately(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
        }
    }
}