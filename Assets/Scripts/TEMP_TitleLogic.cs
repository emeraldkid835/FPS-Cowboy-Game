using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEMP_TitleLogic : MonoBehaviour
{
    private Animator meAnimator;
    [SerializeField, Range(0f,1f)] private float BGMVolume = 1;
    [SerializeField] private AudioSource titleMusic;
    // Start is called before the first frame update
    void Start()
    {
        meAnimator = this.GetComponent<Animator>();
        
        if (meAnimator == null)
        {
            Debug.Log("NO ANIMATOR FOR cANVAS!");
        }
        else { Debug.Log("animator on canvas :)"); meAnimator.SetBool("showcreds", false); }

        
        audiomanager.instance.PlayBGM(titleMusic.clip); //only a temp measure while we have shitty scene loading

    }

    
    public void LoadAScene(int index)
    {
        if (SceneManager.GetSceneByBuildIndex(index) != null)
        {
            Debug.Log("Should be loading scene at index of: " + index);
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.Log("No scene at that index!");
        }
    }

    public void Quit()
    {
        Debug.Log("Quitting application!");
        Application.Quit();
    }

    public void Opencredits()
    {
        Debug.Log("Should be opening credits");
        if (meAnimator != null)
        {
            meAnimator.SetBool("showcreds", true);
        }
    }

    public void Closecredits()
    {
        Debug.Log("Should be closing credits");
        if(meAnimator != null)
        {
            meAnimator.SetBool("showcreds", false);
        }
    }
}
