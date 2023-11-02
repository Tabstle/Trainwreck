using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightBtnScript : MonoBehaviour, IInteractable
{

    private GameObject lightRiddleControllerObj;
    private LightRiddleController lightRiddleController;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed = 0.5f;
    private bool movingTowardsTarget = true;
    private Vector3 startpos;
    private Vector3 targetpos;
    private bool blocked = false;
    private bool returning = false;

    
    private Vector3 length;

    public void Interact()
    {
        if (blocked || lightRiddleController.getState(gameObject)) return;
        blocked = true;
        Debug.Log("LightButton pressed");
        lightRiddleController.pressedButton(gameObject);
        
       
    }

     void Start()
    {
        length = new Vector3(.05f, .05f, .05f); //TODO: has to bee made dynamic
        targetpos = Vector3.Scale(length, direction);
        startpos = transform.position;
    }
    void Update()
    {
        if (blocked)
        {
            Vector3 target = movingTowardsTarget ? (targetpos + startpos) : startpos;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                movingTowardsTarget = !movingTowardsTarget;
                returning = true;
            }
            if (Vector3.Distance(transform.position, startpos) < 0.01f)
            {
                if (returning)
                {
                    transform.position = startpos;
                    blocked = false;
                    returning = false;
                }
               
            }
        }
    }
    public void setRiddleController (LightRiddleController o)
    {
        lightRiddleController = o.GetComponent<LightRiddleController>();
    }

}
