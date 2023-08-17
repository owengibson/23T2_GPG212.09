using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG212_09
{
    public class CheckpointPuzzle : Puzzle
    {
        [SerializeField] private PuzzleType _puzzleType;
        private PuzzleState _puzzleState = PuzzleState.InProgress;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || _puzzleState == PuzzleState.Completed) return;

            _puzzleState = PuzzleState.Completed;
            EventManager.onPuzzleComplete?.Invoke(_puzzleType);
        }
    }
}
