using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider colliding)
    {

        if (colliding.gameObject.layer == LayerMask.NameToLayer("Pickup")) 
        {
            Debug.Log("Cube entered clipNode");
            // other.gameObject.GetComponent<Interact>().hoverNode(gameObject);
            colliding.gameObject.GetComponent<PickupScript>().hoverIO(gameObject);

        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickup"))
        {
            Debug.Log("Cube exited clipNode");
            // other.gameObject.GetComponent<Interact>().hoverNode(gameObject);
            other.gameObject.GetComponent<PickupScript>().exitClipNode();
        }
    }
}
