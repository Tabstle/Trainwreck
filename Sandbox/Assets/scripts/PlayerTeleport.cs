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


    PlayerController controller;
    enum Trains
    {
        Train1,
        Train2
    }
    private Trains currentTrain = Trains.Train1;
    private Vector3 destination;
    private Interact interact;

    // FOV variables
    private float normalFOV;
    public float teleportationFOV;
    public float transitionDuration = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<PlayerController>();
        interact = gameObject.GetComponent<Interact>();

        // Store the initial normal FOV
        normalFOV = Camera.main.fieldOfView;
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
                Debug.Log("Teleported to Train 1");
            }
            StartCoroutine(Teleport(destination));
        }
    }
    IEnumerator Teleport(Vector3 destination)
    {

        controller.disabled = true;

        float elapsedTime = 0f;

        // Smoothly change the FOV from normal to teleportationFOV
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            // Interpolate between normalFOV and teleportationFOV using Lerp
            Camera.main.fieldOfView = Mathf.Lerp(normalFOV, teleportationFOV, t);

            yield return null;
        }

        // Ensure the final FOV is the teleportationFOV
        Camera.main.fieldOfView = teleportationFOV;
 
        yield return new WaitForSeconds(.1f);
        gameObject.transform.position = destination;
        yield return new WaitForSeconds(.1f);

       // Smoothly change the FOV and bloom back to normal after teleportation
        elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            // Interpolate between teleportationFOV and normalFOV using Lerp
            Camera.main.fieldOfView = Mathf.Lerp(teleportationFOV, normalFOV, t);

            yield return null;
        }

        // Ensure the final FOV is the normalFOV
        Camera.main.fieldOfView = normalFOV;

        controller.disabled = false;
        
    }
}
