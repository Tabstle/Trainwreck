using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class gravNodeDubScript : MonoBehaviour
{

    private bool isOccupied = false;
    [SerializeField] private GameObject movableObj = null;    
    private GameObject meshObject = null;
    private GameObject dublicate = null;
    private Vector3 objectHeight;
    [SerializeField] private Material material;


    public void createDublicate(GameObject meshObject)
    {
        Debug.LogWarning("Create Dublicate");
        this.meshObject = meshObject;
        objectHeight = new Vector3(0, meshObject.transform.lossyScale.y * 0.5f, 0);


        MeshFilter meshFilter = meshObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = meshObject.GetComponent<MeshRenderer>();
        if (meshFilter != null && meshRenderer != null)
        {
            //Debug.Log("Init clip");
            dublicate = new GameObject("Silhouette of: " + meshObject.name);
            // Setts Scale and Postion of the dublicate
            dublicate.transform.localScale = meshObject.transform.localScale;
            dublicate.transform.position = transform.position + objectHeight;


            MeshFilter newMeshFilter = dublicate.AddComponent<MeshFilter>();
            newMeshFilter.sharedMesh = meshFilter.sharedMesh;
            MeshRenderer newMeshRenderer = dublicate.AddComponent<MeshRenderer>();

            // add Transparent Material
            newMeshRenderer.material = this.material;

            Outline outline = dublicate.AddComponent<Outline>();
            if (!meshObject.transform.GetChild(0).GetComponent<gravNodeScript>().checkColliders())
            {
                outline.OutlineColor  = new Color(0.85f, 0.4f, 0.4f, 0.4f);
            }
            else
            {
                outline.OutlineColor = new Color(1, 1, 1, 0.4f);
            }
            
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineWidth = 10;

            dublicate.GetComponent<Outline>().enabled = false;
            dublicate.GetComponent<Outline>().enabled = true;

        }
    }
    public void setOccupied(bool isOccupied, GameObject movableObj)
    {
        this.isOccupied = isOccupied;
        if (isOccupied)
        {
            this.movableObj = movableObj;
        }
        else
        {
            this.movableObj = null;
        }
    }
    public bool getOccupied()
    {
        return isOccupied;
    }

    public GameObject getMovableObj()
    {
        return movableObj;
    }

    public GameObject getDublicate()
    {
        return dublicate;
    }

    public void setMain()
    {
        dublicate.GetComponent<Outline>().OutlineColor = new Color(1, 1, 1, 0.4f);
    }
    public void setSecondary()
    {
        dublicate.GetComponent<Outline>().OutlineColor = new Color(1, 1, 1, 0.1f);
    }

    public void setError()
    {
        dublicate.GetComponent<Outline>().OutlineColor = new Color(0.85f, 0.4f, 0.4f, 0.4f);

    }

    public void destroyDublicate()
    {
        meshObject = null;
        Destroy(dublicate);
    }
    public bool checkEquals(Object o)
    {
        if (o != null && movableObj != null)
        {
            Debug.Log("Tag of dim1: " + movableObj.GetComponent<PickupScript>().ObjectTag);
            Debug.Log(" Tag of dim2: " + o.GetComponent<PickupScript>().ObjectTag);
            if (movableObj.GetComponent<PickupScript>().ObjectTag == o.GetComponent<PickupScript>().ObjectTag)
            {
                return true;
            }
        }
        return false;
    }
}
