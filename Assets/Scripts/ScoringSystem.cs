using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public Text scoreText;
    public PlayerController playerController;
    public ScrollingBackground scrollingBackground;
    
    public static float score;
    private float speed;
    private bool flagFall;
    private float timeElapsed;

    void Start()
    {
        score = 0f;
        timeElapsed = 0f;
    }

    void Update()
    {
        speed = scrollingBackground.speed;
        flagFall = playerController.flagFall;
        if (flagFall)
        {
            timeElapsed += Time.deltaTime * speed * 20;
            score = timeElapsed;
        }
        scoreText.text = "Score: " + Mathf.Round(score);
    }
}
