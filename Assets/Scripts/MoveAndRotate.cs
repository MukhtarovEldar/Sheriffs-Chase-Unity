using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MoveAndRotate : MonoBehaviour
{
    public ScrollingBackground scrollingBackground;
    public PlayerController playerController;
    public PauseScreen pauseScreen;

    private float speed;
    private bool flagFall;
    private bool isPaused;

    void Update()
    {
        isPaused = pauseScreen.isPaused;
        flagFall = playerController.flagFall;
        if (flagFall)
        {
            speed = scrollingBackground.speed;
        }
        if (!isPaused)
        {
            transform.Rotate(0, 0, 1f + speed);
        }
        transform.position += Vector3.left * speed * 33 * Time.deltaTime;
    }
}
