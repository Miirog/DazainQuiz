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
        }

        [SerializeField] private List<Question> _questions;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private Button[] answerButtons;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private StepManager _stepManager; // Reference to the StepManager

        [SerializeField] private string[] _resultsText;

        private int currentQuestionIndex = 0;
        private int totalPoints = 0;

        void Start()
        {
            DisplayQuestion();
        }

        public void OnAnswerSelected(int answerIndex)
        {
            totalPoints += _questions[currentQuestionIndex].points[answerIndex];
            currentQuestionIndex++;

            if (currentQuestionIndex < _questions.Count)
            {
                DisplayQuestion();
            }
            else
            {
                ShowResult();
            }
        }

        private void DisplayQuestion()
        {
            Question currentQuestion = _questions[currentQuestionIndex];
            questionText.text = currentQuestion.questionText;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (i < currentQuestion.answers.Length)
                {
                    answerButtons[i].gameObject.SetActive(true);
                    answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
                    int index = i; // Capture index for the button click
                    answerButtons[i].onClick.RemoveAllListeners();
                    answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
                }
                else
                {
                    answerButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void ShowResult()
        {
            _stepManager.NextStep();

            if (totalPoints <= 179)
            {
                resultText.text = _resultsText[0];
            }
            else if (totalPoints >= 180 && totalPoints <= 259)
            {
                resultText.text = _resultsText[1];
            }
            else if (totalPoints >= 260 && totalPoints <= 339)
            {
                resultText.text = _resultsText[2];
            }
            else
            {
                resultText.text = _resultsText[3];
            }

            resultText.gameObject.SetActive(true);
        }
    }
}