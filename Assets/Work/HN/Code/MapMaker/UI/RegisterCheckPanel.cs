using System;
using UnityEngine;
using UnityEngine.UI;

namespace Work.HN.Code.MapMaker.UI
{
    public class RegisterCheckPanel : MonoBehaviour
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            gameObject.SetActive(false);
        }

        public void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}