using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class BuildingManager : MonoBehaviour
{

    public Grid grid;

    public GameObject[] buildings;

    public Player player1;

    public Player player2;

    private GameObject currentBuilding;

    public GameObject villagerBuilding;

    public bool worldChanged;


    // Update is called once per frame
    void Update()
    {
        
        if (currentBuilding != null && !currentBuilding.GetComponent<BuildingScript>().isBuilt)
        {
            var currentMousePosition = grid.GetTileCenterPosition();

            if (currentBuilding.tag == "Barracks")
            {

                currentMousePosition = currentMousePosition - new Vector3(0, 0, 5f);
            }
            if (currentBuilding.tag == "Stables" || currentBuilding.tag == "Townhall")
            {
                currentMousePosition = currentMousePosition - new Vector3(5f, 0, 0);
            }

            currentBuilding.transform.position = currentMousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(currentMousePosition);
                GameObject.Find("AStar").GetComponent<GridA>().UpdateGrid();
                worldChanged = true;
            }
            

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(currentBuilding);
            }
            


        }
    }

    public void PlaceBuilding(Vector3 mousePosition)
    {
        BuildingScript buildingScript = currentBuilding.GetComponent<BuildingScript>();
        
        if (grid.GetValue(mousePosition) == "Empty" && buildingScript.canBePlaced)
        {
            
            currentBuilding.transform.position = mousePosition;
            buildingScript.isBeingBuilt = true;

            villagerBuilding.GetComponent<BuildScript>().target = mousePosition;
            villagerBuilding.GetComponent<BuildScript>().building = currentBuilding;


            changeColor(Color.white);
            if(currentBuilding != null && currentBuilding.tag == "House")
            {
                player1.wood -= 150;
                player1.AddBuilding(currentBuilding);
                
            }
            else if(currentBuilding.tag == "Warehouse")
            {
                player1.food -= 100;
                player1.wood -= 200;
                player1.AddBuilding(currentBuilding);
            }
            else if(currentBuilding.tag == "Townhall")
            {
                player1.food -= 300;
                player1.wood -= 300;
                player1.wood -= 300;
                player1.wood -= 300;
                player1.AddBuilding(currentBuilding);
            }
            else if (currentBuilding.tag == "Barracks")
            {
                player1.food -= 100;
                player1.wood -= 250;
                player1.gold -= 100;
                player1.AddBuilding(currentBuilding);
            }
            else if (currentBuilding.tag == "Stables")
            {
                player1.food -= 100;
                player1.wood -= 300;
                player1.gold -= 150;
                player1.AddBuilding(currentBuilding);
            }
            
            currentBuilding = null;
            
        }
    }

    public void BuildingPlacement(string name)
    {
        
        
        villagerBuilding = GameObject.Find("GameRTSController").GetComponent<GameRTSController>().selectedUnitsList[0];
        if (villagerBuilding.GetComponent<FSM>().building)
        {
            return;
        }
        villagerBuilding.GetComponent<FSM>().building = true;



        if (name == "House" && player1.wood >= 150)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[0]); //house
            changeColor(Color.green);
            
        }
        if (name == "Warehouse" && player1.wood >= 200 && player1.food >= 100)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[1]); //warehouse
            changeColor(Color.green);
            
        }
        if (name == "Townhall" && player1.wood >= 300 && player1.gold >= 300 && player1.stone >= 300 && player1.food >= 300)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[2]); //townhall
            changeColor(Color.green);
            
        }
        if (name == "Barracks" && player1.wood >= 250 && player1.gold >= 100 && player1.food >= 100)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[3]); //barracks
            changeColor(Color.green);
            
        }
        if (name == "Stables" && player1.wood >= 300 && player1.gold >= 150 && player1.food >= 100)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[4]); //stables
            changeColor(Color.green);
            
            
        }
        if (Input.GetKeyDown("escape") && currentBuilding != null)
        {
            Destroy(currentBuilding);
        }
        
    }
    private void changeColor(Color color)
    {
        Transform[] buildingParts = currentBuilding.transform.GetChild(0).GetComponentsInChildren<Transform>();
        for (int i = 0; i < buildingParts.Length; i++)
        {
            buildingParts[i].GetComponent<MeshRenderer>().material.color = color;
        }
    }

    public void CreateGrid()
    {
        Vector3Int mesh = Vector3Int.FloorToInt(GetComponent<MeshRenderer>().bounds.size);
        Vector3Int scale = Vector3Int.FloorToInt(GetComponent<Transform>().localScale);
        grid = new Grid(mesh.x / 10, mesh.z / 10, 10, new Vector3(-mesh.x / 2, 0, -mesh.z / 2));
    }
}
