using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GPG212_09
{
    public class SceneSwapper : MonoBehaviour
    {
        public void LoadMain()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void LoadEnd()
        {
            SceneManager.LoadScene("EndScene");
        }

        public void QuitApplication()
        {
            Application.Quit(); 
        }

        private void OnEnable()
        {
            EventManager.onGameFinish += LoadEnd;
        }

        private void OnDisable()
        {
            EventManager.onGameFinish -= LoadEnd;
        }
    }
}
