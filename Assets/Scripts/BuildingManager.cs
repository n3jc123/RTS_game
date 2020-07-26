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

    BuildingScript buildingScript;

    // Start is called before the first frame update

    private void Awake()
    {
        Vector3Int mesh = Vector3Int.FloorToInt(GetComponent<MeshRenderer>().bounds.size);
        Vector3Int scale = Vector3Int.FloorToInt(GetComponent<Transform>().localScale);


        grid = new Grid(mesh.x / 10, mesh.z / 10, 10, new Vector3(-mesh.x / 2, 0, -mesh.z / 2));
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
            buildingScript.isBuilt = true;
            
            
            
            changeColor(Color.white);
            if(currentBuilding != null && currentBuilding.tag == "House")
            {
                player.AddBuilding(Instantiate(currentBuilding));
                player.maxPopulation += 5;
            }

            currentBuilding = null;
            
        }
    }

    public void BuildingPlacement(string name)
    {
       
        
        if (name == "House")
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[0]); //house

            //currentBuilding.GetComponent<BuildingScript>().unit = "Villager";

            changeColor(Color.green);
        }
        if (name == "Warehouse")
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[1]); //warehouse
            //currentBuilding = new Warehouse();
            changeColor(Color.green);
        }
        if (name == "Townhall")
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[2]); //townhall
            //currentBuilding = new Building("Townhall");
            changeColor(Color.green);
        }
        if (name == "Barracks")
        {
            Destroy(currentBuilding);
            currentBuilding = Instantiate(buildings[3]); //barracks
            //currentBuilding = new Building("Barracks");
            changeColor(Color.green);
        }
        if (name == "Stables")
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
        Transform[] buildingParts = currentBuilding.GetComponentsInChildren<Transform>();
        for (int i = 0; i < buildingParts.Length; i++)
        {
            buildingParts[i].GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
