using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audiomanager : MonoBehaviour
{
    private bool valid;
    public static audiomanager instance; //global variable, every object can see and send messages to the audio manager

    [SerializeField, Range(1, 20)] private int sfxAmount = 20; //dictates how many sound effects we can have at once
    [SerializeField] private GameObject audioObject;

    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;

    [SerializeField, Range(-3, 1)] float minVariation;
    [SerializeField, Range(1, 3)] float maxVariation;

    private bool canOverideMusicVolume;

    private AudioSource[] sfxSources; //stores all the sound effect sources
    private AudioSource leMusic; //stores the background music
    public List<GameObject> worldSFX = new List<GameObject>();

    private int sfxIndex = 0; //Store the current sfx index

    private void InitSFX()
    {
        valid = true;
        sfxSources = new AudioSource[sfxAmount]; //set the size of the audiosources array
        sfxIndex = 0;
        leMusic = gameObject.AddComponent<AudioSource>();//create new audio source, make it the bgm

        for (int i = 0; i < sfxAmount; i++) //for size of array, add a new audio source, and put it into the correct index of the array
        {
            sfxSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }
    public void SetSoundValid(bool valide)
    {
        valid = valide;
    }
    private void VolumeTick()
    {
        if (canOverideMusicVolume == true && leMusic.volume != musicVolume.value && musicVolume != null)
        {
            leMusic.volume = musicVolume.value;
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
        InvokeRepeating("VolumeTick", 0.2f, 0.2f);
        InitSFX(); // now that the manager is up, initialize all needed audio sources
    }

    public void PlaySFX(AudioClip clipToPlay) 
    {
        if (valid == true)
        {
            sfxSources[sfxIndex].clip = clipToPlay; //tell the current audio source to load x clip

            sfxSources[sfxIndex].pitch = Random.Range(minVariation, maxVariation);

            sfxSources[sfxIndex].Play(); //tell the current audio source to play

            sfxIndex++; //increment to next current clip

            if (sfxIndex >= sfxSources.Length) //check if index goes out of range
            {
                sfxIndex = 0; //reset index if true
            }
        }
    }

    public void PlaySFX3D(AudioClip clipToPlay, Vector3 position, float epicFloat = 1, float minPitch = 1, float maxPitch = 1)
    {
        if (audioObject != null && valid == true) //does an audio object exist?
        { 
            GameObject gaming = Instantiate(audioObject, position, Quaternion.identity); //Quaternion.identity is basically default for Quaternions

            if (gaming.GetComponent<AudioSource>() == null) //if no audio source
            {
                gaming.AddComponent<AudioSource>(); //create an audio source
            }

            AudioSource temp = gaming.GetComponent<AudioSource>();
            if (sfxVolume != null)
            {
                temp.volume = sfxVolume.value;
            }
            temp.clip = clipToPlay;
            temp.spatialBlend = epicFloat;
            temp.pitch = Random.Range(minPitch, maxPitch);
            temp.Play();
            worldSFX.Add(gaming);
            if (clipToPlay != null)
            {
                StartCoroutine(BleanUp(gaming, clipToPlay.length));
            }
        }
    }

    //only exists so a coroutine can be called by another script
    public void PlayBGM(AudioClip musicToPlay, float fadeTime = 5, bool isLooping = true, float volume = 1)
    {
        StartCoroutine(PlayBGMPog(musicToPlay, fadeTime, isLooping, volume));
    }


    private IEnumerator PlayBGMPog(AudioClip musicToPlay, float fadeTime = 60, bool isLooping = true, float volume = 1)
    {
        AudioSource newBGM = gameObject.AddComponent<AudioSource>(); //make a new Audio sauce
        newBGM.clip = musicToPlay; //Init the new sauce, based on passed in values
        newBGM.volume = 0;
        newBGM.loop = isLooping;
        newBGM.Play();
        float oldMax = leMusic.volume;
        float t = 0; //shorthand for time, starting at 0

        while(t < fadeTime)
        {
            canOverideMusicVolume = false;
            //increase t by amount of time passed between frames
            t += Time.deltaTime;
            //calc percent of time that has passed, based on fadeTime
            float perc = t / fadeTime;
            //fade the musics out/in
            leMusic.volume = Mathf.Lerp(oldMax, 0, t / perc);
            if (musicVolume != null)
            {
                newBGM.volume = Mathf.Lerp(0, musicVolume.value, t / perc);
            }
            else { newBGM.volume = Mathf.Lerp(0, oldMax, t / perc); }
            //yield the frame, then continue
            yield return null;
        }
        //destroy unneeded audio sauce
        canOverideMusicVolume = true;
        Destroy(leMusic);
        //set new sauce where the old sauce was
        leMusic = newBGM;
    }
    
    private IEnumerator BleanUp(GameObject gaming, float duration)
    {
        yield return new WaitForSeconds(duration);
        worldSFX.Remove(gaming);
        Destroy(gaming);
    }
}
