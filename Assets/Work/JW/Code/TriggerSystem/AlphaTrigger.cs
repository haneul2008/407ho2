using DG.Tweening;
using UnityEngine;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.HN.Code.Save;
using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    public class AlphaTrigger : Trigger
    {
        [SerializeField] private float alphaValue;
        [SerializeField] private float fadeTime;
        private SpriteRenderer[] _spriters;
        
        public override void TriggerEvent(Entity entity)
        {
            if (_targets == null) return;
            if(_isTrigger) return;

            AlphaChange(entity);
        }

        public override void SetTargets(Transform[] targets)
        {
            base.SetTargets(targets);
            _spriters = new SpriteRenderer[targets.Length];

            for (int i = 0; i < targets.Length; i++)
            {
                _spriters[i] = targets[i].GetComponentInChildren<SpriteRenderer>();
            }
        }

        public override void SetData(TriggerData data)
        {
            AlphaInfo info = data.alphaInfo;
            
            alphaValue = info.endValue;
            fadeTime = info.duration;
        }

        public void AlphaChange(Entity entity)
        {
            _isTrigger = true;

            foreach (var item in _spriters)
            {
                Color color = item.color;
                color.a = alphaValue;
                
                item.DOColor(color, fadeTime).OnComplete(() =>
                {
                    _isTrigger = false;
                    base.TriggerEvent(entity);
                });
            }
        }
    }
}