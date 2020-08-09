using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameRTSController : MonoBehaviour
{
    private Vector3 startPosition;

    private Vector2 startUIPosition;

    private Vector3 endPosition;

    public List<GameObject> selectedUnitsList;

    private GameObject selectedBuilding;

    public List<GameObject> playerUnits;

    public Player player;

    public RectTransform selectionBox;



    private void Awake()
    {
        selectedUnitsList = new List<GameObject>();
        playerUnits = player.getUnits();

    }

   

    void Update()

    {
        if (Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.currentSelectedGameObject == null && selectedBuilding != null)
            {
                selectedBuilding.GetComponent<BuildingScript>().isClicked = false;
                player.GetComponent<UIController>().DeactivateUI(selectedBuilding);
                selectedBuilding = null;
            }


            startPosition = Grid.GetMouseWorldPosition();
            startUIPosition = Input.mousePosition;
            foreach (GameObject unit in selectedUnitsList)
            {

                //izklopi indikator enotam, ki so bile prej izbrane
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                player.GetComponent<UIController>().VillagerUI.gameObject.SetActive(false);
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            player.GetComponent<UIController>().VillagerUI.gameObject.SetActive(false);
            selectedUnitsList.Clear();


            endPosition = Grid.GetMouseWorldPosition();
            ReleaseSelectionBox();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.transform.tag == "Villager")
                {
                    selectedUnitsList.Clear();
                    selectedUnitsList.Add(hit.collider.gameObject);

                    player.GetComponent<UIController>().VillagerUI.gameObject.SetActive(true);

                }
                else if (hit.collider.transform.tag == "Knight" || hit.collider.transform.tag == "Soldier")
                {
                    selectedUnitsList.Clear();
                    selectedUnitsList.Add(hit.collider.gameObject);

                }
                else if ((hit.collider.transform.tag == "Stables" || hit.collider.transform.tag == "Townhall" || hit.collider.transform.tag == "Barracks") && selectedUnitsList.Count == 0)
                {
                    selectedBuilding = hit.collider.gameObject;
                    selectedBuilding.GetComponent<BuildingScript>().isClicked = true;
                    player.GetComponent<UIController>().ActivatuUI(selectedBuilding);
                }
            }

            


        }
        if (Input.GetMouseButton(0))
        {

            UpdateSelectionBox(Input.mousePosition);
        }
     

        if (Input.GetMouseButton(1))
        {
            
            
            foreach (GameObject unit in selectedUnitsList)
            {
                unit.GetComponent<Unit>().TakeAction();
            }

        }

        if (Input.GetKeyDown("escape"))
        {
            player.GetComponent<UIController>().VillagerUI.enabled = false;
            selectedBuilding.GetComponent<BuildingScript>().isClicked = false;
            player.GetComponent<UIController>().DeactivateUI(selectedBuilding);
            selectedBuilding = null;
            foreach (GameObject unit in playerUnits)
            {
                selectedUnitsList.Clear();
            }
        }
        foreach (GameObject unit in playerUnits)
        {
            unit.GetComponent<Unit>().isSelected = false;
        }
        foreach (GameObject unit in selectedUnitsList)
        {
            unit.GetComponent<Unit>().isSelected = true;
        }
        if(selectedUnitsList.Count == 1 && selectedUnitsList[0].tag == "Villager")
        {
            player.GetComponent<UIController>().VillagerUI.gameObject.SetActive(true);
        }



    }

    private bool IsInSelectionBox(GameObject unit)
    {
        float x = unit.transform.position.x;
        float z = unit.transform.position.z;
        if ((x > startPosition.x && x < endPosition.x && z < startPosition.z && z > endPosition.z)
            || (x > startPosition.x && x < endPosition.x && z > startPosition.z && z < endPosition.z)
            || (x < startPosition.x && x > endPosition.x && z < startPosition.z && z > endPosition.z)
            || (x < startPosition.x && x > endPosition.x && z > startPosition.z && z < endPosition.z))
        {
            return true;
        }

        return false;
    }

    void UpdateSelectionBox(Vector2 currentMousePosition)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }
        float width = currentMousePosition.x - startUIPosition.x;
        float height = currentMousePosition.y - startUIPosition.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startUIPosition + new Vector2(width / 2, height / 2);
    }

    

    public void SpawnVillager()
    {
        if(selectedBuilding.GetComponent<BuildingScript>().isBuilt && player.food >= 100 && player.currentPopulation < player.maxPopulation)
        {
            GameObject spawnedunit = Instantiate(player.getAvailibleUnits()[0], new Vector3(selectedBuilding.transform.position.x + 10f, selectedBuilding.transform.position.y, selectedBuilding.transform.position.z), Quaternion.Euler(0, 0, 0));
            playerUnits.Add(spawnedunit);
            player.food -= 100;
        }
        else
        {
            Debug.Log("Zgradba še ni zgrajena!");
        }


    }

    public void SpawnSoldier()
    {
        if (selectedBuilding.GetComponent<BuildingScript>().isBuilt && player.gold >= 50 && player.food >= 150 && player.currentPopulation < player.maxPopulation)
        {
            GameObject spawnedunit = Instantiate(player.getAvailibleUnits()[1], new Vector3(selectedBuilding.transform.position.x + 5f, selectedBuilding.transform.position.y, selectedBuilding.transform.position.z), Quaternion.Euler(0, 0, 0));
            playerUnits.Add(spawnedunit);
            player.gold -= 50;
            player.food -= 150;
        }
        else
        {
            Debug.Log("Zgradba še ni zgrajena!");
        }


    }

    public void SpawnKnight()
    {
        if (selectedBuilding.GetComponent<BuildingScript>().isBuilt && player.gold >= 100 && player.food >= 200 && player.wood >= 50 && player.currentPopulation < player.maxPopulation)
        {
            GameObject spawnedunit = Instantiate(player.getAvailibleUnits()[2], new Vector3(selectedBuilding.transform.position.x + 10f, selectedBuilding.transform.position.y, selectedBuilding.transform.position.z), Quaternion.Euler(0, 0, 0));
            playerUnits.Add(spawnedunit);
            player.food -= 200;
            player.gold -= 100;
            player.wood -= 50;
        }
        else
        {
            Debug.Log("Zgradba še ni zgrajena!");
        }
        
    }

    public void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach(GameObject unit in playerUnits)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if(screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                selectedUnitsList.Add(unit);
            }
        }

    }


}
