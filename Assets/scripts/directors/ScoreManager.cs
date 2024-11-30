using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int currentScore;

    public void AddScore(int score)
    {
        this.currentScore += score;
        UpdateScoreUI();
    }

    private void Start()
    {
        this.currentScore = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        GetComponent<Text>().text = currentScore.ToString();
    }
}
