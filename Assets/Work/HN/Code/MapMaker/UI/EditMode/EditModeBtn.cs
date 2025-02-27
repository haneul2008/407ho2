using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.HN.Code.EventSystems;

namespace Work.HN.Code.MapMaker.UI.EditMode
{
    public class EditModeBtn : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO mapMakerChannel;
        
        private TextMeshProUGUI _text;
        private EditType _editType;

        public void Initialize(EditType editType, EditBtnDataSO data)
        {
            _editType = editType;
            
            GetComponent<Image>().sprite = data.sprite;
        }

        public void OnClick()
        {
            EditModeChangeEvent editModeEvt = MapMakerEvent.EditModeChangeEvent;
            editModeEvt.editType = _editType;
            
            mapMakerChannel.RaiseEvent(editModeEvt);
        }
    }
}