using DG.Tweening;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    public class AlphaTrigger : Trigger
    {
        [SerializeField] private SpriteRenderer targetSpr;
        [SerializeField] private float alphaValue;
        [SerializeField] private float fadeTime;
        private SpriteRenderer[] _spriters;
        
        public override void TriggerEvent(Entity entity)
        {
            AlphaChange();
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

        public void SetData(float alpha, float duration)
        {
            alphaValue = alpha;
            fadeTime = duration;
        }

        public void AlphaChange()
        {
            Color color = targetSpr.color;
            color.a = alphaValue;
            
            if (_targets.Length <= 0) return;

            foreach (var item in _spriters)
            {
                item.DOColor(color, fadeTime);
            }
        }
    }
}