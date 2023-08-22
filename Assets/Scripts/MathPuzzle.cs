using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GPG212_09
{
    public class MathPuzzle : MonoBehaviour
    {
        public PuzzleState puzzleState = PuzzleState.Closed;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private TMP_InputField answerInputField;
        [SerializeField] private Slider timerSlider;
        [Space]

        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private GameObject puzzlePanel;
        [SerializeField] private GameObject successPanel;
        [SerializeField] private GameObject failPanel;
        [Space]

        [SerializeField] private GameObject proximityPopup;
        [Space]

        [Header("Parameters")]
        [SerializeField] private float timeToComplete;
        [SerializeField] private int maxNumberOfRounds;

        private StarterAssetsInputs _playerInput;

        private readonly Vector3 inactiveCanvasPos = new Vector3(0.0719999969f, 1.01199996f, 0);
        private readonly Vector3 inactiveCanvasRot = new Vector3(56.4899979f, 270, 0);
        private readonly Vector3 inactiveCanvasScale = new Vector3(0.00039999999f, 0.00039999999f, 0.00039999999f);

        private bool _isInTriggerArea = false;

        private int _answer;
        private int _currentRound = 1;
        private float _currentTime = 0f;

        private void Awake()
        {
            
        }

        private void Update()
        {
            if (puzzleState == PuzzleState.Closed && _isInTriggerArea)
            {
                if (Input.GetKeyDown(KeyCode.E)) OpenPuzzle();
            }

            if (puzzleState == PuzzleState.Open)
            {
                if (Input.GetKeyDown(KeyCode.Return)) StartPuzzle();
            }

            if (puzzleState == PuzzleState.InProgress)
            {
                _currentTime += Time.deltaTime;
                timerSlider.value = 1 - _currentTime / timeToComplete;
                if (_currentTime >= timeToComplete)
                {
                    // when timer runs out
                    failPanel.SetActive(true);
                    answerInputField.DeactivateInputField();
                    Invoke("ClosePuzzle", 1f);
                    Invoke("ResetPuzzle", 1f);
                }
            }
        }

        private void OpenPuzzle()
        {
            if (puzzleState != PuzzleState.Closed) return;
            puzzleState = PuzzleState.Open;

            proximityPopup.SetActive(false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _playerInput.cursorInputForLook = false;

            canvas.GetComponent<GraphicRaycaster>().enabled = true;
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

        private void ClosePuzzle()
        {
            if (puzzleState != PuzzleState.Completed) puzzleState = PuzzleState.Closed;
            if (failPanel.activeSelf) failPanel.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _playerInput.cursorInputForLook = true;
            //_characterController.enabled = true;
            EventSystem.current.SetSelectedGameObject(null);

            canvas.GetComponent<GraphicRaycaster>().enabled = false;
            canvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;

            RectTransform rectTransform = canvas.GetComponent<RectTransform>();
            rectTransform.localPosition = inactiveCanvasPos;
            rectTransform.localEulerAngles = inactiveCanvasRot;
            rectTransform.localScale = inactiveCanvasScale;

            if (_isInTriggerArea && puzzleState != PuzzleState.Completed) proximityPopup.SetActive(true);
        }

        private void StartPuzzle()
        {
            tutorialPanel.SetActive(false);
            puzzlePanel.SetActive(true);

            if (puzzleState == PuzzleState.Open) puzzleState = PuzzleState.InProgress;
            StartPuzzleRound();
        }

        private void StartPuzzleRound()
        {
            int a = Random.Range(1, 10);
            int b = Random.Range(1, 10);
            _answer = a + b;

            questionText.text = $"{a} + {b} = ";
            answerInputField.text = "";
            answerInputField.ActivateInputField();
        }

        public void CheckPuzzleAnswer()
        {
            if (int.Parse(answerInputField.text) == _answer)
            {
                if (_currentRound == maxNumberOfRounds)
                {
                    successPanel.SetActive(true);
                    puzzleState = PuzzleState.Completed;
                    Invoke("ClosePuzzle", 1f);
                }

                else
                {
                    _currentRound++;
                    StartPuzzleRound();
                }
            }
            else
            {
                failPanel.SetActive(true);
                answerInputField.DeactivateInputField();
                Invoke("ClosePuzzle", 1f);
                Invoke("ResetPuzzle", 1f);
            }
        }
        private void ResetPuzzle()
        {
            tutorialPanel.SetActive(true);
            puzzlePanel.SetActive(false);
            _currentRound = 1;
            _currentTime = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (puzzleState == PuzzleState.Completed) return;

            proximityPopup.SetActive(true);
            _isInTriggerArea = true;
            _playerInput = other.GetComponent<StarterAssetsInputs>();

        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (puzzleState == PuzzleState.Completed) return;

            if (puzzleState == PuzzleState.InProgress) ClosePuzzle();

            proximityPopup.SetActive(false);
            _isInTriggerArea = false;
        }
    }
}
