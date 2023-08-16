using UnityEngine;
using UnityEngine.UI;
using oezyowen;
using UnityEngine.Rendering;
using StarterAssets;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

namespace GPG212_09
{
    public class ColourPuzzle : MonoBehaviour
    {
        public enum PuzzleState { Closed, InProgress, Matched, OnCooldown }
        public PuzzleState puzzleState = PuzzleState.Closed;
        [Space]

        [Header("Slider References")]
        [SerializeField] private Slider redSlider;
        [SerializeField] private Slider greenSlider;
        [SerializeField] private Slider blueSlider;
        [Space]
        [SerializeField] private Slider timerSlider;
        [Space]

        [Header("Colour Preview References")]
        [SerializeField] private Image currentColourPreview;
        [SerializeField] private Image targetColourPreview;
        [Space]

        [Header("Other References")]
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject successText;
        [SerializeField] private GameObject failureText;
        [SerializeField] private GameObject proximityPopup;
        [SerializeField] private TextMeshProUGUI proximityPopupText;
        [Space]

        [Header("Parameters")]
        [SerializeField] private float colourMatchThreshold;
        [SerializeField] private float timeToComplete;
        [SerializeField] private float attemptCooldown;

        private readonly Vector3 inactiveCanvasPos = new Vector3(0.0719999969f, 1.01199996f, 0);
        private readonly Vector3 inactiveCanvasRot = new Vector3(56.4899979f, 270, 0);
        private readonly Vector3 inactiveCanvasScale = new Vector3(0.00039999999f, 0.00039999999f, 0.00039999999f);

        private Color _currentColour;
        private Color _targetColour;

        private float _redLow, _redHigh, _greenLow, _greenHigh, _blueLow, _blueHigh;
        private float _currentTime = 0f;
        private bool _isInTriggerArea = false;

        private StarterAssetsInputs _playerInput;

        private void Start()
        {
            GenerateColour();
        }

        private void GenerateColour()
        {
            _targetColour = Utils.GetRandomColor();
            targetColourPreview.color = _targetColour;

            _redLow = _targetColour.r - colourMatchThreshold;
            _redHigh = _targetColour.r + colourMatchThreshold;
            _greenLow = _targetColour.g - colourMatchThreshold;
            _greenHigh = _targetColour.g + colourMatchThreshold;
            _blueLow = _targetColour.b - colourMatchThreshold;
            _blueHigh = _targetColour.b + colourMatchThreshold;

            Debug.Log($"Target colour: {_targetColour.r}, {_targetColour.g}, {_targetColour.b}");
        }

        public void UpdateCurrentColour()
        {
            _currentColour = new Color(redSlider.value, greenSlider.value, blueSlider.value);
            currentColourPreview.color = _currentColour;
        }

        public void CheckColourMatch()
        {
            if (puzzleState == PuzzleState.Matched) return;

            if((_redLow <= redSlider.value && redSlider.value <= _redHigh) &&
                (_greenLow <= greenSlider.value && greenSlider.value <= _greenHigh) &&
                (_blueLow <= blueSlider.value && blueSlider.value <= _blueHigh))
            {
                // PUZZLE SUCCEEDED
                puzzleState = PuzzleState.Matched;
                Debug.Log("Current colour matches target colour");
                successText.SetActive(true);
                Invoke("ClosePuzzle", 1f);

            }
            else
            {
                // PUZZLE FAILED
                StartCoroutine(PuzzleCooldown(timeToComplete));
            }
        }

        public void ClosePuzzle()
        {
            if (puzzleState != PuzzleState.Matched) puzzleState = PuzzleState.Closed;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _playerInput.cursorInputForLook = true;
            EventSystem.current.SetSelectedGameObject(null);

            canvas.GetComponent<GraphicRaycaster>().enabled = false;
            canvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;

            RectTransform rectTransform = canvas.GetComponent<RectTransform>();
            rectTransform.localPosition = inactiveCanvasPos;
            rectTransform.localEulerAngles = inactiveCanvasRot;
            rectTransform.localScale = inactiveCanvasScale;
        }

        public void OpenPuzzle()
        {
            if (puzzleState != PuzzleState.Matched) puzzleState = PuzzleState.InProgress;

            proximityPopup.SetActive(false);

            Cursor.visible = true;
            _playerInput.cursorInputForLook = false;
            Cursor.lockState = CursorLockMode.Confined;

            canvas.GetComponent <GraphicRaycaster>().enabled = true;
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

        private IEnumerator PuzzleCooldown(float cooldown)
        {
            ClosePuzzle();
            puzzleState = PuzzleState.OnCooldown;
            failureText.SetActive(true);
            proximityPopupText.text = "Puzzle on cooldown";

            yield return new WaitForSeconds(cooldown);

            failureText.SetActive(false);
            proximityPopupText.text = "Press E to open puzzle";

            GenerateColour();
            ResetSliders();
            
            puzzleState = PuzzleState.Closed;
        }

        private void ResetSliders()
        {
            redSlider.value = 0;
            greenSlider.value = 0;
            blueSlider.value = 0;
            currentColourPreview.color = Color.black;

            timerSlider.value = 1;
            _currentTime = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (puzzleState == PuzzleState.Matched) return;

            proximityPopup.SetActive(true);
            _isInTriggerArea = true;
            _playerInput = other.GetComponent<StarterAssetsInputs>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (puzzleState == PuzzleState.Matched) return;

            ClosePuzzle();
            proximityPopup.SetActive(false);
        }

        private void Update()
        {
            if (puzzleState != PuzzleState.Matched)
            {
                if (_isInTriggerArea && puzzleState == PuzzleState.Closed)
                {
                    if (Input.GetKeyDown(KeyCode.E)) OpenPuzzle();
                }

                if (puzzleState == PuzzleState.InProgress)
                {
                    _currentTime += Time.deltaTime;
                    timerSlider.value = 1 - _currentTime / timeToComplete;
                    if (_currentTime >= timeToComplete)
                    {
                        CheckColourMatch();
                    }
                }
            }
            
        }
    }
}