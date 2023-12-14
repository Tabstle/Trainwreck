using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizeScript : MonoBehaviour
{

    GameObject trainOne;
    GameObject trainTwo;

    void Start()
    {
        trainOne = GameObject.Find("Train1");
        trainTwo = GameObject.Find("Train2");

        randomize();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            randomize();
        }
    }


    public void randomize()
    {

        float randomizePercentage = 0.8f;

        Debug.Log("Randomize Objcts");
        int nodesTrain1Count = trainOne.transform.GetChild(1).childCount;
        GameObject[] nodesTrain1 = new GameObject[nodesTrain1Count];
        for (int i = 0; i < nodesTrain1Count; i++)
        {
            nodesTrain1[i] = trainOne.transform.GetChild(1).GetChild(i).gameObject;
        }

        List<GameObject> tableNodesTrain1 = new List<GameObject>();
        List<GameObject> floorNodesTrain1 = new List<GameObject>();
        List<GameObject> wallNodesTrain1 = new List<GameObject>();

        for (int i = 0; i < nodesTrain1Count; i++)
        {
            if (nodesTrain1[i].name.Contains("Table"))
            {
                tableNodesTrain1.Add(nodesTrain1[i]);
            }
            else if (nodesTrain1[i].name.Contains("Floor"))
            {
                floorNodesTrain1.Add(nodesTrain1[i]);
            }
            else if (nodesTrain1[i].name.Contains("Wall"))
            {
                wallNodesTrain1.Add(nodesTrain1[i]);
            }
        }

        int nodesTrain2Count = trainTwo.transform.GetChild(1).childCount;
        GameObject[] nodesTrain2 = new GameObject[nodesTrain2Count];
        for (int i = 0; i < nodesTrain2Count; i++)
        {
            nodesTrain2[i] = trainTwo.transform.GetChild(1).GetChild(i).gameObject;
        }

        List<GameObject> tableNodesTrain2 = new List<GameObject>();
        List<GameObject> floorNodesTrain2 = new List<GameObject>();
        List<GameObject> wallNodesTrain2 = new List<GameObject>();


        for (int i = 0; i < nodesTrain2Count; i++)
        {
            if (nodesTrain2[i].name.Contains("Table"))
            {
                tableNodesTrain2.Add(nodesTrain2[i]);
            }
            else if (nodesTrain2[i].name.Contains("Floor"))
            {
                floorNodesTrain2.Add(nodesTrain2[i]);
            }
            else if (nodesTrain2[i].name.Contains("Wall"))
            {
                wallNodesTrain2.Add(nodesTrain2[i]);
            }
        }

        //For Debugging
        for(int i = 0; i < floorNodesTrain1.Count; i++)
        {
            Debug.Log("TableNodeTrain1: " + tableNodesTrain1[i].name);
        }

        int moveableTrain1Count = trainOne.transform.GetChild(2).childCount;
        GameObject[] moveableTrain1 = new GameObject[moveableTrain1Count];

        for (int i = 0; i < moveableTrain1Count; i++)
        {
            moveableTrain1[i] = trainOne.transform.GetChild(2).GetChild(i).gameObject;
        }

        List<GameObject> moveableTableTrain1 = new List<GameObject>();
        List<GameObject> moveableFloorTrain1 = new List<GameObject>();
        List<GameObject> moveableWallTrain1 = new List<GameObject>();

        for (int i = 0; i < moveableTrain1Count; i++)
        {
            GameObject moveable = moveableTrain1[i];
            // disable all moveables
            moveable.SetActive(false);
            switch (moveable.GetComponent<PickupV2Script>().ObjectTag)
            {
                case PickupV2Script.Tag.ArmChair:
                    moveableFloorTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.BottleLarge:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.BottleMedium:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.BottleSmall:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.CandleholderLarge:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.CandleholderMedium:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.CandleholderSmall:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.Chair:
                    moveableFloorTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.Cutlery:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.GlassLarge:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.GlassMedium:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.GlassSmall:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.KnifeLarge:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.KnifeSmall:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.Pan:
                    moveableTableTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.Picture1:
                    moveableWallTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.Picture2:
                    moveableWallTrain1.Add(moveable);
                    break;
                case PickupV2Script.Tag.Vase:
                    moveableTableTrain1.Add(moveable);
                    break;
                default:
                    Debug.LogError("ObjectTag not found");
                    return;
            }  
        }


        int moveableTrain2Count = trainTwo.transform.GetChild(2).childCount;
        GameObject[] moveableTrain2 = new GameObject[moveableTrain2Count];

        for (int i = 0; i < moveableTrain2Count; i++)
        {
            moveableTrain2[i] = trainTwo.transform.GetChild(2).GetChild(i).gameObject;
            moveableTrain1[i].SetActive(false);
        }

        List<GameObject> moveableTableTrain2 = new List<GameObject>();
        List<GameObject> moveableFloorTrain2 = new List<GameObject>();
        List<GameObject> moveableWallTrain2 = new List<GameObject>();


        for (int i = 0; i < moveableTrain2Count; i++)
        {
            GameObject moveable = moveableTrain2[i];
            // disable all moveables
            moveable.SetActive(false);
            switch (moveable.GetComponent<PickupV2Script>().ObjectTag)
            {
                case PickupV2Script.Tag.ArmChair:
                    moveableFloorTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.BottleLarge:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.BottleMedium:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.BottleSmall:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.CandleholderLarge:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.CandleholderMedium:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.CandleholderSmall:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.Chair:
                    moveableFloorTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.Cutlery:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.GlassLarge:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.GlassMedium:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.GlassSmall:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.KnifeLarge:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.KnifeSmall:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.Pan:
                    moveableTableTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.Picture1:
                    moveableWallTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.Picture2:
                    moveableWallTrain2.Add(moveable);
                    break;
                case PickupV2Script.Tag.Vase:
                    moveableTableTrain2.Add(moveable);
                    break;
                default:
                    Debug.LogError("ObjectTag not found");
                    return;
            }
        }


        // Place the percentage of objects

        for (int i = 0; i < (int)(moveableTableTrain1.Count * randomizePercentage); i++)
        {
            GameObject indexMovableTrain1 = moveableTableTrain1[i];
            GameObject indexMovableTrain2 = moveableTableTrain2[i];
            int randomIndex = Random.Range(0, tableNodesTrain1.Count);
            GameObject randomNodeTrain1 = tableNodesTrain1[randomIndex];
            GameObject randomNodeTrain2 = tableNodesTrain2[randomIndex];

            indexMovableTrain1.SetActive(true);
            indexMovableTrain2.SetActive(true);

            randomNodeTrain1.GetComponent<DublicateV2Script>().initMovable(indexMovableTrain1);
            randomNodeTrain2.GetComponent<DublicateV2Script>().initMovable(indexMovableTrain2);

            moveableTableTrain1.Remove(indexMovableTrain1);
            moveableTableTrain2.Remove(indexMovableTrain2);

            tableNodesTrain1.Remove(randomNodeTrain1);
            tableNodesTrain2.Remove(randomNodeTrain2);
        }

        for (int i = 0; i < (int)(moveableFloorTrain1.Count * randomizePercentage); i++)
        {
            GameObject indexMovableTrain1 = moveableFloorTrain1[i];
            GameObject indexMovableTrain2 = moveableFloorTrain2[i];
            int randomIndex = Random.Range(0, floorNodesTrain1.Count);
            GameObject randomNodeTrain1 = floorNodesTrain1[randomIndex];
            GameObject randomNodeTrain2 = floorNodesTrain2[randomIndex];

            indexMovableTrain1.SetActive(true);
            indexMovableTrain2.SetActive(true);

            randomNodeTrain1.GetComponent<DublicateV2Script>().initMovable(indexMovableTrain1);
            randomNodeTrain2.GetComponent<DublicateV2Script>().initMovable(indexMovableTrain2);

            moveableFloorTrain1.Remove(indexMovableTrain1);
            moveableFloorTrain2.Remove(indexMovableTrain2);

            floorNodesTrain1.Remove(randomNodeTrain1);
            floorNodesTrain2.Remove(randomNodeTrain2);
        }
        
        for (int i = 0; i < (int)(moveableWallTrain1.Count * randomizePercentage); i++)
        {
            GameObject indexMovableTrain1 = moveableWallTrain1[i];
            GameObject indexMovableTrain2 = moveableWallTrain2[i];
            int randomIndex = Random.Range(0, wallNodesTrain1.Count);
            GameObject randomNodeTrain1 = wallNodesTrain1[randomIndex];
            GameObject randomNodeTrain2 = wallNodesTrain2[randomIndex];

            indexMovableTrain1.SetActive(true);
            indexMovableTrain2.SetActive(true);

            randomNodeTrain1.GetComponent<DublicateV2Script>().initMovable(indexMovableTrain1);
            randomNodeTrain2.GetComponent<DublicateV2Script>().initMovable(indexMovableTrain2);

            moveableWallTrain1.Remove(indexMovableTrain1);
            moveableWallTrain2.Remove(indexMovableTrain2);

            wallNodesTrain1.Remove(randomNodeTrain1);
            wallNodesTrain2.Remove(randomNodeTrain2);
        }   



        List<GameObject> combinedTableMoveables = new List<GameObject>();
        combinedTableMoveables.AddRange(moveableTableTrain1);
        combinedTableMoveables.AddRange(moveableTableTrain2);

        List<GameObject> combinedFloorMoveables = new List<GameObject>();
        combinedFloorMoveables.AddRange(moveableFloorTrain1);
        combinedFloorMoveables.AddRange(moveableFloorTrain2);

        List<GameObject> combinedWallMoveables = new List<GameObject>();
        combinedWallMoveables.AddRange(moveableWallTrain1);
        combinedWallMoveables.AddRange(moveableWallTrain2);


        List<GameObject> combinedFloorNodes = new List<GameObject>();
        combinedFloorNodes.AddRange(floorNodesTrain1);
        combinedFloorNodes.AddRange(floorNodesTrain2);

        List<GameObject> combinedWallNodes = new List<GameObject>();
        combinedWallNodes.AddRange(wallNodesTrain1);
        combinedWallNodes.AddRange(wallNodesTrain2);

        List<GameObject> combinedTableNodes = new List<GameObject>();
        combinedTableNodes.AddRange(tableNodesTrain1);
        combinedTableNodes.AddRange(tableNodesTrain2);




        // Randomize Floor
        for (int i = 0; i < combinedFloorMoveables.Count; i++)
        {
            GameObject indexMovable = combinedFloorMoveables[i];
            int randomIndex = Random.Range(0, combinedFloorNodes.Count);
            GameObject randomNode = combinedFloorNodes[randomIndex];

            indexMovable.SetActive(true);
            randomNode.GetComponent<DublicateV2Script>().initMovable(indexMovable);
            combinedFloorNodes.Remove(randomNode);
        }

        // Randomize Wall
        for (int i = 0; i < combinedWallMoveables.Count; i++)
        {
            GameObject indexMovable = combinedWallMoveables[i];
            int randomIndex = Random.Range(0, combinedWallNodes.Count);
            GameObject randomNode = combinedWallNodes[randomIndex];

            indexMovable.SetActive(true);
            randomNode.GetComponent<DublicateV2Script>().initMovable(indexMovable);
            combinedWallNodes.Remove(randomNode);
        }

        // Randomize Table
        for (int i = 0; i < combinedTableMoveables.Count; i++)
        {
            GameObject indexMovable = combinedTableMoveables[i];
            int randomIndex = Random.Range(0, combinedTableNodes.Count);
            GameObject randomNode = combinedTableNodes[randomIndex];

            indexMovable.SetActive(true);
            randomNode.GetComponent<DublicateV2Script>().initMovable(indexMovable);
            combinedTableNodes.Remove(randomNode);
        }
    }
}
