using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private Renderer bgRenderer;
    public PlayerController playerController;
    
    public float speed = 0.2f;
    public float increase = 0.002f;
    private bool flagFall;

    void Update()
    {
        flagFall = playerController.flagFall;
        bgRenderer.material.mainTextureOffset += new Vector2(Time.deltaTime * speed, 0);
        if (flagFall) {
            speed += increase * Time.deltaTime;
        }
        else 
        {
            speed = 0.02f;
        }
    }
}
