using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveCurrentScene : MonoBehaviour
{
    private void Start()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Get the name of the current scene
        string currentSceneName = currentScene.name;

        // Save the current scene name to playerprefs
        PlayerPrefs.SetString("CurrentScene", currentSceneName);

        PlayerPrefs.Save();
    }
}
