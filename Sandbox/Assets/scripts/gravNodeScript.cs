using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class gravNodeScript : MonoBehaviour
{

    private GameObject gravNodeHost = null;
    private List<GameObject> gravNodeList = new List<GameObject>();
    private List<GameObject> allgravNodelist = new List<GameObject>();
    private GameObject movableObj = null;
    private GameObject closestNode = null;

    private void OnTriggerEnter(Collider colliding)
    {
        if (colliding.gameObject.CompareTag("clipPoint") && movableObj.GetComponent<PickupScript>().getPickedup() && !colliding.gameObject.GetComponent<gravNodeDubScript>().getOccupied())
        {
            if(!gravNodeList.Contains(colliding.gameObject))
            {
               Debug.Log("Trigger Enter clipPoint not in list");
               allgravNodelist.Add(colliding.gameObject);
               gravNodeList.Add(colliding.gameObject);
               colliding.gameObject.GetComponent<gravNodeDubScript>().createDublicate(movableObj);
            }
        }
       
    }
    private void OnTriggerExit(Collider colliding)
    {
        if (colliding.gameObject.CompareTag("clipPoint"))
        {
            if (gravNodeList.Contains(colliding.gameObject))
            {
                Debug.Log("Trigger Exit clipPoint in list");
                allgravNodelist.Remove(colliding.gameObject);
                gravNodeList.Remove(colliding.gameObject);
                colliding.gameObject.GetComponent<gravNodeDubScript>().destroyDublicate();
            }
        }

        
    }
    private void FixedUpdate()
    {
        //Update Closest Node
        if (gravNodeHost == null)
        {
            if (gravNodeList.Count > 0)
            {
                GameObject closestNode = getClosestGravNode();
                foreach (GameObject node in gravNodeList)
                {
                    if (node == closestNode)
                    {
                        node.GetComponent<gravNodeDubScript>().setMain();
                    }
                    else
                    {
                        node.GetComponent<gravNodeDubScript>().setSecondary();
                    }
                }
            }
        }        
    }

    public void setgravNodeHost()
    {
        this.gravNodeHost = getClosestGravNode();
        gravNodeHost.GetComponent<gravNodeDubScript>().setOccupied(true, movableObj);
    }
    public void releaseGravNodeHost()
    {
        gravNodeHost.GetComponent<gravNodeDubScript>().setOccupied(false, movableObj);
        gravNodeHost = null;
    }


    public void initGravNodeList()
    {
        Debug.Log("Init gravNodeList");
        LayerMask mask = LayerMask.GetMask("gravClip");
        allgravNodelist.Clear();
        gravNodeList.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, mask);
        foreach (Collider collider in colliders)
        {
            allgravNodelist.Add(collider.gameObject);
            if (collider.gameObject.GetComponent<gravNodeDubScript>().getOccupied()) continue;
            gravNodeList.Add(collider.gameObject);
            collider.gameObject.GetComponent<gravNodeDubScript>().createDublicate(movableObj);

        }
    }

    public bool checkColliders()
    {
        Debug.Log("Check Colliders");
        foreach (GameObject node in allgravNodelist)
        {
            Debug.LogWarning("Node: " + node.name);
            if (node.GetComponent<gravNodeDubScript>().getOccupied())
            {
                Collider[] colliders = Physics.OverlapSphere(getClosestGravNode().transform.position, GetComponent<SphereCollider>().radius, LayerMask.GetMask("Interactable"));
                foreach (Collider collider in colliders)
                {
                    Debug.LogWarning("Collider: " + collider.gameObject.name);
                    if (collider.gameObject == node.GetComponent<gravNodeDubScript>().getMovableObj())
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public GameObject getGravNodeHost()
    {
        return gravNodeHost;
    }

    public GameObject getClosestGravNode()
    {
        float closestDis = 0;
        GameObject closestNode = null;
        foreach (GameObject graveNode in gravNodeList)
        {
            if(closestNode == null)
            {
                closestNode = graveNode;
                closestDis = Vector3.Distance(movableObj.transform.position, graveNode.transform.position);
            }
            else
            {
                if (Vector3.Distance(movableObj.transform.position, graveNode.transform.position) < closestDis)
                {
                    closestNode = graveNode;
                    closestDis = Vector3.Distance(movableObj.transform.position, graveNode.transform.position);
                }
            }            
        }
        this.closestNode = closestNode;
        return closestNode;
    }

    public bool hasValidGravNode()
    {
        if (gravNodeList.Count > 0)
        {
            return true;
        }
        return false;
    }

    public void removeAllDublicates()
    {
        foreach (GameObject node in gravNodeList)
        {
            node.GetComponent<gravNodeDubScript>().destroyDublicate();
        }
    }
     
    
    public void Start()
    {
        movableObj = transform.parent.gameObject;
    }
}
