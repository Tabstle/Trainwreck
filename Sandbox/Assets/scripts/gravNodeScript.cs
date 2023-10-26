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
    [SerializeField] private GameObject initGravNode; // if u wanna have an object directli on an gravNode u  have to reference it here;

    private void OnTriggerEnter(Collider colliding)
    {
        if (colliding.gameObject.CompareTag("clipPoint") && movableObj.GetComponent<PickupScript>().getPickedup() && !colliding.gameObject.GetComponent<gravNodeDubScript>().getOccupied())
        {
            if(!gravNodeList.Contains(colliding.gameObject))
            {
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
        // Has to be tested
        GameObject raycastPos = getClosestGravNode();
        if (raycastPos == null) return true;
        Collider[] nodeColliders = Physics.OverlapSphere(raycastPos.transform.position,4f,LayerMask.GetMask("gravClip"));

        foreach (Collider node in nodeColliders)
        {
            if (!node.gameObject.Equals(raycastPos) && node.gameObject.GetComponent<gravNodeDubScript>().getOccupied())
            {
                Vector3 direction = node.transform.position - raycastPos.transform.position;
                Renderer renderer = movableObj.GetComponent<Renderer>();
                Bounds bounds = renderer.bounds;
                float distance = bounds.extents.magnitude;
                if (Physics.Raycast(raycastPos.transform.position, direction, distance, LayerMask.GetMask("Interactable")))
                {
                    Debug.LogWarningFormat("Shit i cant place {0} because of: {1} ", movableObj.name ,node.gameObject.name);
                    return false;
                }
            }
        }
        return true;
    }

    public void shakeDublicate()
    {
        Debug.Log("Closest Node DUblicate : " + getClosestGravNode().GetComponent<gravNodeDubScript>().getDublicate());
        GameObject dublicate = getClosestGravNode().GetComponent<gravNodeDubScript>().getDublicate();
        this.GetComponent<shakeScript>().shakeObj(dublicate, 0.2f);
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

    public void clearList()
    {
        gravNodeList.Clear();
    }
     
    private void initialiseGravNode() //
    {
        gravNodeHost = initGravNode;
        allgravNodelist.Add(gravNodeHost);
        gravNodeList.Add(gravNodeHost);
        gravNodeHost.GetComponent<gravNodeDubScript>().setOccupied(true, movableObj);
        movableObj.GetComponent<PickupScript>().setMoveToClipNode(true);

    }


    public void Start()
    {
        movableObj = transform.parent.gameObject;
        if(initGravNode != null)
        {
            initialiseGravNode();
        }
    }
}
