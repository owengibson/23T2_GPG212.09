using UnityEngine;
using UnityEngine.UI;
using oezyowen;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using StarterAssets;

namespace GPG212_09
{
    public class ColourPuzzle : Puzzle
    {
        [SerializeField] private PuzzleState puzzleState = PuzzleState.Closed;
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

        private PuzzleType _puzzleType = PuzzleType.Colour;

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

            CheckColourMatch();
        }

        public void CheckColourMatch()
        {
            if (puzzleState != PuzzleState.InProgress) return;

            if((_redLow <= redSlider.value && redSlider.value <= _redHigh) &&
                (_greenLow <= greenSlider.value && greenSlider.value <= _greenHigh) &&
                (_blueLow <= blueSlider.value && blueSlider.value <= _blueHigh))
            {
                // PUZZLE SUCCEEDED
                puzzleState = PuzzleState.Completed;
                Debug.Log("Current colour matches target colour");
                successText.SetActive(true);
                Invoke("ClosePuzzle", 1f);
                EventManager.onPuzzleComplete?.Invoke(_puzzleType);

            }
            /*else
            {
                // PUZZLE FAILED
                StartCoroutine(PuzzleCooldown(attemptCooldown));
            }*/
        }

        public void ClosePuzzle()
        {
            if (puzzleState != PuzzleState.Completed) puzzleState = PuzzleState.Closed;

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

        public void OpenPuzzle()
        {
            if (puzzleState != PuzzleState.Completed) puzzleState = PuzzleState.InProgress;

            proximityPopup.SetActive(false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _playerInput.cursorInputForLook = false;
            //_characterController.enabled = false;

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

        private void Update()
        {
            if (puzzleState != PuzzleState.Completed)
            {
                if (_isInTriggerArea && puzzleState == PuzzleState.Closed)
                {
                    if (Input.GetKeyDown(KeyCode.E)) OpenPuzzle();
                }

                if (puzzleState == PuzzleState.InProgress)
                {
                    if (Input.GetKeyDown(KeyCode.Escape)) ClosePuzzle();

                    /*_currentTime += Time.deltaTime;
                    timerSlider.value = 1 - _currentTime / timeToComplete;
                    if (_currentTime >= timeToComplete)
                    {
                        CheckColourMatch();
                    }*/
                }
            }
            
        }
    }
}