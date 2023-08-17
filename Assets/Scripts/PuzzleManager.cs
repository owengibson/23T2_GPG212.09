using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG212_09
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] private PuzzleType puzzleType;
        [SerializeField] private Puzzle[] puzzles;
        [Space]

        [SerializeField] private GameObject particleSystem;

        private int _completedPuzzles;

        private void IncrementCompletedPuzzles(PuzzleType puzzleState)
        {
            if (puzzleState != puzzleType) return;

            _completedPuzzles++;

            if (_completedPuzzles == puzzles.Length)
            {
                // ALL PUZZLES OF THIS TYPE COMPLETED
                Debug.Log($"All {puzzleType} puzzles done.");
                particleSystem.SetActive(true);
            }
        }

        private void OnEnable()
        {
            EventManager.onPuzzleComplete += IncrementCompletedPuzzles;
        }
        private void OnDisable()
        {
            EventManager.onPuzzleComplete -= IncrementCompletedPuzzles;
        }
    }
}
