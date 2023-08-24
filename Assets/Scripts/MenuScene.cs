using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GPG212_09
{
    public class MenuScene : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        
        void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (SceneManager.GetActiveScene().name == "EndScene")
            {
                timerText.text = PlayerPrefs.GetFloat("FinalTime").ToString("00.00");
            }
        }
    }
}
