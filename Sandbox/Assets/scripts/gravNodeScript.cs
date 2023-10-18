using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class gravNodeScript : MonoBehaviour
{

    private GameObject gravNodeHost = null;

    private void OnTriggerEnter(Collider colliding)
    {

        if (colliding.gameObject.CompareTag("movable")) 
        {
            if (gravNodeHost == null)
            {
                gravNodeHost = colliding.gameObject;
                gravNodeHost.GetComponent<PickupScript>().enterNode(gameObject);
            }
           

        }
       
    }
    private void OnTriggerExit(Collider colliding)
    {
        if (colliding.gameObject.CompareTag("movable"))
        {
            if(gravNodeHost.Equals(colliding.gameObject))
            {
                gravNodeHost = null;
            }
            colliding.gameObject.GetComponent<PickupScript>().exitClipNode();
        }
    }
    
    public GameObject getGravNode()
    {
        return gravNodeHost;
    }

     
    public bool checkEquals(Object o)
    {
        if (o != null && gravNodeHost != null)
        {
            Debug.Log("Tag of dim1: "+ gravNodeHost.GetComponent<PickupScript>().ObjectTag +"\n Tag of dim2: "+ o.GetComponent<PickupScript>().ObjectTag);
            if(gravNodeHost.GetComponent<PickupScript>().ObjectTag == o.GetComponent<PickupScript>().ObjectTag)
            {
                return true;
            }
        }
        return false;
    }
}
