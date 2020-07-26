using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> unitsList = new List<GameObject>();
    private List<GameObject> buildingsList = new List<GameObject>();
    public List<GameObject> availableUnits;
    public int maxPopulation;
    public int currentPopulation;
    public int gold;
    public int wood;
    public int stone;
    public int food;

    Text resources;



    void Start()
    {
        maxPopulation = 10;
        gold = 100;
        wood = 100;
        stone = 100;
        food = 200;
    }

    // Update is called once per frame
    void Update()
    {
        currentPopulation = unitsList.Count;
        resources = GameObject.Find("ResourcesText").GetComponent<Text>();
        resources.text = "Gold: " + gold + "\nWood: " + wood + "\nStone: " + stone + "\nFood: " + food + "\nPopulation: " +currentPopulation + "/" + maxPopulation;

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
}
