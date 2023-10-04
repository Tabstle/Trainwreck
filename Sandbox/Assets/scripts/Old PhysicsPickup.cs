using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPickup : MonoBehaviour
{
    //[SerializeField] float clippingDistance = 0.2f;

    [SerializeField] private LayerMask PickupMask;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Transform PickupTarget;
    [Space]
    [SerializeField] private float PickupRange = 2f;
    private Rigidbody CurrentObject;
    private Collider CurrentCollider;
    private bool shouldClip;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentObject)
            {
                var outline = CurrentCollider.GetComponent<Outline>();
                outline.OutlineColor = Color.white;

                
                

                CurrentObject.velocity = Vector3.zero;
                CurrentObject.useGravity = true;
                CurrentObject = null;
                return;
            }

            Ray CameraRay = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(CameraRay, out RaycastHit HitInfo, PickupRange, PickupMask))
            {
                var outline = HitInfo.collider.GetComponent<Outline>();
                CurrentCollider = HitInfo.collider;
                outline.OutlineColor = Color.black;
                CurrentObject = HitInfo.rigidbody;


                CurrentObject.useGravity = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (CurrentObject)
            {
               CurrentObject.transform.Rotate(0, 30, 0);
            }
        }   
    }

    private void FixedUpdate()
    {
        if (CurrentObject)
        {
            Vector3 DirectionToPoint = PickupTarget.position - CurrentObject.position;
            float DistanceToPoint = DirectionToPoint.magnitude;

            CurrentObject.velocity = DirectionToPoint * 12f * DistanceToPoint; 
        }
    }

}
