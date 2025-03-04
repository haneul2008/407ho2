using System;
using System.Collections;
using UnityEngine;

namespace Work.JW.Code.MapLoad.UI
{
    public class FadeInOut : MonoBehaviour
    {
        private static readonly int FadeValueHash = Shader.PropertyToID("_FadeValue");
        private SpriteRenderer _spriter;
        private Material _material;

        private void Awake()
        {
            _spriter = GetComponent<SpriteRenderer>();
            _material = _spriter.material;
        }

        private void Start()
        {
            _material.SetFloat(FadeValueHash, 0);
        }

        public void Fade(bool isFadeIn)
        {
            if (isFadeIn)
            {
                StopAllCoroutines();
                StartCoroutine(StartFadeIn());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(StartFadeOut());
            }
        }

        private IEnumerator StartFadeIn()
        {
            float duration = 0;
            float value = 35;
            _material.SetFloat(FadeValueHash, value);
            
            while (duration < 1)
            {
                value -= 1;
                duration += Time.deltaTime;
                _material.SetFloat(FadeValueHash, value);
                yield return null;
            }
        }
        
        private IEnumerator StartFadeOut()
        {
            float value = 0;
            float duration = 0;
            _material.SetFloat(FadeValueHash, value);
            
            while (duration < 1)
            {
                value += 1;
                duration += Time.deltaTime;
                _material.SetFloat(FadeValueHash, value);
                yield return null;
            }
        }
    }
}