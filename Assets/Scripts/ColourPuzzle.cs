using UnityEngine;
using UnityEngine.UI;
using oezyowen;
using UnityEngine.Rendering;
using StarterAssets;
using UnityEngine.EventSystems;

namespace GPG212_09
{
    public class ColourPuzzle : MonoBehaviour
    {
        [Header("Slider References")]
        [SerializeField] private Slider redSlider;
        [SerializeField] private Slider greenSlider;
        [SerializeField] private Slider blueSlider;
        [Space]

        [Header("Colour Preview References")]
        [SerializeField] private Image currentColourPreview;
        [SerializeField] private Image targetColourPreview;
        [Space]

        [Header("Other References")]
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject successText;
        [SerializeField] private GameObject proximityPopup;
        [Space]

        [Header("Parameters")]
        [SerializeField] private float colourMatchThreshold;

        private readonly Vector3 activeCanvasPos = new Vector3(1280, 720, 0);
        private readonly Vector3 activeCanvasScale = new Vector3(1.33333337f, 1.33333337f, 1.33333337f);

        private readonly Vector3 inactiveCanvasPos = new Vector3(0.0719999969f, 1.01199996f, 0);
        private readonly Vector3 inactiveCanvasRot = new Vector3(56.4899979f, 270, 0);
        private readonly Vector3 inactiveCanvasScale = new Vector3(0.00039999999f, 0.00039999999f, 0.00039999999f);

        private Color _currentColour;
        private Color _targetColour;

        private float _redLow, _redHigh, _greenLow, _greenHigh, _blueLow, _blueHigh;
        private bool _hasColourBeenMatched = false;
        private bool _isInTriggerArea = false;

        private StarterAssetsInputs _playerInput;

        private void Start()
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
            if (_hasColourBeenMatched) return;

            if((_redLow <= redSlider.value && redSlider.value <= _redHigh) &&
                (_greenLow <= greenSlider.value && greenSlider.value <= _greenHigh) &&
                (_blueLow <= blueSlider.value && blueSlider.value <= _blueHigh))
            {
                // PUZZLE SUCCEEDED
                _hasColourBeenMatched = true;
                Debug.Log("Current colour matches target colour");
                successText.SetActive(true);
                Invoke("ClosePuzzle", 1f);

            }
        }

        public void ClosePuzzle()
        {
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
            proximityPopup.SetActive(false);

            Cursor.visible = true;
            _playerInput.cursorInputForLook = false;
            Cursor.lockState = CursorLockMode.Confined;

            canvas.GetComponent <GraphicRaycaster>().enabled = true;
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            proximityPopup.SetActive(true);
            _isInTriggerArea = true;
            _playerInput = other.GetComponent<StarterAssetsInputs>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            ClosePuzzle();
            proximityPopup.SetActive(false);
        }

        private void Update()
        {
            if (_isInTriggerArea && !_hasColourBeenMatched)
            {
                if (Input.GetKeyDown(KeyCode.E)) OpenPuzzle();
            }
        }
    }
}