using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Work.HN.Code.ETC;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Input;

namespace Work.HN.Code.MapMaker.UI.Angle
{
    public class AngleDragUI : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private InputReaderSO inputReader;
        
        private GameEventChannelSO _mapMakerChannel;
        private Vector2 _centerPos;
        private Vector2 _offset;
        private float _radius;
        private float _radiusInUI;

        public void Initialize(GameEventChannelSO mapMakerChannel, float radius)
        {
            _mapMakerChannel = mapMakerChannel;
            _radius = radius;
            
            CalculateRadius();

            _mapMakerChannel.AddListener<CameraMoveEvent>(HandleCameraMove);
            _mapMakerChannel.AddListener<CameraZoomInEvent>(HandleCameraZoomIn);
        }

        private void OnDestroy()
        {
            _mapMakerChannel.RemoveListener<CameraMoveEvent>(HandleCameraMove);
            _mapMakerChannel.RemoveListener<CameraZoomInEvent>(HandleCameraZoomIn);
        }

        private void HandleCameraZoomIn(CameraZoomInEvent evt)
        {
            CalculateRadius();
            float size = 5f / evt.orthographic;
            transform.localScale = size * Vector3.one;
        }

        private void HandleCameraMove(CameraMoveEvent evt)
        {
            float oneBlockInUI = _radiusInUI / _radius;
            transform.position -= evt.moveAmount * oneBlockInUI;
        }

        private void CalculateRadius()
        {
            Vector2 screenRadius = Camera.main.WorldToScreenPoint(new Vector3(0, _radius, 0));
            Vector2 screenZeroPos = Camera.main.WorldToScreenPoint(Vector2.zero);
            _radiusInUI = Vector2.Distance(screenZeroPos, screenRadius);
        }
        
        public void SetAngle(Vector2 targetPos, float angle)
        {
            Vector2 worldPos = CalculatePosition(targetPos, angle, _radius);
            
            transform.position = Camera.main.WorldToScreenPoint(worldPos);
            
            _centerPos = Camera.main.WorldToScreenPoint(targetPos);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _offset = inputReader.MouseScreenPos - (Vector2)transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 dir = inputReader.MouseScreenPos + _offset - _centerPos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.position = CalculatePosition(_centerPos, angle, _radiusInUI);

            RaiseDragEvent(angle, false);
        }

        public void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private Vector2 CalculatePosition(Vector2 targetPos, float angle, float radius)
        {
            float rad = angle * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

            return targetPos + pos;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Vector2 dir = (Vector2)transform.position - _centerPos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            RaiseDragEvent(angle, true);
        }

        private void RaiseDragEvent(float angle, bool isPointerUp)
        {
            AngleDragEvent dragEvt = MapMakerEvent.AngleDragEvent;
            dragEvt.angle = angle;
            dragEvt.isPointerUp = isPointerUp;
            _mapMakerChannel.RaiseEvent(dragEvt);
        }
    }
}