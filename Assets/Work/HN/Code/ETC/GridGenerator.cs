using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Work.HN.Code.EventSystems;

namespace Work.HN.Code.ETC
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private Camera targetCam;
        [SerializeField] private TileBase gridTile;
        [SerializeField] private float gridCheckMultiplier = 1;
        [SerializeField] private float camSizeX, camSizeY;

        private Transform cameraTrm => targetCam.transform;
        private Tilemap _gridTilemap;
        private float _originOrthographicSize;

        private void Awake()
        {
            _originOrthographicSize = targetCam.orthographicSize;
            _gridTilemap = GetComponent<Tilemap>();
            
            mapMakerChannel.AddListener<CameraMoveEvent>(HandleCameraMoved);
            mapMakerChannel.AddListener<CameraZoomInEvent>(HandleCameraZoomIn);
        }

        private void OnDestroy()
        {
            mapMakerChannel.RemoveListener<CameraMoveEvent>(HandleCameraMoved);
            mapMakerChannel.RemoveListener<CameraZoomInEvent>(HandleCameraZoomIn);
        }

        private void HandleCameraZoomIn(CameraZoomInEvent evt)
        {
            DrawGrid();
        }

        private void HandleCameraMoved(CameraMoveEvent evt)
        {
            DrawGrid();
        }

        private void DrawGrid()
        {
            float orthographic = targetCam.orthographicSize;
            Vector3Int camPos = new Vector3Int(GetCeilValue(cameraTrm.position.x), GetCeilValue(cameraTrm.position.y), 0);
            float normalizedOrthographic = Mathf.Clamp(orthographic / _originOrthographicSize, 1f, float.MaxValue);
            float multiplier = normalizedOrthographic * gridCheckMultiplier;
            
            int camX = GetCeilValue(camSizeX * multiplier);
            int camY = GetCeilValue(camSizeY * multiplier);

            for (int i = -camX; i <= camX; i++)
            {
                for (int j = -camY; j <= camY; j++)
                {
                    if (!_gridTilemap.HasTile(new Vector3Int(camPos.x + i, camPos.y + j, 0)))
                    {
                        _gridTilemap.SetTile(new Vector3Int(camPos.x + i, camPos.y + j, 0), gridTile);
                    }
                }
            }
        }

        private int GetCeilValue(float value)
        {
            return Mathf.CeilToInt(value);
        }
    }
}