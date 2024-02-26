using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CalculateBestScore : MonoBehaviour
{
    public Text bestText;
    public Text currText;
    private static float score;
    private static float best = 0f;

    void Update()
    {
        best = Mathf.Max(best, ScoringSystem.score);

        bestText.text = "" + Mathf.Round(best);
        currText.text = "" + Mathf.Round(ScoringSystem.score);
    }
}
