using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriageControls : MonoBehaviour
{
    public ScrollingBackground scrollingBackground;
    public PlayerController playerController;
    public PauseScreen pauseScreen;

    private bool flagFall;
    private float speed;
    private bool isPaused;

    void Update()
    {
        isPaused = pauseScreen.isPaused;
        flagFall = playerController.flagFall;
        if (flagFall)
        {
            speed = scrollingBackground.speed;
        }
        if (!gameObject.CompareTag("Carriage") && !isPaused)
        {
            transform.Rotate(0, 0, -0.8f - speed);
        }
        if (!flagFall && !isPaused)
        {
            transform.position += new Vector3(speed / 3, 0, 0);
        }
    }
}
