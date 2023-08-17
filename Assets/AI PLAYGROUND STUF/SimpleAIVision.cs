using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG212_09
{
    public class SimpleAIVision : MonoBehaviour
    {
        public GameObject playerCharacter;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            playerCharacter = other.gameObject;
        }
    }
}
