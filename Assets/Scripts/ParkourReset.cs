using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG212_09
{
    public class ParkourReset : MonoBehaviour
    {
        [SerializeField] private GameObject resetPosition;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.transform.position = resetPosition.transform.position;
        }
    }
}
