using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG212_09
{
    public class WorldSpacePuzzle : Puzzle
    {
        [SerializeField] private PuzzleType _puzzleType = PuzzleType.Maze;

        private PuzzleState _puzzleState = PuzzleState.Closed;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_puzzleState == PuzzleState.Completed) return;

            _puzzleState = PuzzleState.Completed;
            EventManager.onPuzzleComplete?.Invoke(_puzzleType);
        }
    }
}
