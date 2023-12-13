using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupV2Script : MonoBehaviour , IInteractable
{
    private bool pickedUp = false;
    private bool rdyToGrab = false;
    private bool rdyToPut = false;
    private bool PutatRay = false;
    private bool PutOnGravNode = false;

    private bool rdyToPutOnGravNode = false;

    private RaycastHit rayCastPutHit;

    private GameObject arm;
    private GameObject itemPos;
    private Rigidbody rb;
    private Vector3 objectHeight; 
    
    private GameObject radarObject;
    private GameObject cam;

    private GameObject gravNode = null;

    private float handLaenge = 1.5f;

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

    void Start()
    {
        itemPos = GameObject.Find("ItemPos");
        cam = GameObject.Find("Main Camera");
        rb = GetComponent<Rigidbody>();
        
        objectHeight = DublicateV2Script.getHeight(this.gameObject);
        radarObject = GameObject.Find("PickUpPoint").transform.GetChild(0).gameObject;


    }

    public void Interact()
    {
        pickedUp = !pickedUp;

    Debug.Log("Pickup - PUTDOWN: " + pickedUp);
        if (!pickedUp) //Put Down
        {


            if(radarObject.GetComponent<gravNodeV2Script>().hasValidGravNode())
            {
                if (radarObject.GetComponent<gravNodeV2Script>().checkColliders())
                {
                   
                    gravNode = radarObject.GetComponent<gravNodeV2Script>().getClosestGravNode();
                    gravNode.GetComponent<DublicateV2Script>().setOccupied(true, this.gameObject);

                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    rdyToPut = true;
                    PutOnGravNode = true;

                    radarObject.GetComponent<gravNodeV2Script>().destroyAllDublicates();

                }
                else
                {
                    Debug.LogError("Colliders not valid");

                    gravNode = radarObject.GetComponent<gravNodeV2Script>().getClosestGravNode();
                    gravNode.GetComponent<DublicateV2Script>().setOccupied(true, this.gameObject);

                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    rdyToPut = true;
                    PutOnGravNode = true;

                    radarObject.GetComponent<gravNodeV2Script>().destroyAllDublicates();
                    //Shake Object
                }   
            }
            else
            {
                Debug.LogWarning("No valid GravNode");

                RaycastHit[] hitarray = Physics.RaycastAll(cam.transform.position, cam.transform.forward.normalized, handLaenge*3);

                Debug.Log("arrayL:"+hitarray.Length);


                List<RaycastHit> hitsList = new List<RaycastHit>();
                RaycastHit closestHit = new RaycastHit();
                float closestHitDistance = 100;
                foreach (RaycastHit hit in hitarray)
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        hitsList.Add(hit);
                    }
                }
                Debug.Log("List Count:" + hitsList.Count);
                //Get closest hit
                if (hitsList.Count == 0)
                {
                    Debug.Log("PutDown - Outof reach");
                   rdyToPut = true;
                }
                else
                {
                    foreach (RaycastHit pHit in hitsList)
                    {
                        if (pHit.distance < closestHitDistance)
                        {
                            closestHit = pHit;
                            closestHitDistance = pHit.distance;
                        }
                    }

                    if (closestHit.distance > handLaenge)
                    {
                        Debug.Log("PutDown - Outof reach but in range Of ray");
                        rdyToPut = true;
                    }
                    else
                    {
                        Debug.Log("PutDown - Closest Hit: " + closestHit.collider.gameObject.name);
                        PutatRay = true;
                        rayCastPutHit = closestHit;
                    }
                }
                
                

                rb.useGravity = true;
                rb.velocity = Vector3.zero;
            }   
            radarObject.GetComponent<gravNodeV2Script>().setItem(null);
        }


        else //Pick Up
        {
            //If on GravNode
            radarObject.GetComponent<gravNodeV2Script>().setItem(this.gameObject);

            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            Debug.Log("ITEMPOS: " + this.gameObject.transform.position);
            rdyToGrab = true;
            
            Debug.Log("ITEMPOS: " + this.gameObject.transform.position);
            if (gravNode != null)
            {
                Debug.Log("Pickup - GravNode not Null: " + gravNode.name);
                //release GravNodeHost
                Debug.Log("Pickup - Release GravNodeHost");
                gravNode.GetComponent<DublicateV2Script>().setOccupied(false, this.gameObject);

                gravNode = null;
            }   

            Debug.Log("Pickup - Init GravNodeList");
            //init GravNodeList
            radarObject.GetComponent<gravNodeV2Script>().initGravNodeList();
            Debug.Log("ITEMPOS: " + this.gameObject.transform.position);
        }
    }
    void FixedUpdate()
    {
        if (rdyToGrab)
        {
            this.gameObject.transform.position = itemPos.transform.position;
            rdyToGrab = false;
        }
        if (rdyToPut)
        {
            rdyToPut = false;
            if (PutatRay)
            {
                Debug.Log("FIxedUpdate - PutAtRay");
                this.gameObject.transform.position = rayCastPutHit.point + objectHeight/2;
                PutatRay = false;
            }
            else if (PutOnGravNode)
            {
               this.gameObject.transform.position = gravNode.transform.position + objectHeight/2;
                PutOnGravNode = false;
            }
            else
            {
                Debug.Log("FIxedUpdate - PutAtRadar");
                this.gameObject.transform.position = radarObject.transform.position;
            }
        }
        if (rdyToPutOnGravNode)
        {
            this.gameObject.transform.position = gravNode.transform.position + objectHeight/2;
            rdyToPutOnGravNode = false;
        }
    }
    public void setGravnode(GameObject gravNode)
    {
        this.gravNode = gravNode;
    }

    public bool getPickedup()
    {
        return pickedUp ? true : false;
        
    }

    public bool hasGravNode()
    {
        if (gravNode != null)
        {
            return true;
        }
        return false;
    }
}


