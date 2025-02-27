using System;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Input;

namespace Work.HN.Code.ETC
{
    public class CameraZoomIn : MonoBehaviour
    {
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private float min, max;

        private Camera _camera;
        
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            
            inputReader.OnZoomInEvent += HandleZoomIn;
        }

        private void OnDestroy()
        {
            inputReader.OnZoomInEvent -= HandleZoomIn;
        }

        private void HandleZoomIn(float y)
        {
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - y, min, max);
            
            CameraZoomInEvent evt = MapMakerEvent.CameraZoomInEvent;
            evt.orthographic = _camera.orthographicSize;
            mapMakerChannel.RaiseEvent(evt);
        }
    }
}