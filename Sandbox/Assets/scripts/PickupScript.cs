using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour, IInteractable
{
    private bool pickedUp = false;
    private bool moveToClip = false;
    private GameObject clipToObject;
    private GameObject ClipObject;
    private Rigidbody Rigidbody;
    public void Interact()
    {

        if (pickedUp){

            pickedUp = false;
            Clip("clipPoint");


            Rigidbody.velocity = Vector3.zero;
            Rigidbody.useGravity = true;
            Debug.Log("Put down");
            

        }
        else
        {
            Rigidbody.useGravity = false;
            Debug.Log("Picked up");
            pickedUp = true;
        }
        
    }
   

    // Start is called before the first frame update
    void Start()
    {
        clipToObject = GameObject.Find("PickUpPoint");
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pickedUp)
        {
            Rigidbody.velocity = (clipToObject.transform.position - transform.position) * 10;
        }
        else if (moveToClip)
        {
            Rigidbody.velocity = (ClipObject.transform.position - transform.position) * 10;

            if (Vector3.Distance(transform.position, ClipObject.transform.position) <= 0.2f)
            {
                transform.transform.position = ClipObject.transform.position;
                Debug.Log("Object clipped");
                moveToClip = false;
                ClipObject = null;
            }
        }
        
    }
    public void Clip(string Tag)
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(Tag);
        
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject targetObject in targetObjects)
        {
            // Calculate the distance between this object and the target object
            float distance = Vector3.Distance(transform.position, targetObject.transform.position);

            // Check if the target object is within the detection range
            if (distance <= 1f)
            {
                closestDistance = distance;
                closestObject = targetObject;
                Debug.Log("Object with tag '" + Tag + "' is within range!");
                  
            }
        }
        if (closestObject != null)
        {
            ClipObject = closestObject;
            moveToClip = true;
        }

    }
}
