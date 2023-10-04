using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour, IInteractable
{
    private bool pickedUp = false;
    private bool moveToClipNode = false;
    private bool showClip = false;
    private GameObject clipToObject;
    private GameObject clipNode;
    private Rigidbody Rigidbody;
    private Outline outline;
    public void Interact()
    {

        if (pickedUp){

            pickedUp = false;
            if (showClip)
            {
                moveToClipNode = true;
            }


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
    public void hoverIO(GameObject clipNode)
    {
        if (pickedUp)
        {
            this.clipNode = clipNode;
            showClip = !showClip;
            //This will show that your able to Clip the object;
            if (showClip)
            {
                Debug.Log("Show clip");
                outline.OutlineColor = Color.red;
            }
            else
            {
                Debug.Log("Hide clip");
                outline.OutlineColor = Color.white;
            }
            
        }
    }
   

    // Start is called before the first frame update
    void Start()
    {
        clipToObject = GameObject.Find("PickUpPoint");
        Rigidbody = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pickedUp)
        {
            Rigidbody.velocity = (clipToObject.transform.position - transform.position) * 10;
        }
        else if (moveToClipNode)
        {
            Rigidbody.velocity = (clipNode.transform.position - transform.position) * 10;

            if (Vector3.Distance(transform.position, clipNode.transform.position) <= 0.2f)
            {
                transform.transform.position = clipNode.transform.position;
                Debug.Log("Object clipped");
                moveToClipNode = false;
                clipNode = null;
            }
        }
        
    }
}
