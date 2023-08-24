using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GPG212_09
{
    public class FinishLine : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            EventManager.onGameFinish?.Invoke();
        }
    }
}
