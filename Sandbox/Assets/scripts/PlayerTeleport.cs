using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerTeleport : MonoBehaviour
{
    public GameObject Train1;
    public GameObject Train2;

    private GameObject pickedItem;
    private IInteractable interactableObj;
    private Vector3 startPosition;


    PlayerController controller;
    enum Trains
    {
        Train1,
        Train2
    }
    private Trains currentTrain = Trains.Train1;
    private Vector3 destination;
    private Interact interact;
    
    public float transitionDuration = 0.5f;

    //PostProcessing Stuff
    private PostProcessVolume postProcessVolume;
    private Bloom bloom;
    private LensDistortion lensDistortion;
    private DepthOfField depthOfField;
    private AutoExposure autoExposure;

    [Range(0f, 100f)]
    public float teleportationBloomIntensity = 5f;

    [Range(-200f, 200f)]
    public float teleportationLensIntensity = 0f;

    [Range(-5f, 5f)]
    public float teleportationAperture = 5f;

    [Range(-5f, 0f)]
    public float teleportationExposureminLuminance = 1f;



    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
        controller = gameObject.GetComponent<PlayerController>();
        interact = gameObject.GetComponent<Interact>();

        // Get the post-processing volume and Bloom effect
        postProcessVolume = FindObjectOfType<PostProcessVolume>();

        if (postProcessVolume != null && postProcessVolume.isActiveAndEnabled)
        {
            // Get the Bloom effect from the Post-Processing Profile
            postProcessVolume.profile.TryGetSettings(out bloom);
            postProcessVolume.profile.TryGetSettings(out lensDistortion);
            postProcessVolume.profile.TryGetSettings(out depthOfField);
            postProcessVolume.profile.TryGetSettings(out autoExposure);
        }
        else
        {
            Debug.LogWarning("No active Post-Processing Volume found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && controller.disabled == false)
        {
            
            GetComponent<SoundEffect>().PlayRewindSound();
            
            float playerHeight = gameObject.GetComponent<CharacterController>().height;
            if (currentTrain == Trains.Train1)
            {
                Vector3 shift = transform.position - Train1.transform.position;
                destination = Train2.transform.position + shift;
                currentTrain = Trains.Train2;
                Debug.Log("Teleported to Train 2");
            }
            else
            {
                Vector3 shift = transform.position - Train2.transform.position;
                destination = Train1.transform.position + shift ;
                currentTrain = Trains.Train1;
                Debug.Log("Telandeported to Train 1");
            }
            StartCoroutine(Teleport(destination));
        }

        if (Input.GetKeyDown(KeyCode.O) && controller.disabled == false)
        {
            GetComponent<SoundEffect>().PlayRewindSound();
            
            StartCoroutine(resetPosition());
        }
    }
    IEnumerator resetPosition()
    {
        yield return new WaitForSeconds(.1f);
        gameObject.transform.position = startPosition;
        yield return new WaitForSeconds(.1f);
    }
    IEnumerator Teleport(Vector3 destination)
    {

        controller.disabled = true;

        // Get the initial bloom intensity
        float initialBloomIntensity = bloom.intensity.value;
        float initialLensIntensity = lensDistortion.intensity.value;
        float initialAperture = depthOfField.aperture.value;
        float initialExposureminLuminance = autoExposure.minLuminance.value;
        float elapsedTime = 0f;

        // Smoothly change the FOV and Bloom from normal to teleportationFOV
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            // Interpolate between normal Effects and teleportationEffects using Lerp
            bloom.intensity.value = Mathf.Lerp(initialBloomIntensity, teleportationBloomIntensity, t);
            lensDistortion.intensity.value = Mathf.Lerp(initialLensIntensity, teleportationLensIntensity, t);
            depthOfField.aperture.value = Mathf.Lerp(initialAperture, teleportationAperture, t);
            autoExposure.minLuminance.value = Mathf.Lerp(initialExposureminLuminance, teleportationExposureminLuminance, t);

            yield return null;
        }

        // Ensure the final Effects are the teleportationEffects
        bloom.intensity.value = teleportationBloomIntensity;
        lensDistortion.intensity.value = teleportationLensIntensity;
        depthOfField.aperture.value = teleportationAperture;
        autoExposure.minLuminance.value = teleportationExposureminLuminance;
 
        yield return new WaitForSeconds(.1f);
        gameObject.transform.position = destination;
        yield return new WaitForSeconds(.1f);

       // Smoothly change the Effects back to normal after teleportation
        elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            // Interpolate between teleportationEffects and normalEffects using Lerp
            bloom.intensity.value = Mathf.Lerp(teleportationBloomIntensity, initialBloomIntensity, t);
            lensDistortion.intensity.value = Mathf.Lerp(teleportationLensIntensity, initialLensIntensity, t);
            depthOfField.aperture.value = Mathf.Lerp(teleportationAperture, initialAperture, t);
            autoExposure.minLuminance.value = Mathf.Lerp(teleportationExposureminLuminance, initialExposureminLuminance, t);

            yield return null;
        }

        // Ensure the final Effects is the normalEffects
        bloom.intensity.value = initialBloomIntensity;
        lensDistortion.intensity.value = initialLensIntensity;
        depthOfField.aperture.value = initialAperture;
        autoExposure.minLuminance.value = initialExposureminLuminance;
 

        controller.disabled = false;
        
    }
}
