using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour, IInteractable
{
    private bool pickedUp = false;
    private bool moveToClipNode = false;
    private bool enteredClipNode = false;
    private bool clipped = false;
    private GameObject clipToObject;
    private GameObject clipNode;
    private Rigidbody Rigidbody;
    private Outline outline;
    private GameObject dublicate;
    private Vector3 objectHeight;
    public void Interact()
    {
        pickedUp = !pickedUp;
        // Relese Freezed Position;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        if (!pickedUp){

            if (enteredClipNode)
            {
                moveToClipNode = true;

                dublicate.GetComponent<MeshRenderer>().enabled = false;
            }


            Rigidbody.velocity = Vector3.zero;
            Rigidbody.useGravity = true;
            Debug.Log("Put down");
            

        }
        else
        {
            if (enteredClipNode)
            {
                dublicate.GetComponent<MeshRenderer>().enabled = true;
            }

            Rigidbody.useGravity = false;
            Debug.Log("Picked up");
        }
        
    }
    public void hoverIO(GameObject clipNode)
    {
        if (pickedUp)
        {

            this.clipNode = clipNode;
            //This will show that your able to Clip the object;
            enteredClipNode = true;
           
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Debug.Log("Init clip");
                dublicate = new GameObject("Silhouette of: " + gameObject.name);
                dublicate.transform.localScale = transform.localScale;
                dublicate.transform.position = clipNode.transform.position + objectHeight;
                MeshFilter newMeshFilter = dublicate.AddComponent<MeshFilter>();
                newMeshFilter.sharedMesh = meshFilter.sharedMesh;
                MeshRenderer newMeshRenderer = dublicate.AddComponent<MeshRenderer>();

                Outline outline = dublicate.AddComponent<Outline>();
                outline.OutlineColor = new Color(0, 0, 0, 0.5f);
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineWidth = 10;
            }
  
            
                
            
        }
    }
    public void exitClipNode()
    {
        enteredClipNode = false;
        Debug.Log("_________________ClipNode NULL_____________________________");
        clipNode = null;
        Destroy(dublicate);

    }

    // Start is called before the first frame update
    void Start()
    {
        clipToObject = GameObject.Find("PickUpPoint");
        Rigidbody = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        objectHeight = new Vector3(0, transform.lossyScale.y * 0.5f, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pickedUp)
        {
            //Move to playerClip
            Rigidbody.velocity = (clipToObject.transform.position - transform.position) * 10;
        }
        else if(moveToClipNode)
        {
            //Move to clipNode
            Vector3 clipPosition = clipNode.transform.position + objectHeight;
            Rigidbody.velocity = (clipPosition - transform.position) * 10;
            Rigidbody.useGravity = false;
            if (Vector3.Distance(transform.position, clipPosition) <= 0.1f)
            {
                //Rigidbody.velocity = Vector3.zero;
                Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                transform.transform.position = clipPosition;
                Debug.Log("Object clipped");
                clipped = true;
                moveToClipNode = false;
            }
        }
        
    }
}
