using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Objects;
using Work.HN.Code.MapMaker.UI;
using Work.HN.Code.MapMaker.UI.Angle;

namespace Work.HN.Code.ETC
{
    public class AngleDragHandler : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private AngleDragUI dragUI;
        [SerializeField] private float radius;
        [SerializeField] private int segments;
        
        private LineRenderer _lineRenderer;
        private EditType _currentMode;
        private bool _isActive;
        private EditorObject _currentObject;

        private void Awake()
        {
            _lineRenderer = GetComponentInChildren<LineRenderer>();
            _lineRenderer.positionCount = segments + 1;
            
            dragUI.Initialize(mapMakerChannel, radius);

            mapMakerChannel.AddListener<EditModeChangeEvent>(HandleEditModeChange);
            mapMakerChannel.AddListener<CurrentObjectChangeEvent>(HandleCurrentObjectChange);
            mapMakerChannel.AddListener<AngleChangeEvent>(HandleAngleChange);

            SetActive(false);
            DrawLine();
        }

        private void OnDestroy()
        {
            mapMakerChannel.RemoveListener<EditModeChangeEvent>(HandleEditModeChange);
            mapMakerChannel.RemoveListener<CurrentObjectChangeEvent>(HandleCurrentObjectChange);
            mapMakerChannel.RemoveListener<AngleChangeEvent>(HandleAngleChange);
        }

        private void HandleAngleChange(AngleChangeEvent evt)
        {
            EditorObject targetObject = evt.targetObject;

            if (targetObject != _currentObject)
            {
                SettingHandler(targetObject);
                _currentObject = targetObject;
            }
            
            dragUI.SetAngle(transform.position, evt.angle);
        }

        private void HandleEditModeChange(EditModeChangeEvent evt)
        {
            _currentMode = evt.editType;

            if (_isActive && evt.editType != EditType.Rotation)
            {
                SetActive(false);
            }
            else if (evt.editType == EditType.Rotation && _currentObject != null)
            {
                SettingHandler(_currentObject);
            }
        }

        private void DrawLine()
        {
            float angleStep = 360f / segments;
            
            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Deg2Rad * i * angleStep;
                Vector2 position = new Vector2(transform.position.x + Mathf.Cos(angle) * radius, transform.position.y + Mathf.Sin(angle) * radius);
                _lineRenderer.SetPosition(i, position);
            }
        }

        private void SetActive(bool isActive)
        {
            _isActive = isActive;
            
            _lineRenderer.enabled = isActive;
            
            dragUI.Active(isActive);
        }

        private void HandleCurrentObjectChange(CurrentObjectChangeEvent evt)
        {
            _currentObject = evt.currentObject;
            
            if (_currentMode == EditType.Rotation && evt.currentObject != null)
            {
                SettingHandler(evt.currentObject);
            }
        }

        private void SettingHandler(EditorObject currentObject)
        {
            transform.position = currentObject.transform.position;
                
            DrawLine();
                
            float angle = currentObject.GetAngle();
            dragUI.SetAngle(transform.position, angle);
            
            SetActive(true);
        }
    }
}