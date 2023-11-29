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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        pickedUp = !pickedUp;

        if (!pickedUp) //Put Down
        {


            if(radarObject.GetComponent<gravNodeV2Script>().hasValidGravNode())
            {
                if (radarObject.GetComponent<gravNodeV2Script>().checkColliders())
                {
                   
                    gravNode = radarObject.GetComponent<gravNodeV2Script>().getClosestGravNode();
                    gravNode.GetComponent<DublicateV2Script>().setOccupied(true, this.gameObject);

                }else
                {
                    //Shake Object
                }   
            }
            else
            {
                Debug.LogWarning("No valid GravNode");
                int layerMask = ~LayerMask.GetMask("Player");

                Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 1.5f, layerMask);
                Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.red, 1f);
                Debug.Log("HIT POINT: " + hit.point);
                if (hit.collider != null)
                {
                    transform.position = hit.point;
                }
                else
                {
                    Debug.Log("No hit");
                    transform.position = cam.transform.position + cam.transform.forward * handLaenge;
                }

                rb.useGravity = true;
                rb.velocity = Vector3.zero;
            }   
            radarObject.GetComponent<gravNodeV2Script>().setItem(null);
        }
        else //Pick Up
        {
            radarObject.GetComponent<gravNodeV2Script>().setItem(this.gameObject);
            //Logic pickup
            if(gravNode != null)
            {
                //release GravNodeHost
                gravNode.GetComponent<DublicateV2Script>().setOccupied(false, null);
                gravNode = null;
                //init GravNodeList
                radarObject.GetComponent<gravNodeV2Script>().initGravNodeList();
            }



            // Visual pickup
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            transform.position = itemPos.transform.position;

        }
    }

    public bool getPickedup()
    {
        return pickedUp ? true : false;
    }
}


