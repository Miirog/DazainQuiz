using System;
using UnityEngine;
using UnityEngine.UI;

namespace DZ.Step
{

    public class StepManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _steps; // Array of steps to be managed	
        [SerializeField] private GameObject _currentStep; // The current step being displayed
        [SerializeField] private Button _nextButton; // Button to proceed to the next step

        private void Start()
        {
            // Initialize the first step
            if (_steps.Length > 0)
            {
                _currentStep = _steps[0];
                _currentStep.SetActive(true);
                if (_nextButton != null)
                {
                    _nextButton.onClick.AddListener(OnNextButtonClicked);
                }
            }
        }

        private void OnNextButtonClicked()
        {
            // Proceed to the next step when the button is clicked
            NextStep();
        }

        public void NextStep()
        {
            // Deactivate the current step
            if (_currentStep != null)
            {
                _currentStep.SetActive(false);
            }

            // Find the next step in the array
            int currentIndex = Array.IndexOf(_steps, _currentStep);
            int nextIndex = (currentIndex + 1) % _steps.Length; // Loop back to the first step if at the end

            Debug.Log("Current step: " + _currentStep);
            // Activate the next step
            _currentStep = _steps[nextIndex];
            Debug.Log("Next step: " + _currentStep);
            _currentStep.SetActive(true);
        }
    }
}
