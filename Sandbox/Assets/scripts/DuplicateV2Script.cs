using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DublicateV2Script : MonoBehaviour
{

    [SerializeField] private GameObject initGravNode; // if u wanna have an object directli on an gravNode u  have to reference it here;

    private bool isOccupied = false;
    private GameObject meshObject = null;
    private GameObject dublicate = null;
    private Vector3 objectHeight;
    [SerializeField] private Material material;

    public void Start()
    {


        if (initGravNode != null)
        {
            //Logik für initGravNode
            meshObject = initGravNode;
    
            //Visual für initGravNode
            objectHeight = getHeight(meshObject);
            meshObject.transform.position = transform.position + objectHeight;
        }

        
    }

    private Vector3 getHeight (GameObject meshObject)
    {
        try
        {
            MeshFilter meshFilter = meshObject.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.mesh != null)
            {
                Vector3[] vertices = meshFilter.mesh.vertices;
                float highestY = 0;
                float lowestY = float.MaxValue;

                foreach (Vector3 vertex in vertices)
                {
                    if (vertex.y > highestY)
                    {
                        highestY = vertex.y;
                    }
                    if (vertex.y < lowestY)
                    {
                        lowestY = vertex.y;
                    }
                }
                return new Vector3(0, (highestY - lowestY) * 0.5f, 0);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("MeshFilter not found");
        }
        

        return Vector3.zero;
    }


    public void createDublicate(GameObject meshObject)
    {
        Debug.LogWarning("Create Dublicate");
        this.meshObject = meshObject;
        objectHeight = getHeight(meshObject);


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
            outline.OutlineColor = new Color(1, 1, 1, 0.4f);
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineWidth = 10;

            dublicate.GetComponent<Outline>().enabled = false;
            dublicate.GetComponent<Outline>().enabled = true;

        }
    }

    public void setOccupied(bool isOccupied, GameObject meshObject)
    {
        this.isOccupied = isOccupied;
        if (isOccupied)
        {
            this.meshObject = meshObject;

            //Visual für setOccupied
            meshObject.transform.position = transform.position + objectHeight;
            meshObject.GetComponent<Rigidbody>().useGravity = false;
            meshObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            meshObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Destroy(dublicate);
        }
        else
        {
            Debug.LogWarning("Meshobject: "+ meshObject);
            Rigidbody rb = this.meshObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            this.meshObject = null;
        }
    }
    public bool getOccupied()
    {
        return isOccupied;
    }

    public GameObject getMovableObj()
    {
        return meshObject;
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
    public void destroyDublicate()
    {
        Debug.LogWarning("Destroy Dublicate");
        meshObject = null;
        Destroy(dublicate);
    }


    public bool checkEquals(Object o)
    {
        if (o != null && meshObject != null)
        {
            Debug.Log("Tag of dim1: " + meshObject.GetComponent<PickupV2Script>().ObjectTag);
            Debug.Log(" Tag of dim2: " + o.GetComponent<PickupV2Script>().ObjectTag);
            if (meshObject.GetComponent<PickupV2Script>().ObjectTag == o.GetComponent<PickupV2Script>().ObjectTag)
            {
                return true;
            }
        }
        return false;
    }
}
