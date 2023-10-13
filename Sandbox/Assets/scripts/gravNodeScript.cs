using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravNodeScript : MonoBehaviour   
{

    private GameObject gravNode = null;

    private void OnTriggerEnter(Collider colliding)
    {

        if (colliding.gameObject.CompareTag("movable")) 
        {
            if (gravNode == null)
            {
                gravNode = colliding.gameObject;
                gravNode.GetComponent<PickupScript>().enterNode(gameObject);
            }
           

        }
       
    }
    private void OnTriggerExit(Collider colliding)
    {
        if (colliding.gameObject.CompareTag("movable"))
        {
            gravNode = null;
            colliding.gameObject.GetComponent<PickupScript>().exitClipNode();
        }
    }
    
    public GameObject getGravNode()
    {
        return gravNode;
    }

}
