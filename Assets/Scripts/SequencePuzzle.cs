using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG212_09
{
    public class SequencePuzzle : Puzzle
    {

        //[SerializeField] private SequenceBlock[] blocks;
        [SerializeField] private SequenceBlock[] sequence;
        [Space]

        [SerializeField] private int _index = 0;

        private PuzzleType _puzzleType = PuzzleType.Sequence;
        private PuzzleState _puzzleState = PuzzleState.InProgress;

        private void CheckNextBlockInSequence(SequenceBlock block)
        {
            if (_puzzleState == PuzzleState.Completed) return;

            if (block == sequence[_index])
            {
                _index++;
                if (_index >= sequence.Length)
                {
                    _puzzleState = PuzzleState.Completed;
                    EventManager.onPuzzleComplete?.Invoke(_puzzleType);
                }
            }
            else _index = 0;
        }

        /*private GameObject[] ShuffleArray(GameObject[] array)
        {
            GameObject[] result = new GameObject[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                GameObject tmp = array[i];
                int r = Random.Range(i, array.Length);
                result[i] = array[r];
                result[r] = tmp;
            }
            return result;
        }*/

        private void OnEnable()
        {
            EventManager.onSequenceBlockStep += CheckNextBlockInSequence;
        }
        private void OnDisable()
        {
            EventManager.onSequenceBlockStep -= CheckNextBlockInSequence;
        }
    }
}
