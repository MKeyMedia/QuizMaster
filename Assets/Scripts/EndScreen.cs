using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    ScoreTracker scoreTracker;
    void Awake()
    {
      scoreTracker = FindObjectOfType<ScoreTracker>();  
    }
    public void ShowFinalScore()
    {
        finalScoreText.text = "You got a score of " + scoreTracker.CalculateScore() + "%";
    }

}



