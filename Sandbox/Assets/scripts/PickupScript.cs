using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour, IInteractable
{
    private bool pickedUp = false;
    private bool moveToClipNode = false;
    private GameObject clipToObject;
    private Rigidbody Rigidbody;
    private Outline outline;
    private Vector3 objectHeight;

    private GameObject radarObject;
    private GameObject player;



    public enum Tag
    {
        BottleSmall,
        BottleMedium,
        BottleLarge,
        GlassSmall,
        GlassMedium,
        GlassLarge,
        KnifeSmall,
        KnifeLarge,
        Chair,
        ArmChair,
        Picture1,
        Picture2,
        Vase,
        CandleholderSmall,
        CandleholderMedium,
        CandleholderLarge,
        Cutlery,
        Pan
    }

    [SerializeField] private Tag objectTag;

    public Tag ObjectTag
    {
        get { return objectTag; }
        set
        {
            objectTag = value;
        }
    }

    public void Interact()
    {
        pickedUp = !pickedUp;
        // Release Freezed Position;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        if (!pickedUp) //Put Down
        {


            if (radarObject.GetComponent<gravNodeScript>().hasValidGravNode())
            {
                //We have to check if the Moveable would be in the way of another the gravNode witch is occupied
                
                if (radarObject.GetComponent<gravNodeScript>().checkColliders())
                {

                    moveToClipNode = true;

                    radarObject.GetComponent<gravNodeScript>().setgravNodeHost();
                    radarObject.GetComponent<gravNodeScript>().removeAllDublicates();

                }
                else
                {
                    
                    radarObject.GetComponent<gravNodeScript>().shakeDublicate();
                    Debug.LogWarningFormat("Can't put down {0} because it would block another gravNode", gameObject.name);
                    radarObject.GetComponent<gravNodeScript>().clearList();
                    pickedUp = true;
                    player.GetComponent<Interact>().setPickedUp(true);
                    return;
                }


            }


            Rigidbody.velocity = Vector3.zero;
            Rigidbody.useGravity = true;
            Debug.Log("Put down");


        }
        else //Pick up
        {


            if (radarObject.GetComponent<gravNodeScript>().hasValidGravNode())
            {
                radarObject.GetComponent<gravNodeScript>().releaseGravNodeHost();
                radarObject.GetComponent<gravNodeScript>().initGravNodeList();
            }
            Rigidbody.useGravity = false;
            Debug.Log("Picked up");
        }

    }
   

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        clipToObject = GameObject.Find("PickUpPoint");
        Rigidbody = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        objectHeight = new Vector3(0, transform.lossyScale.y * 0.5f, 0);
        radarObject = gameObject.transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pickedUp)
        {
            //Move to playerClip
            Rigidbody.velocity = (clipToObject.transform.position - transform.position) * 10;
        }
        else if (moveToClipNode)
        {
            //Move to gravNode
            Vector3 clipPosition = radarObject.GetComponent<gravNodeScript>().getGravNodeHost().transform.position + objectHeight;
            Rigidbody.velocity = (clipPosition - transform.position) * 10;
            Rigidbody.useGravity = false;
            if (Vector3.Distance(transform.position, clipPosition) <= 0.1f)
            {
                //Rigidbody.velocity = Vector3.zero;
                Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                transform.transform.position = clipPosition;
                Debug.Log("Object clipped");
                moveToClipNode = false;
            }
        }

    }
    public bool getPickedup()
    {
        return pickedUp?true:false;
    }
    public void setMoveToClipNode(bool b)
    {
        moveToClipNode = b;
    }
}
