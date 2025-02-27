using UnityEngine;
using Work.HN.Code.EventSystems;

namespace Work.HN.Code.MapMaker.UI.EditMode
{
    [CreateAssetMenu(menuName = "SO/EditModeBtn")]
    public class EditBtnDataSO : ScriptableObject
    {
        public EditType editType;
        public Sprite sprite;
    }
}