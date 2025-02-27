using System;
using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;

namespace Work.HN.Code.MapMaker.UI
{
    public class SettingPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private GameEventChannelSO mapMakerChannel;

        private void Awake()
        {
            nameField.onValueChanged.AddListener(HandleMapNameChanged);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            nameField.onValueChanged.RemoveListener(HandleMapNameChanged);
        }

        private void HandleMapNameChanged(string value)
        {
            MapNameChangeEvent evt = MapMakerEvent.MapNameChangeEvent;
            evt.mapName = value;
            mapMakerChannel.RaiseEvent(evt);
        }

        public void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}