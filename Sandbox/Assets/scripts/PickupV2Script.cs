using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PickupV2Script : MonoBehaviour , IInteractable
{
    private bool pickedUp = false;
    private bool moveToClipNode = false;

    private GameObject clipToObject;
    private GameObject itemPos;
    private Rigidbody rb;
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

    void Start()
    {
        itemPos = GameObject.Find("ItemPos");
        player = GameObject.Find("Player");
        clipToObject = GameObject.Find("PickUpPoint");
        rb = GetComponent<Rigidbody>();
        objectHeight = new Vector3(0, transform.lossyScale.y * 0.5f, 0);
        radarObject = transform.GetChild(0).gameObject;

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
            transform.position = clipToObject.transform.position;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
        }
        else //Pick Up
        {
            rb.useGravity = false;
            transform.position = itemPos.transform.position;
        }
    }

}
