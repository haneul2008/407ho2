using System;
using UnityEngine;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;
using Work.JW.Code.Network;

namespace Code.Network.UI
{
    public class WaitLoadingPanel : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO networkChannel;
        [SerializeField] private Image waitLoadingPanel;

        private void Awake()
        {
            networkChannel.AddListener<LoadingEvent>(HandleLoadingEvent);
            waitLoadingPanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            networkChannel.RemoveListener<LoadingEvent>(HandleLoadingEvent);
        }

        private void HandleLoadingEvent(LoadingEvent evt)
        {
            waitLoadingPanel.gameObject.SetActive(true);
        }
    }
}