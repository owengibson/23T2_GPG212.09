using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GPG212_09
{
    public class SimpleAISquadCoordinator : MonoBehaviour
    {
        [SerializeField] private List<NavMeshAgent> squadMembers;
        [SerializeField] private Transform[] squadPositions;

        private void Update()
        {
            for (int i = 0; i < squadMembers.Count; i++)
            {
                
            }
        }
    }
}
