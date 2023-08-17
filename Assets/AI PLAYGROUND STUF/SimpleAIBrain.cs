using oezyowen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GPG212_09
{
    public class SimpleAIBrain : MonoBehaviour
    {
        public enum Tactics { NoTactics, SquadTactics, Undefined }

        public Tactics currentTactics = Tactics.NoTactics;

        private SimpleAIVision _vision;
        private NavMeshAgent _agent;
        private Vector3 _destination;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _vision = GetComponentInChildren<SimpleAIVision>();

            _destination = Utils.RandomRangeVector3(new Vector3(-25, 0, -25), new Vector3(25, 0, 25));
            _agent.SetDestination(_destination);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _destination = Utils.RandomRangeVector3(new Vector3(-25, 0, -25), new Vector3(25, 0, 25));
                _agent.SetDestination(_destination);
            }

            if (_vision.playerCharacter != null)
            {
                _agent.SetDestination(_vision.playerCharacter.transform.position);
            }
        }
    }
}
