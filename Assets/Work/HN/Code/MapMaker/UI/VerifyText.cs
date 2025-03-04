using TMPro;
using UnityEngine;
using Work.HN.Code.Save;

public class VerifyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void HandleDataChanged(MapData mapData)
    {
        text.text = mapData.isVerified ? "���� ����" : "���� ����";
    }
}
