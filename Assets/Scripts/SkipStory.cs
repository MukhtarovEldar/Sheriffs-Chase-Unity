using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipStory : MonoBehaviour
{
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 20f || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
