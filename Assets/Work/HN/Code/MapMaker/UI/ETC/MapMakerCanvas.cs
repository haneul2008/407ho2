using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Work.HN.Code.MapMaker.UI
{
    public class MapMakerCanvas : MonoBehaviour
    {
        private readonly List<RaycastResult> _results = new List<RaycastResult>();
        
        public bool IsPointerOverUI(Vector2 screenPosition)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };

            EventSystem.current.RaycastAll(eventData, _results);

            return _results.Count > 0;
        }
    }
}