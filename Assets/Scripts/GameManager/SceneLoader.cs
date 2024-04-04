using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void Respawn()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
