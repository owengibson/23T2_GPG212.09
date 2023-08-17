using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG212_09
{
    public class SequenceBlock : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Colliding with " + other.name);
            if (!other.CompareTag("Player")) return;

            EventManager.onSequenceBlockStep?.Invoke(this);
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;

            Debug.Log("Colliding with " + collision.gameObject.name);
            EventManager.onSequenceBlockStep?.Invoke(gameObject);

        }*/

        /*private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Debug.Log("Colliding with " + hit.gameObject.name);
            if (!hit.gameObject.CompareTag("SequenceBlock")) return;

            EventManager.onSequenceBlockStep?.Invoke(hit.gameObject);
        }*/
    }
}
