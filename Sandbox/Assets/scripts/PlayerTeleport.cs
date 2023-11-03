using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public GameObject Train1;
    public GameObject Train2;

    private GameObject pickedItem;
    private IInteractable interactableObj;
    [SerializeField] private GameObject teleportDisplayObj;


    PlayerController controller;
    enum Trains
    {
        Train1,
        Train2
    }
    private Trains currentTrain = Trains.Train1;
    private Vector3 destination;
    private Interact interact;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<PlayerController>();
        interact = gameObject.GetComponent<Interact>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && controller.disabled == false)
        {
            if (!teleportDisplayObj.GetComponent<TeleportDisplay>().isOnCooldonw())
            {
                float playerHeight = gameObject.GetComponent<CharacterController>().height;
                if (currentTrain == Trains.Train1)
                {
                    Vector3 shift = transform.position - Train1.transform.position;
                    destination = Train2.transform.position + shift;
                    currentTrain = Trains.Train2;
                    Debug.Log("Teleported to Train 2. Vector3: " + shift);
                }
                else
                {
                    Vector3 shift = transform.position - Train2.transform.position;
                    destination = Train1.transform.position + shift;
                    currentTrain = Trains.Train1;
                    Debug.Log("Teleported to Train 1 Vector3: " + shift);
                }

                StartCoroutine(Teleport(destination));
                
                
            }
            else
            {
                Debug.Log("Teleport on cooldown");
            }


        }
    }
    IEnumerator Teleport(Vector3 destination)
    {
        
        controller.disabled = true;
        yield return new WaitForSeconds(.1f);
        gameObject.transform.position = destination;
        if (interact.IsPickedUp())
        {
            interact.getObject().transform.position = GameObject.Find("PickUpPoint").transform.position;
        }
        teleportDisplayObj.GetComponent<TeleportDisplay>().startCooldown();
        yield return new WaitForSeconds(.1f);
        controller.disabled = false;

    }
}
