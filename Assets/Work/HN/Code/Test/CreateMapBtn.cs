using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Work.HN.Code.Save;

namespace Work.HN.Code.Test
{
    public class CreateMapBtn : MonoBehaviour
    {
        public void OnClick()
        {
            DataReceiver.Instance.CreateNewMap();
            SceneManager.LoadScene("HN");
        }
    }
}