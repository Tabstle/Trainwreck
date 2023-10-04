using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
   // public void Interact(GameObject gameObject);
}

public class Interact : MonoBehaviour
{
    [SerializeField] private LayerMask PickupMask;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] float InteractionDistance = 2f;
    private bool pickedUp = false;
    private IInteractable interactObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedUp) {
                interactObj.Interact();
                pickedUp = !pickedUp;
                interactObj = null;
            }
            Ray CameraRay = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(CameraRay, out RaycastHit HitInfo, InteractionDistance, PickupMask))
            {
                //Debug.Log("Hit " + HitInfo.collider.gameObject.name);
                if (HitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    this.interactObj = interactObj;
                    pickedUp = !pickedUp;
                    interactObj.Interact();
                }
            }
        }
    }
    public bool IsPickedUp()
    {
        bool pickedUp = this.pickedUp;
        return pickedUp;
    }
    public void  letgo()
    {
        if(pickedUp) { 
            interactObj.Interact();
            pickedUp = !pickedUp;
            interactObj = null;
        }
    }
    //public void hoverNode(GameObject clipNode)
    //{
    //    interactObj.Interact(clipNode);
    //}

}
