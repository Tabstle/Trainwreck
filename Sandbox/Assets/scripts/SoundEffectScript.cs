using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
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
    

    private AudioSource audioSource;
    private bool isWalking;


    // Start is called before the first frame update
   void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
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

    IEnumerator PlayFootsteps()
    {
        while (isWalking)
        {
            // Play a random sound from StepsL list
            audioSource.clip = StepsL[Random.Range(0, StepsL.Count)];
            audioSource.Play();

            yield return new WaitForSeconds(0.5f);

            // Play a random sound from StepsR list
            audioSource.clip = StepsR[Random.Range(0, StepsR.Count)];
            audioSource.Play();

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Stop walking sound when keys are released
    void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) ||
            Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            isWalking = false;
        }
    }
}

