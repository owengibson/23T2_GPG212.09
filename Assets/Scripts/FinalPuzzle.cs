using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace GPG212_09
{
    public class FinalPuzzle : MonoBehaviour
    {
        [SerializeField] private PuzzleManager[] puzzleManagers;
        [SerializeField] private GameObject door;

        private int _finishedPuzzles;

        private void IncrementFinishedPuzzleTypes()
        {
            _finishedPuzzles++;
            if (_finishedPuzzles == puzzleManagers.Length) DeactivateDoor();
        }

        private void DeactivateDoor()
        {
            door.SetActive(false);
        }

        private void OnEnable()
        {
            EventManager.onPuzzleTypeComplete += IncrementFinishedPuzzleTypes;
        }
        private void OnDisable()
        {
            EventManager.onPuzzleTypeComplete -= IncrementFinishedPuzzleTypes;
        }
    }
}
