using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPause : MonoBehaviour
{

    public bool isPaused = false;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject controlsPanel;

 
    void Start()
    {
        Time.timeScale = 1f;
        
        Cursor.lockState = CursorLockMode.Locked;
        if (pausePanel != null && controlsPanel != null)
        {
            pausePanel.SetActive(false);
            controlsPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void OnPausePressed() //check added to do nothing if no panels are present, should not need any other changes. for now.
    {
        if (pausePanel != null && controlsPanel != null)
        {
            if (isPaused == false)
            {
                isPaused = true;
                pausePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
            else
            {
                if (DialogKnower.instance.gameObject.GetComponent<Canvas>().enabled == true)
                {
                    DialogKnower.instance.ExitDialog();
                }
                isPaused = false;
                pausePanel.SetActive(false);
                controlsPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
        }
        else
        {
            Debug.Log("I'm missing panels, no pause for you!");
        }

    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void PanicPause()
    {
        isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ControlsButton()
    {
        if (!controlsPanel.activeSelf)
        {
            pausePanel.SetActive(false);
            controlsPanel.SetActive(true);
        }
        
    }

    public void BackButton()
    {
        if (!controlsPanel.activeSelf)
        {
            pausePanel.SetActive(true);
            controlsPanel.SetActive(false);
        }
    }


}
