using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public GameObject Train1;
    public GameObject Train2;


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
            if (interact.IsPickedUp())
            {
                interact.letgo();
            }

            
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
        yield return new WaitForSeconds(.1f);
        gameObject.transform.position = destination;
        yield return new WaitForSeconds(.1f);
        controller.disabled = false;
        
    }
}
