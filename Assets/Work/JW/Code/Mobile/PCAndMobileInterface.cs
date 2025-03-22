using System;
using DG.Tweening;
using UnityEngine;
using Work.JW.Code.Entities;
using Work.JW.Code.Entities.Player;

namespace Work.JW.Code.Mobile
{
    public class PCAndMobileInterface : MonoBehaviour
    {
        [SerializeField] private GameObject pcInterfaceImage;
        [SerializeField] private GameObject mobileInterfaceImage;
        [SerializeField] private float moveY = 15f;

        private void Awake()
        {
#if UNITY_IOS || UNITY_ANDROID
            pcInterfaceImage.gameObject.SetActive(false);
            mobileInterfaceImage.gameObject.SetActive(true);
            return;
#endif
            pcInterfaceImage.gameObject.SetActive(true);
            mobileInterfaceImage.gameObject.SetActive(false);

            float moveYPos = transform.position.y + moveY;
            DOVirtual.DelayedCall(2.5f,
                () => { pcInterfaceImage.transform.DOMoveY(-moveYPos, 1f).SetEase(Ease.InBack); });
        }
    }
}