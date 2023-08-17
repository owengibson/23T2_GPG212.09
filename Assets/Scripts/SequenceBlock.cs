using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG212_09
{
    public class SequenceBlock : MonoBehaviour
    {
        private AudioSource _audio;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Colliding with " + other.name);
            if (!other.CompareTag("Player")) return;

            _audio.Play();
            EventManager.onSequenceBlockStep?.Invoke(this);
        }
    }
}
