using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Work.HN.Code.ETC
{
    public class FontAssetGenerator : MonoBehaviour
    {
        [SerializeField] private TMP_FontAsset fontAsset;
        
        [ContextMenu("Generate FontAsset")]
        public void GenerateFontAsset()
        {
            List<TMP_Text> texts = FindObjectsByType<TMP_Text>(FindObjectsSortMode.None).ToList();
            List<TextMeshProUGUI> tmpUGUIs = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None).ToList();
            
            texts.ForEach(text => text.font = fontAsset);
            tmpUGUIs.ForEach(text => text.font = fontAsset);
        }
    }
}