using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG212_09
{
    public class EventManager : MonoBehaviour
    {
        public static Action<PuzzleType> onPuzzleComplete;
        public static Action onPuzzleTypeComplete;
        public static Action<SequenceBlock> onSequenceBlockStep;
    }
}
