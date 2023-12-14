using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DublicateV2Script : MonoBehaviour
{

    [SerializeField] private GameObject initGravNode; // if u wanna have an object directli on an gravNode u  have to reference it here;

    private bool tpToGravNode = false;
    private bool duplicateToNode = false;

    private bool isOccupied = false;
    private GameObject meshObject = null;
    private GameObject dublicate = null;
    private Vector3 objectHeight;
    [SerializeField] private Material material;

    private Vector3 thePlaceToBe;

    public void Start()
    {


        if (initGravNode != null)
        {
            initMovable(initGravNode);
        }
    }

    private void FixedUpdate()
    {
        if (tpToGravNode)
        {
            tpToGravNode = false;
            meshObject.transform.position = transform.position + objectHeight/2;
        }
        if (duplicateToNode)
        {
            duplicateToNode = false;
            if (dublicate != null)
            {
                dublicate.transform.localScale = meshObject.transform.localScale;
                dublicate.transform.position = transform.position + objectHeight / 2;
                thePlaceToBe = transform.position + objectHeight / 2;
            }
            
        }
    }

    public static Vector3 getHeight (GameObject meshObject)
    {
        try
        {
            MeshFilter meshFilter = meshObject.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.mesh != null)
            {
                Vector3[] vertices = meshFilter.mesh.vertices;
                float highestY = float.MinValue;
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
                return new Vector3(0, (highestY - lowestY), 0);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("MeshFilter not found: "+e);
        }
        

        return Vector3.zero;
    }


    public void createDublicate(GameObject meshObject)
    {
        Debug.LogWarning("Create Dublicate");
        this.meshObject = meshObject;
        objectHeight = getHeight(meshObject);

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        
        meshFilter = meshObject.GetComponent<MeshFilter>();
        meshRenderer = meshObject.GetComponent<MeshRenderer>();
        

        
        if (meshFilter != null && meshRenderer != null)
        {
            Debug.Log("Init clip");
            dublicate = new GameObject("Silhouette of: " + meshObject.name);
            // Setts Scale and Postion of the dublicate

            duplicateToNode = true;

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
        this.meshObject = meshObject;
        if (isOccupied)
        {
            this.meshObject = meshObject;
            Destroy(dublicate);
        }
        else {
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
   
    public void initMovable(GameObject initObject)
    {
        
        Debug.LogWarning("Init GravNode");
        //Logik für initObject
        setOccupied(true, initObject);
        meshObject.GetComponent<Rigidbody>().useGravity = false;
        meshObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        meshObject.GetComponent<PickupV2Script>().setGravnode(this.gameObject);
        //Visual für initObject
        objectHeight = getHeight(meshObject);
        tpToGravNode = true;
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
