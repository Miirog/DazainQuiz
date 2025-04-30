using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

namespace DZ.Step
{

    public class StepManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _steps; // Array of steps to be managed	
        [SerializeField] private GameObject _currentStep; // The current step being displayed
        [SerializeField] private Button[] _nextButtons; // Button to proceed to the next step
        [SerializeField] private TMP_InputField _desktopInputField; // Input field for desktop
        [SerializeField] private TMP_InputField _mobileInputField; // Input field for mobile

        private string _inputText; // Variable to store the input text

        private void Start()
        {
            // Initialize the first step
            if (_steps.Length > 0)
            {
                _currentStep = _steps[0];
                _currentStep.SetActive(true);
                if (_nextButtons != null)
                {
                    foreach (var button in _nextButtons)
                    {
                        button.interactable = false;
                        button.onClick.AddListener(OnNextButtonClicked); // Add listener to each button
                    }
                }

                _desktopInputField.onValueChanged.AddListener(OnInputFieldChanged); // Add listener to the desktop input field
                _mobileInputField.onValueChanged.AddListener(OnInputFieldChanged); // Add listener to the mobile input field
            }
        }

        private void OnInputFieldChanged(string arg0)
        {
            _inputText = arg0;

            foreach (var button in _nextButtons)
            {
                button.interactable = true;
            }
        }

        private void OnNextButtonClicked()
        {
            StartCoroutine(SendToGoogleSheets(_inputText));

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

        private IEnumerator SendToGoogleSheets(string inputText)
        {
            string url = "https://script.google.com/macros/s/AKfycbw4xFk_BuA3NWxxrkIegz7KGXoStMm5GgVh1sfn9zTCWGNZ2d1lbIRzR4fuu7uwaZS8ew/exec"; // Replace with your Web App URL

            // Create JSON payload using the serializable class
            GoogleSheetsPayload payload = new GoogleSheetsPayload { inputText = inputText };
            string jsonPayload = JsonUtility.ToJson(payload);

            Debug.Log("JSON Payload: " + jsonPayload); // Log the JSON payload

            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Data sent to Google Sheets successfully!");
                    Debug.Log("Response: " + www.downloadHandler.text); // Log the response from the server
                }
                else
                {
                    Debug.LogError("Failed to send data: " + www.error);
                }
            }
        }

        [Serializable]
        public class GoogleSheetsPayload
        {
            public string inputText;
        }
    }
}
