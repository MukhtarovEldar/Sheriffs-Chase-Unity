using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;  
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class DirectButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText;
    public PauseScreen pauseScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.CompareTag("Resume"))
            {
                pauseScreen.Resume();
            }
            else if (gameObject.CompareTag("Start"))
            {
                SceneManager.LoadScene("Story");
            }
            else if (gameObject.CompareTag("Try again"))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = new Color32(253, 236, 193, 255);

        Outline outline = buttonText.GetComponent<Outline>();
        if (outline == null)
        {
            outline = buttonText.gameObject.AddComponent<Outline>();
        }

        outline.effectColor = new Color32(40, 20, 22, 255);
        outline.effectDistance = new Vector2(1, 5);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = new Color32(40, 20, 22, 255);

        Outline outline = buttonText.GetComponent<Outline>();
        if (outline != null)
        {
            Destroy(outline);
        }
    }

    public void OnButtonClick()
    {
        if (gameObject.CompareTag("Start"))
        {
            SceneManager.LoadScene("Story");
        }
        else if (gameObject.CompareTag("Quit"))
        {
            Application.Quit();
        }
        else if (gameObject.CompareTag("Contribute"))
        {
            Application.OpenURL("https://github.com/MukhtarovEldar/Sheriffs-Chase-Game");
        }
        else if (gameObject.CompareTag("Resume"))
        {
            pauseScreen.Resume();
        }
        else if (gameObject.CompareTag("Try again"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
