using System;
using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.MapMaker.Core;

namespace Work.HN.Code.MapMaker.Objects
{
    public class EditorObject : Object
    {
        public event Action<EditorObject, InfoType> OnInfoChange;
        public event Action<EditorObject> OnSpawned;
        
        public Collider2D Collider { get; private set; }
        public ObjectInfoManager InfoManager { get; private set; }
        public SpriteRenderer SpriteRenderer { get; protected set; }
        
        [SerializeField] protected List<InfoType> notChangeableInfos = new List<InfoType>();

        public override void Spawn()
        {
            Collider = GetComponent<Collider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            
            InfoManager = new ObjectInfoManager(transform.localScale, transform.position,
                transform.eulerAngles.z, SpriteRenderer.color, null, SpriteRenderer.sortingOrder, notChangeableInfos);
            
            InfoManager.OnInfoChange += HandleInfoChange;
            
            OnSpawned?.Invoke(this);
        }

        protected virtual void HandleInfoChange(InfoType type, object info)
        {
            if(notChangeableInfos.Contains(type)) return;
            
            switch (type)
            {
                case InfoType.Size:
                    transform.localScale = (Vector3)info;
                    break;
                
                case InfoType.Position:
                    transform.position = (Vector3)info;
                    break;
                
                case InfoType.Angle:
                    transform.eulerAngles = new Vector3(0, 0, (float)info);
                    break;
                
                case InfoType.Color:
                    SpriteRenderer.color = (Color)info;
                    break;
                
                case InfoType.TriggerID:
                    print("trigger id changed");
                    break;
                
                case InfoType.SortingOrder:
                    SpriteRenderer.sortingOrder = (int)info;
                    break;
                
                default:
                    Debug.LogError("info type not supported");
                    break;
            }
            
            OnInfoChange?.Invoke(this, type);
        }

        public Vector3 GetPosition() => (Vector3)InfoManager.GetInfo(InfoType.Position);
        public Vector3 GetSize() => (Vector3)InfoManager.GetInfo(InfoType.Size);
        public float GetAngle() => (float)InfoManager.GetInfo(InfoType.Angle);
        public Color GetColor() => (Color)InfoManager.GetInfo(InfoType.Color);
        public int? GetTriggerID() => (int?)InfoManager.GetInfo(InfoType.TriggerID);
        public int GetSortingOrder() => (int)InfoManager.GetInfo(InfoType.SortingOrder);
    }
}