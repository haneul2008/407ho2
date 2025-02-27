using System;
using UnityEditor;
using UnityEngine;
using Work.HN.Code.MapMaker.Objects.Triggers;

namespace Work.JW.Code.MapLoad
{
    public class InGameObject : MonoBehaviour
    {
        public int objectId;
        public string triggerID;
        public TriggerType triggerType;

        private Collider2D _col;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _col = GetComponent<Collider2D>();
            _sr = GetComponentInChildren<SpriteRenderer>();
        }

        public void SetPositionAndAngle(Vector2 pos, float angle) => transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, angle));
        public void SetScale(Vector3 scale) => transform.localScale = scale;
        public void SetIsTrigger(bool isTrigger) => _col.isTrigger = isTrigger;
        public void SetColor(Color color) => _sr.color = color;
    }
}