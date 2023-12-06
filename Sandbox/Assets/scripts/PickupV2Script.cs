using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PickupV2Script : MonoBehaviour , IInteractable
{
    private bool pickedUp = false;

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
        objectHeight = new Vector3(0, transform.lossyScale.y * 0.5f, 0);
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
                    radarObject.GetComponent<gravNodeV2Script>().destroyAllDublicates();

                }
                else
                {
                    Debug.LogError("Colliders not valid");
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
                    transform.position = radarObject.transform.position;
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
                        transform.position = radarObject.transform.position;
                    }
                    else
                    {
                        Debug.Log("PutDown - Closest Hit: " + closestHit.collider.gameObject.name);
                        transform.position = closestHit.point + objectHeight / 3;
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
            this.gameObject.transform.position = itemPos.transform.position;
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

    public bool getPickedup()
    {
        return pickedUp ? true : false;
    }
}


