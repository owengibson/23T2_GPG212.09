using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GPG212_09
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private float _time = 0f;
        private bool _hasGameFinished = false;

        private void Awake()
        {
            timerText.text = "00.00";
            PlayerPrefs.DeleteAll();
        }

        private void Update()
        {
            if (!_hasGameFinished)
            {
                _time += Time.deltaTime;
                timerText.text = _time.ToString("00.00");
            }
        }

        private void SubmitScore()
        {
            _hasGameFinished = true;
            PlayerPrefs.SetFloat("FinalTime", _time);
        }

        private void OnEnable()
        {
            EventManager.onGameFinish += SubmitScore;
        }
        private void OnDisable()
        {
            EventManager.onGameFinish -= SubmitScore;
        }
    }
}
