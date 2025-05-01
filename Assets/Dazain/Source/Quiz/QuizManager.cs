using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DZ.Step;

namespace DZ.Quiz
{
    public class QuizManager : MonoBehaviour
    {
        [System.Serializable]
        public class Question
        {
            public string questionText;
            public string[] answers;
            public int[] points; // Points corresponding to each answer
            public Sprite _sprite;
            public Color _color;
        }

        [SerializeField] private List<Question> _questions;
        [SerializeField] private TextMeshProUGUI questionTextDesktop;
        [SerializeField] private TextMeshProUGUI questionTextMobile;

        [SerializeField] private Button[] desktopAnswerButtons;
        [SerializeField] private Button[] mobileAnswerButtons;

        [SerializeField] private Image resultImage;
        [SerializeField] private StepManager _stepManager; // Reference to the StepManager

        [SerializeField] private Image _bgImage;
        [SerializeField] private Image _bgColor;

        [SerializeField] private Sprite[] _resultsImgDesktop;
        [SerializeField] private Sprite[] _resultsImgMobile;

        [SerializeField] private TextMeshProUGUI[] resultTextDesktop;
        [SerializeField] private TextMeshProUGUI[] resultTextMobile;

        [SerializeField] private string[] _resultsTextsTitle;
        [SerializeField] private string[] _resultsTextsDescription;
        [SerializeField] private string[] _resultsTextsDescription2;
        [SerializeField] private string[] _simplifiedResultsTextsDescription;

        [SerializeField] private TextMeshProUGUI[] _totalquestions;

        private int currentQuestionIndex = 0;
        private int totalPoints = 0;

        void Start()
        {
            DisplayQuestion();
        }

        public void OnAnswerSelected(int answerIndex)
        {
            if (currentQuestionIndex >= 9)
            {
                Debug.LogWarning("No more questions available. Showing results.");
                ShowResult();
                return;
            }

            // Add points for the selected answer
            totalPoints += _questions[currentQuestionIndex].points[answerIndex];
            currentQuestionIndex++;

            for (int i = 0; i < _totalquestions.Length; i++)
            {
                _totalquestions[i].text = (currentQuestionIndex + 1).ToString() + "/" + _questions.Count.ToString();

                // Check if there are more questions
                if (currentQuestionIndex < _questions.Count)
                {
                    DisplayQuestion();
                }
                else
                {
                    ShowResult();
                }
            }
        }


        private void DisplayQuestion()
        {
            Question currentQuestion = _questions[currentQuestionIndex];
            questionTextDesktop.text = currentQuestion.questionText;
            questionTextMobile.text = currentQuestion.questionText;

            // Update desktop and mobile buttons
            UpdateButtons(desktopAnswerButtons, currentQuestion);
            UpdateButtons(mobileAnswerButtons, currentQuestion);

            // Update background image and color
            _bgImage.sprite = currentQuestion._sprite;
            _bgColor.color = currentQuestion._color;
        }

        private void UpdateButtons(Button[] buttons, Question currentQuestion)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < currentQuestion.answers.Length && i < currentQuestion.points.Length)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];

                    int index = i; // Capture index for the button click
                    buttons[i].onClick.RemoveAllListeners();
                    buttons[i].onClick.AddListener(() => OnAnswerSelected(index));
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        private void ShowResult()
        {
            _stepManager.NextStep();

            if (totalPoints <= 179)
            {
                CheckOrientation(0);
            }
            else if (totalPoints >= 180 && totalPoints <= 259)
            {
                CheckOrientation(1);
            }
            else if (totalPoints >= 260 && totalPoints <= 339)
            {
                CheckOrientation(2);
            }
            else
            {
                CheckOrientation(3);
            }

            resultImage.gameObject.SetActive(true);
        }

        private void CheckOrientation(int index)
        {
            float aspectRatio = (float)Screen.width / Screen.height;

            StartCoroutine(_stepManager.SendToGoogleSheets(_simplifiedResultsTextsDescription[index]));

            if (aspectRatio > 1f)
            {
                resultImage.sprite = _resultsImgDesktop[index];
                resultTextDesktop[0].text = _resultsTextsTitle[index];
                resultTextDesktop[1].text = _resultsTextsDescription[index];
                resultTextDesktop[2].text = _resultsTextsDescription2[index];
            }
            else if (aspectRatio < 1f)
            {
                resultImage.sprite = _resultsImgMobile[index];
                resultTextMobile[0].text = _resultsTextsTitle[index];
                resultTextMobile[1].text = _resultsTextsDescription[index];
                resultTextMobile[2].text = _resultsTextsDescription2[index];
            }
        }
    }
}

