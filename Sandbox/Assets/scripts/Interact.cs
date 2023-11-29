using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
    // public void Interact(GameObject gameObject);
}


public class Interact : MonoBehaviour
{
    [SerializeField] private LayerMask InteractMask;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] float InteractionDistance = 2f;
    private bool pickedUp = false;
    private IInteractable interactObj;
    private GameObject hitObject;
    private GameObject radar;

    // Start is called before the first frame update
    void Start()
    {
        radar = GameObject.Find("PickUpPoint").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            
            if (pickedUp)
            {
                if (radar.GetComponent<gravNodeV2Script>().checkColliders())
                {
                    interactObj.Interact();
                    pickedUp = !pickedUp;
                    interactObj = null;
                    hitObject = null;
                    return;
                }
                else
                {
                    interactObj.Interact();
                    return;
                }
                
            }
            Ray camRay = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Debug.DrawRay(camRay.origin, camRay.direction * InteractionDistance, Color.red, 1f);
            if (Physics.Raycast(camRay, out RaycastHit HitInfo, InteractionDistance, InteractMask))
            {
                Debug.Log("Hit something");
                Debug.Log(HitInfo.collider.gameObject.name);
                

                if(HitInfo.collider.gameObject.TryGetComponent(out interactObj))
                {
                    if (HitInfo.collider.gameObject.CompareTag("doorBtn"))
                    {
                        Debug.Log("Hit doorBtn");

                        interactObj.Interact();
                    }
                    else if (HitInfo.collider.gameObject.CompareTag("movable"))
                    {
                        Debug.Log("Hit movable");
                        hitObject = HitInfo.collider.gameObject;
                        pickedUp = !pickedUp;
                        interactObj.Interact();
                    }
                    else if (HitInfo.collider.gameObject.CompareTag("movableV2"))
                    {
                        Debug.Log("Hit movable");
                        hitObject = HitInfo.collider.gameObject;
                        pickedUp = !pickedUp;
                        interactObj.Interact();
                    }

                    else if (HitInfo.collider.gameObject.CompareTag("lightBtn"))
                    {
                        interactObj.Interact();
                    }
                    
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (pickedUp)
            {
                //interactObj.Inspect();
            }
        }
    }
    public bool IsPickedUp()
    {
        bool pickedUp = this.pickedUp;
        return pickedUp;
    }
    public void setPickedUp(bool pickedUp)
    {
        this.pickedUp = pickedUp;
    }
    
    public void  letgo()
    {
        if(pickedUp) { 
            interactObj.Interact();
            pickedUp = !pickedUp;
            interactObj = null;
        }
    }
    public GameObject getObject()
    {
        if(interactObj != null)
        {
            return hitObject;
        }
        return null;
    }
}
