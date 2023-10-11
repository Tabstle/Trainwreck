using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorbtnScript : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject doorLeft;
    [SerializeField] private GameObject doorRight;
    public void Interact()
    {
        if(doorLeft != null && doorRight != null)
        {
            
        }
        else if(door != null)
        {
        
        }
    }
}
