using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> unitsList = new List<GameObject>();
    private List<GameObject> buildingsList = new List<GameObject>();
    public List<GameObject> availableUnits;
    public GameObject mainTownhall;

    public int maxPopulation;
    public int currentPopulation;
    public int gold;
    public int wood;
    public int stone;
    public int food;

    Text resources;

    int team;

    void Start()
    {
        if(this.name == "Player1")
        {
            team = 0;
        }
        else
        {
            team = 1;
        }
        maxPopulation = 10;
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        GameObject[] knights = GameObject.FindGameObjectsWithTag("Knight");
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");

        GameObject[] tempList = new GameObject[soldiers.Length + knights.Length + villagers.Length];
        soldiers.CopyTo(tempList, 0);
        knights.CopyTo(tempList, soldiers.Length);
        villagers.CopyTo(tempList, knights.Length);

        for (int i = 0; i < tempList.Length; i++)
        {
            if(tempList[i].GetComponent<FSM>().team == team)
            {
                unitsList.Add(tempList[i]);
            }
        }
        buildingsList.Add(mainTownhall);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        currentPopulation = unitsList.Count;
        if(gameObject.name == "Player1")
        {
            resources = GameObject.Find("ResourcesText").GetComponent<Text>();
            resources.text = "Gold: " + gold + "\nWood: " + wood + "\nStone: " + stone + "\nFood: " + food + "\nPopulation: " + currentPopulation + "/" + maxPopulation;
        }
        

    }

    public List<GameObject> getUnits()
    {
        return unitsList;
    }
    public List<GameObject> getBuildings()
    {
        return buildingsList;
    }

    public List<GameObject> getAvailibleUnits()
    {
        return availableUnits;
    }
    public void AddBuilding(GameObject building)
    {
        buildingsList.Add(building);
    }

    public void AddResource(string resourceName)
    {
        switch(resourceName)
        {
            case "Wood":
                wood += 10;
                break;
            case "Stone":
                stone += 10;
                break;
            case "Food":
                food += 10;
                break;
            case "Gold":
                gold += 10;
                break;
        }
            
    }
}
