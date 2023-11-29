using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravNodeV2Script : MonoBehaviour
{

    private List<GameObject> gravNodeList = new List<GameObject>();
    private List<GameObject> allgravNodelist = new List<GameObject>();

    private GameObject movableObj = null;
    private GameObject closestNode = null;

    private GameObject radar;

    public void Start()
    {
        Debug.Log("Grav Node Start");

        radar = GameObject.Find("PickUpPoint").transform.GetChild(0).gameObject;
    }


    private void OnTriggerEnter(Collider colliding)
    {
        Debug.Log("Trigger Enter: " + colliding.name);
        if (colliding.gameObject.CompareTag("clipPoint") && movableObj != null && !colliding.gameObject.GetComponent<DublicateV2Script>().getOccupied())
        {
            Debug.Log("Trigger Enter clipPoint");
            if (!gravNodeList.Contains(colliding.gameObject))
            {
                allgravNodelist.Add(colliding.gameObject);
                gravNodeList.Add(colliding.gameObject);
                colliding.gameObject.GetComponent<DublicateV2Script>().createDublicate(movableObj);
            }
        }

    }
    private void OnTriggerExit(Collider colliding)
    {
        if (colliding.gameObject.CompareTag("clipPoint"))
        {
            if (gravNodeList.Contains(colliding.gameObject))
            {
                Debug.Log("Trigger Exit clipPoint");

                allgravNodelist.Remove(colliding.gameObject);
                gravNodeList.Remove(colliding.gameObject);
                colliding.gameObject.GetComponent<DublicateV2Script>().destroyDublicate();
            }
        }
    }



    private void FixedUpdate()
    {
        //Update Closest Node
        if(movableObj != null)
        {
            if (movableObj.GetComponent<PickupV2Script>().getPickedup())
            {
                if (gravNodeList.Count > 0)
                {
                    GameObject closestNode = getClosestGravNode();
                    foreach (GameObject node in gravNodeList)
                    {
                        if (node == closestNode)
                        {
                            node.GetComponent<DublicateV2Script>().setMain();
                        }
                        else
                        {
                            node.GetComponent<DublicateV2Script>().setSecondary();
                        }
                    }
                }
            }
        }
       
    }

    public   void setItem(GameObject item)
    {
        movableObj = item;
    }

    public GameObject getClosestGravNode()
    {
        float closestDis = 0;
        GameObject closestNode = null;
        foreach (GameObject graveNode in gravNodeList)
        {
            if (closestNode == null)
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
    public void initGravNodeList()
    {
        Debug.Log("Init gravNodeList");
        LayerMask mask = LayerMask.GetMask("gravClip");
        allgravNodelist.Clear();
        gravNodeList.Clear();
        Collider[] colliders = Physics.OverlapSphere(radar.transform.position, GetComponent<SphereCollider>().radius, mask);
        foreach (Collider collider in colliders)
        {
            allgravNodelist.Add(collider.gameObject);
            if (collider.gameObject.GetComponent<DublicateV2Script>().getOccupied()) continue;
            gravNodeList.Add(collider.gameObject);
            collider.gameObject.GetComponent<DublicateV2Script>().createDublicate(movableObj);

        }
    }

    public bool checkColliders()
    {
        // Has to be tested
        GameObject raycastPos = getClosestGravNode();
        if (raycastPos == null) return true;
        Collider[] nodeColliders = Physics.OverlapSphere(raycastPos.transform.position, 4f, LayerMask.GetMask("gravClip"));

        foreach (Collider node in nodeColliders)
        {
            if (!node.gameObject.Equals(raycastPos) && node.gameObject.GetComponent<DublicateV2Script>().getOccupied())
            {
                Vector3 direction = node.transform.position - raycastPos.transform.position;
                Renderer renderer = movableObj.GetComponent<Renderer>();
                Bounds bounds = renderer.bounds;
                float distance = bounds.extents.magnitude;
                if (Physics.Raycast(raycastPos.transform.position, direction, distance, LayerMask.GetMask("Interactable")))
                {
                    Debug.LogWarningFormat("Shit i cant place {0} because of: {1} ", movableObj.name, node.gameObject.name);
                    return false;
                }
            }
        }
        return true;
    }


}

