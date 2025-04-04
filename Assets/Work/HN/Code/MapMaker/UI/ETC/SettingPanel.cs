using Ami.BroAudio;
using TMPro;
using UnityEngine;
using Work.HN.Code.EventSystems;
using Work.HN.Code.Save;
using Work.ISC._0._Scripts.Save.Firebase;

namespace Work.HN.Code.MapMaker.UI
{
    public class SettingPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private ObjectInvoker objectInvoker;
        [SerializeField] private TextMeshProUGUI capacityText;
        [SerializeField] private int maxText = 10;

        [SerializeField] private SoundID clickSoundID;

        private string _mapName;

        private void Awake()
        {
            nameField.onValueChanged.AddListener(HandleMapNameChanged);
        }

        private void OnDestroy()
        {
            nameField.onValueChanged.RemoveListener(HandleMapNameChanged);
        }

        private void Start()
        {
            _mapName = saveManager.GetMapName();
            gameObject.SetActive(false);
        }

        private void HandleMapNameChanged(string value)
        {
            if (value.Length > maxText)
            {
                nameField.text = value.Substring(0, maxText);
                return;
            }

            _mapName = value;

            MapNameChangeEvent evt = MapMakerEvent.MapNameChangeEvent;
            evt.mapName = value;
            mapMakerChannel.RaiseEvent(evt);
        }

        public void Active(bool isActive)
        {
            BroAudio.Play(clickSoundID);

            gameObject.SetActive(isActive);

            if (isActive)
            {
                nameField.text = _mapName;

                capacityText.text = $"맵 크기 : {objectInvoker.GetMapCapacity()} / {FirebaseData.maxCapacity}";
            }
        }
    }
}