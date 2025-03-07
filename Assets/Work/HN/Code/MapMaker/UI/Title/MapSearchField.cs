using TMPro;
using UnityEngine;
using Work.ISC._0._Scripts.UI.Data;

public class MapSearchField : MonoBehaviour
{
    [SerializeField] private TMP_InputField mapNameField;
    [SerializeField] private DataPanelsLoader dataPanelLoader;

    public void OnClick()
    {
        dataPanelLoader.Search(mapNameField.text);
    }
}
