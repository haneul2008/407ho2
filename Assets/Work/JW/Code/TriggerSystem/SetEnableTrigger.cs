﻿using System;
using DG.Tweening;
using UnityEngine;
using Work.HN.Code.MapMaker.Objects.Triggers;
using Work.HN.Code.Save;
using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    public class SetEnableTrigger : Trigger
    {
        [SerializeField] private SpriteRenderer[] targetStrs;
        [SerializeField] private Collider2D[] targetCols;
        [SerializeField] private bool isEnable;
        [SerializeField] private float fadeTime;

        private Color[] _defaultColors;

        public override void TriggerEvent(Entity entity)
        {
            if (_targets == null) return;

            EnableOrDisable(isEnable);
        }

        public override void SetTargets(Transform[] targets)
        {
            base.SetTargets(targets);
            targetStrs = new SpriteRenderer[targets.Length];
            targetCols = new Collider2D[targets.Length];
            _defaultColors = new Color[targets.Length];

            for (int i = 0; i < targets.Length; i++)
            {
                targetStrs[i] = targets[i].GetComponentInChildren<SpriteRenderer>();
                targetCols[i] = targets[i].GetComponent<Collider2D>();

                _defaultColors[i] = targetStrs[i].color;
            }
        }

        public override void SetData(TriggerData data)
        {
            isEnable = data.triggerType != TriggerType.Destroy;

            fadeTime = 0.15f;
        }

        private void EnableOrDisable(bool enable)
        {
            if (enable)
            {
                for (int i = 0; i < _targets.Length; i++)
                {
                    targetStrs[i].gameObject.SetActive(true);
                    _defaultColors[i].a = 0;
                    targetStrs[i].color = _defaultColors[i];

                    targetStrs[i].DOFade(1, fadeTime);
                    targetCols[i].enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < _targets.Length; i++)
                {
                    var i1 = i;
                    targetStrs[i].DOFade(0, fadeTime).OnComplete(() =>
                    {
                        targetStrs[i1].gameObject.SetActive(false);
                        targetCols[i1].enabled = false;
                    });
                }
            }
        }
    }
}