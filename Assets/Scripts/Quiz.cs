using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    [SerializeField] QuestionSO currentquestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;
    [Header("Button Colours")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreTracker scoreTracker;
    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    public bool isComplete = false;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        scoreTracker = FindObjectOfType<ScoreTracker>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update() 
    {
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion)
        {
            if(progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
            }
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }
    public void OnAnswersSelected(int Index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(Index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreTracker.CalculateScore() + "%";
     
        
    }

    void DisplayAnswer(int Index)
    {
        Image buttonImage;
        if(Index == currentquestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct";
            buttonImage = answerButtons[Index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreTracker.IncrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = currentquestion.GetCorrectAnswerIndex();
            string correctAnswer = currentquestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was;\n" + correctAnswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite= correctAnswerSprite;
        }
    }
    void GetNextQuestion()
    {
        if(questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreTracker.IncrementQuestionSeen();
        }
    }
    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentquestion = questions[index];
        if(questions.Contains(currentquestion))
        {
            questions.Remove(currentquestion);
        }

    }

    void DisplayQuestion()
    {
        questionText.text = currentquestion.GetQuestion();


        for(int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentquestion.GetAnswer(i);
        }
    }
    void SetButtonState(bool state) 
        {
        for(int i = 0; i < answerButtons.Length;i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        } 
    }
    void SetDefaultButtonSprites()
    {
        for(int i = 0; i < answerButtons.Length;i++)
        {
            Image buttonImage;
            buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
