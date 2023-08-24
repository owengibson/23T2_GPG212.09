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
        [SerializeField] private GameObject nextPosition;
        [Space]

        [SerializeField] private bool isFinalPuzzle = false;

        private int _completedPuzzles;
        private bool _areAllPuzzlesCompleted = false;

        private void IncrementCompletedPuzzles(PuzzleType puzzleType)
        {
            if (puzzleType != this.puzzleType) return;

            _completedPuzzles++;

            if (_completedPuzzles == puzzles.Length)
            {
                // ALL PUZZLES OF THIS TYPE COMPLETED
                Debug.Log($"All {this.puzzleType} puzzles done.");
                particleSystem.SetActive(true);
                GetComponent<AudioSource>().Play();
                EventManager.onPuzzleTypeComplete?.Invoke();
                _areAllPuzzlesCompleted = true;
            }
        }

        private void CompleteAllPuzzles()
        {
            _completedPuzzles = puzzles.Length - 1;
            IncrementCompletedPuzzles(puzzleType);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (!_areAllPuzzlesCompleted) return;

            if (isFinalPuzzle) EventManager.onGameFinish?.Invoke();

            else other.transform.position = nextPosition.transform.position;


        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12)) CompleteAllPuzzles();
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
