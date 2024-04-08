using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiomanager : MonoBehaviour
{

    public static audiomanager instance; //global variable, every object can see and send messages to the audio manager

    [SerializeField, Range(1, 20)] private int sfxAmount; //dictates how many sound effects we can have at once
    [SerializeField] private GameObject audioObject;
    private AudioSource[] sfxSources; //stores all the sound effect sources
    private AudioSource leMusic; //stores the background music

    private int sfxIndex = 0; //Store the current sfx index

    private void InitSFX()
    {
        sfxSources = new AudioSource[sfxAmount]; //set the size of the audiosources array
        
        leMusic = gameObject.AddComponent<AudioSource>();//create new audio source, make it the bgm

        for (int i = 0; i < sfxAmount; i++) //for size of array, add a new audio source, and put it into the correct index of the array
        {
            sfxSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

 
    private void Awake() //is called before Start method, at start of the game
    {
        if (audiomanager.instance == null) //does the audiomanager exist?
        {
            audiomanager.instance = this; //if it doesn't exist, make me manager
        }
        else if(audiomanager.instance != this) // if it does exist but not me
        {
            Destroy(this); // game end me
        }

        InitSFX(); // now that the manager is up, initialize all needed audio sources
    }

    public void PlaySFX(AudioClip clipToPlay) 
    {
        sfxSources[sfxIndex].clip = clipToPlay; //tell the current audio source to load x clip
        sfxSources[sfxIndex].Play(); //tell the current audio source to play

        sfxIndex++; //increment to next current clip

        if (sfxIndex >= sfxSources.Length) //check if index goes out of range
        {
            sfxIndex = 0; //reset index if true
        }
    }

    public void PlaySFX3D(AudioClip clipToPlay, Vector3 position, float epicFloat = 1)
    {
        if (audioObject != null) //does an audio object exist?
        { 
            GameObject gaming = Instantiate(audioObject, position, Quaternion.identity); //Quaternion.identity is basically default for Quaternions

            if (gaming.GetComponent<AudioSource>() == null) //if no audio source
            {
                gaming.AddComponent<AudioSource>(); //create an audio source
            }

            AudioSource temp = gaming.GetComponent<AudioSource>();
            temp.clip = clipToPlay;
            temp.spatialBlend = epicFloat;
            temp.Play();

            StartCoroutine(BleanUp(gaming, clipToPlay.length));
        }
    }

    //only exists so a coroutine can be called by another script
    public void PlayBGM(AudioClip musicToPlay, float fadeTime = 5, bool isLooping = true)
    {
        StartCoroutine(PlayBGMPog(musicToPlay, fadeTime, isLooping));
    }


    private IEnumerator PlayBGMPog(AudioClip musicToPlay, float fadeTime = 60, bool isLooping = true)
    {
        AudioSource newBGM = gameObject.AddComponent<AudioSource>(); //make a new Audio sauce
        newBGM.clip = musicToPlay; //Init the new sauce, based on passed in values
        newBGM.volume = 0;
        newBGM.loop = isLooping;
        newBGM.Play();

        float t = 0; //shorthand for time, starting at 0

        while(t < fadeTime)
        {
            //increase t by amount of time passed between frames
            t += Time.deltaTime;
            //calc percent of time that has passed, based on fadeTime
            float perc = t / fadeTime;
            //fade the musics out/in
            leMusic.volume = Mathf.Lerp(1, 0, t / perc);
            newBGM.volume = Mathf.Lerp(0, 1, t / perc);
            //yield the frame, then continue
            yield return null;
        }
        //destroy unneeded audio sauce
        Destroy(leMusic);
        //set new sauce where the old sauce was
        leMusic = newBGM;
    }
    
    private IEnumerator BleanUp(GameObject gaming, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gaming);
    }
}
