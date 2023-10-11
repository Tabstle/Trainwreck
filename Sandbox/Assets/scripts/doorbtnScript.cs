using UnityEngine;

public class doorbtnScript : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject doorLeft;
    [SerializeField] private GameObject doorRight;

    [SerializeField] private float doorSpeed = 1f;

    private Vector3 doorLeftDirection;
    private Vector3 doorRightDirection;

    private Vector3 doorLeftTargetPosition;
    private Vector3 doorRightTargetPosition;

    private bool blocked = false;
    private bool open = false;
    public void Interact()
    {
        if(doorLeft != null && doorRight != null)
        {
            if (blocked)
            {
                return;
            }
            if (open)
            {
                closeDoors();
            }else
            {
                openDoors();
            }
        }
        else if(door != null)
        {
        
        }
    }
    public void Start()
    {
        doorLeftDirection = (doorLeft.transform.position - doorRight.transform.position);
        doorRightDirection = (doorRight.transform.position - doorLeft.transform.position);
    }
    public void Update()
    {
        if (doorLeftTargetPosition != Vector3.zero && doorRightTargetPosition != Vector3.zero)
        {

            Debug.Log("opening door");
            Vector3 newDoorLeftPosition = Vector3.MoveTowards(doorLeft.transform.position, doorLeftTargetPosition, doorSpeed * Time.deltaTime);
            doorLeft.transform.position = newDoorLeftPosition;

            Vector3 newDoorRightPosition = Vector3.MoveTowards(doorRight.transform.position, doorRightTargetPosition, doorSpeed * Time.deltaTime);
            doorRight.transform.position = newDoorRightPosition;

            if (Vector3.Distance(doorLeft.transform.position, doorLeftTargetPosition) == 0f && Vector3.Distance(doorRight.transform.position, doorRightTargetPosition) == 0f)
            {
                Debug.Log("Doors finished moving");
                doorLeftTargetPosition = Vector3.zero;
                doorRightTargetPosition = Vector3.zero;
                blocked = false;
            }
        }
    }

    public void openDoors()
    {
        open = true;
        blocked = true;
        doorLeftTargetPosition = doorLeft.transform.position + doorLeftDirection;
        doorRightTargetPosition = doorRight.transform.position + doorRightDirection;
    }
    public void closeDoors()
    {
        open = false;
        blocked = true;
        doorLeftTargetPosition = doorLeft.transform.position - doorLeftDirection;
        doorRightTargetPosition = doorRight.transform.position - doorRightDirection;
    }
}
