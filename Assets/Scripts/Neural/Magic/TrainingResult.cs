using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainingResult : MonoBehaviour
{
    [SerializeField]
    TMP_Text scoreText;
    public void DisplayScore(double score)
    {
        scoreText.text = "Training Average Score \n\n" + score;
    }
}
