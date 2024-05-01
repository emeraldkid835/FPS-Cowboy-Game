using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VideoPlayer))]
public class Intro_Script : MonoBehaviour
{
    private VideoPlayer me;
    [SerializeField] int levelToLoad;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        me = this.GetComponent<VideoPlayer>();
        me.url = Application.streamingAssetsPath + "/Video/background1.mp4";
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if((me.isPlaying == false && t > 1)|| Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(levelToLoad);
        }
        if (t <= 1)
        {
            t += Time.deltaTime;
        }
    }
}
