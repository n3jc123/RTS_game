using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class BuildingManager : MonoBehaviour
{

    public Grid grid;

    public GameObject[] buildings;

    public Player player;

    private GameObject currentBuilding;

    private GameObject RTSController;

    // Start is called before the first frame update

    private void Awake()
    {
        Vector3Int mesh = Vector3Int.FloorToInt(GetComponent<MeshRenderer>().bounds.size);
        Vector3Int scale = Vector3Int.FloorToInt(GetComponent<Transform>().localScale);
        RTSController = GameObject.Find("GameRTSController");

        grid = new Grid(mesh.x / 10, mesh.z / 10, 10, new Vector3(-mesh.x / 2, 0, -mesh.z / 2));
        Debug.Log(mesh.x / 10);
    }
    private void Start()
    {
        
        

    }

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
            


            changeColor(Color.white);
            if(currentBuilding != null && currentBuilding.tag == "House")
            {
                player.wood -= 150;
                player.AddBuilding(Instantiate(currentBuilding));
                
            }
            else if(currentBuilding.tag == "Warehouse")
            {
                player.food -= 100;
                player.wood -= 200;
            }
            else if(currentBuilding.tag == "Townhall")
            {
                player.food -= 300;
                player.wood -= 300;
                player.wood -= 300;
                player.wood -= 300;
            }
            else if (currentBuilding.tag == "Barracks")
            {
                player.food -= 100;
                player.wood -= 250;
                player.gold -= 100;
            }
            else if (currentBuilding.tag == "Stables")
            {
                player.food -= 100;
                player.wood -= 300;
                player.gold -= 150;
            }

            currentBuilding = null;
            
        }
    }

    public void BuildingPlacement(string name)
    {

        //RTSController.GetComponent<GameRTSController>().selectedUnitsList[0].GetComponent<Villager>().isBuilding = true;
        if (name == "House" && player.wood >= 150)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[0]); //house

            //currentBuilding.GetComponent<BuildingScript>().unit = "Villager";

            changeColor(Color.green);
            
        }
        if (name == "Warehouse" && player.wood >= 200 && player.food >= 100)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[1]); //warehouse
            //currentBuilding = new Warehouse();
            changeColor(Color.green);
            
        }
        if (name == "Townhall" && player.wood >= 300 && player.gold >= 300 && player.stone >= 300 && player.food >= 300)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[2]); //townhall
            //currentBuilding = new Building("Townhall");
            changeColor(Color.green);
            
        }
        if (name == "Barracks" && player.wood >= 250 && player.gold >= 100 && player.food >= 100)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[3]); //barracks
            //currentBuilding = new Building("Barracks");
            changeColor(Color.green);
            
        }
        if (name == "Stables" && player.wood >= 300 && player.gold >= 150 && player.food >= 100)
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[4]); //stables
            //currentBuilding = new Building("Stables");
            changeColor(Color.green);
            
            
        }
        if (Input.GetKeyDown("escape"))
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
}
