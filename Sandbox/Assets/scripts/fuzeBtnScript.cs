using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class fuzeBtnScript : MonoBehaviour, IInteractable
{

    //Movement Variables
    [SerializeField] private Vector3 direction = new Vector3(0,1,0);
    [SerializeField] private float speed = 0.5f;
    private bool movingTowardsTarget = true;
    private Vector3 startpos;
    private Vector3 targetpos;
    private Vector3 length;
    private bool blocked = false;
    private bool returning = false;


    //Wagon && Nodes
    [SerializeField] private GameObject wagenDim1;
    [SerializeField] private GameObject wagenDim2;

    private GameObject[] GraveNodesDim1;
    private GameObject[] GraveNodesDim2;

    //Light Riddle

    [SerializeField] private GameObject lightRiddleControllerObjDim1;
    [SerializeField] private GameObject lightRiddleControllerObjDim2;


    public void Interact()
    {
        Debug.Log("FuzeButton pressed");
        if (blocked) return;
        blocked = true;
        if (checkFuse())
        {
            Debug.Log("Fuse is correct");
        }
        


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

    private bool checkFuse()
    {
        bool fuseCorrect = false;
        //Check for LightRiddle
        if (lightRiddleControllerObjDim1 != null && lightRiddleControllerObjDim2 != null)
        {
            int children = lightRiddleControllerObjDim1.transform.GetChild(0).transform.childCount;
            int children2 = lightRiddleControllerObjDim2.transform.GetChild(0).transform.childCount;

            if (children == children2)
            {
                //Loops all lights
                for (int i = 0; i < children; i++)
                {
                    // as soon as light[i] is not equal to light[i] return false
                    if (!(lightRiddleControllerObjDim1.transform.GetChild(0).transform.GetChild(i).gameObject.activeSelf == lightRiddleControllerObjDim2.transform.GetChild(0).transform.GetChild(i).gameObject.activeSelf))
                    {
                        Debug.LogWarning("Lights are not equal");
                        return false;
                    }
                }
            }
            
        }

        if (checkGravNodes())
        {
            fuseCorrect = true;
            Debug.Log("GraveNodes are correct");
        }
        else
        {
            fuseCorrect = false;
            Debug.Log("GraveNodes are not correct");
        }

        return fuseCorrect;
    }

    private bool checkGravNodes()
    {
        //Check  GravNodes
        int length = wagenDim1.transform.GetChild(0).transform.childCount;
        int movables = (wagenDim1.transform.GetChild(4).transform.childCount + wagenDim2.transform.GetChild(4).transform.childCount);
        if (movables % 2 != 0)
        {
            Debug.LogError("Uneven amount of movables");
            return false;
        }
        int objectCounter = 0;
        for (int i = 0; i < length; i++)
        {
            // TODO: Check if gravNodeHost is null
            GameObject GravNodeDim1;
            GameObject GravNodeDim2;
            try
            {
                GravNodeDim1 = wagenDim1.transform.GetChild(0).transform.GetChild(i).gameObject;
                GravNodeDim2 = wagenDim2.transform.GetChild(0).transform.GetChild(i).gameObject;
            }
            catch (System.Exception)
            {
                Debug.LogWarning("GravNode not found");
                return false;
            }

            // Check if Object on GravNode is equal to the object at the other Position
            if (GravNodeDim1.GetComponent<gravNodeScript>().getGravNode() == null && GravNodeDim2.GetComponent<gravNodeScript>().getGravNode() == null)
            {

            }
            else if (GravNodeDim1.GetComponent<gravNodeScript>().getGravNode() == null && GravNodeDim2.GetComponent<gravNodeScript>().getGravNode() != null)
            {
                Debug.LogWarning("Anordnung nicht korrekt");
                return false;
            }
            else if (GravNodeDim1.GetComponent<gravNodeScript>().getGravNode() != null && GravNodeDim2.GetComponent<gravNodeScript>().getGravNode() == null)
            {
                Debug.LogWarning("Anordnung nicht korrekt");
                return false;
            }// Equals cant work like this have to change it
            else if (GravNodeDim1.GetComponent<gravNodeScript>().checkEquals(GravNodeDim2.GetComponent<gravNodeScript>().getGravNode()))
            {
                objectCounter++;
                objectCounter++;
            }
            else
            {
                objectCounter++;
            }

        }
        if (objectCounter != movables)
        {
            Debug.LogWarning("Not all movables are on a gravNode. " + movables + ":totalmovables; " + objectCounter + ":on a GravNode");
            
        }
        return false;
    }
}
