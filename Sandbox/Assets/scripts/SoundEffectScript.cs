using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    //Field for Music
    [Header("Background Music")]
    public AudioClip dimension1Music;
    public AudioClip dimension2Music;

    [Header("Music Volume")]
    [Range(0.0f, 1.0f)]
    public float dimension1Volume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float dimension2Volume = 1.0f;

    //Field for TP Sound Effect
    [Header("Rewind")]
    public AudioClip rewindSound;
    public void PlayRewindSound()
    {
        audioSource.PlayOneShot(rewindSound);
    }

    //Field for Footsteps
    [Header("Footsteps")]
    public List<AudioClip> StepsL;
    public List<AudioClip> StepsR;


   // Field for Place Glass
    [Header("Place Glass")]
    public List<AudioClip> glassPlace;
    
    // Field for Place Chair
    [Header("Place Chair")]
    public List<AudioClip> chairPlace;
    
    // Field for Place Candle
    [Header("Place Candle")]
    public List<AudioClip> candlePlace;

    // Field for Place Knife
    [Header("Place Knife")]
    public List<AudioClip> knifePlace;

    // Field for Place Meat
    [Header("PlaceMeat")]
    public List<AudioClip> meatPlace;

    private AudioSource backgroundMusicSource;
    private AudioSource audioSource;

    private bool isWalking;
    private bool isInDimension1 = true;

    private Dictionary<AudioClip, float> musicPlaybackPositions = new Dictionary<AudioClip, float>();

    void Start()
    {
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        audioSource = gameObject.AddComponent<AudioSource>();

        SwitchDimension(isInDimension1); // Start with the default dimension music

        // Set up background music loop
        backgroundMusicSource.loop = true;
        backgroundMusicSource.clip = dimension1Music;
        backgroundMusicSource.volume = dimension1Volume;
        backgroundMusicSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isInDimension1 = !isInDimension1;
            SwitchDimension(isInDimension1);
        }

        // Check for key presses to simulate walking
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (!isWalking)
            {
                isWalking = true;
                StartCoroutine(PlayFootsteps());
            }
        }
    }

    void SwitchDimension(bool dimension)
    {
        AudioClip currentMusic = backgroundMusicSource.clip;
        float playbackPosition = backgroundMusicSource.time;

        if (currentMusic != null)
        {
            if (!musicPlaybackPositions.ContainsKey(currentMusic))
            {
                musicPlaybackPositions.Add(currentMusic, playbackPosition);
            }
            else
            {
                musicPlaybackPositions[currentMusic] = playbackPosition;
            }
        }

        if (dimension)
        {
            backgroundMusicSource.volume = dimension1Volume;
            if (musicPlaybackPositions.ContainsKey(dimension1Music))
            {
                backgroundMusicSource.Stop(); // Stop previous dimension music
                backgroundMusicSource.clip = dimension1Music;
                backgroundMusicSource.time = musicPlaybackPositions[dimension1Music];
                backgroundMusicSource.Play();
            }
            else
            {
                PlayNewDimensionMusic(dimension1Music);
            }
        }
        else
        {
            backgroundMusicSource.volume = dimension2Volume;
            if (musicPlaybackPositions.ContainsKey(dimension2Music))
            {
                backgroundMusicSource.Stop(); // Stop previous dimension music
                backgroundMusicSource.clip = dimension2Music;
                backgroundMusicSource.time = musicPlaybackPositions[dimension2Music];
                backgroundMusicSource.Play();
            }
            else
            {
                PlayNewDimensionMusic(dimension2Music);
            }
        }
    }
    void PlayNewDimensionMusic(AudioClip musicClip)
    {
        backgroundMusicSource.Stop(); // Stop previous dimension music
        backgroundMusicSource.clip = musicClip;
        backgroundMusicSource.Play();
    }


    IEnumerator PlayFootsteps()
    {
        isWalking = true;

        while (isWalking)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                audioSource.clip = StepsL[Random.Range(0, StepsL.Count)];
                audioSource.Play();

                yield return new WaitForSeconds(0.5f);

                audioSource.clip = StepsR[Random.Range(0, StepsR.Count)];
                audioSource.Play();

                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                isWalking = false;
            }
        }
    }
}

