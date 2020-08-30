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
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if(selectedBuilding != null && EventSystem.current.currentSelectedGameObject == null)
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

                    player.GetComponent<UIController>().VillagerUI.gameObject.SetActive(false);
                }
            
            

        }
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                Vector3 targetPosition = Grid.GetMouseWorldPosition();
                Vector3[] positionsList = CreatePositions(targetPosition, selectedUnitsList.Count);
                int index = 0;
                if(hit.collider.transform.tag == "Forrest" || hit.collider.transform.tag == "Stone" || hit.collider.transform.tag == "Gold" || hit.collider.transform.tag == "Farm")
                {
                    foreach (GameObject unit in selectedUnitsList)
                    {
                        unit.GetComponent<FSM>().target = targetPosition;

                        index++;
                    }
                }
                else
                {
                    foreach (GameObject unit in selectedUnitsList)
                    {
                        
                        unit.GetComponent<FSM>().target = positionsList[index];

                        index++;
                    }
                }
                
            }

        }

        if (Input.GetKeyUp("escape"))
        {
            
            player.GetComponent<UIController>().VillagerUI.gameObject.SetActive(false);
            
            if(selectedBuilding != null)
            {
                selectedBuilding.GetComponent<BuildingScript>().isClicked = false;
                player.GetComponent<UIController>().DeactivateUI(selectedBuilding);
                selectedBuilding = null;
            }
            
            
            selectedUnitsList.Clear();
            
            
        }
        else if (selectedUnitsList.Count == 1 && selectedUnitsList[0].tag == "Villager")
        {
            player.GetComponent<UIController>().VillagerUI.gameObject.SetActive(true);
            
        }
        foreach (GameObject unit in playerUnits)
        {
            unit.GetComponent<FSM>().selected = false;
        }
        foreach (GameObject unit in selectedUnitsList)
        {
            unit.GetComponent<FSM>().selected = true;
            
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

    private Vector3[] CreatePositions(Vector3 startPosition, int positionCount)
    {
        int x = (int)Math.Ceiling(Math.Log(positionCount, 2));
        int y = x;
        Vector3[] positionList = new Vector3[x * y];
        if (positionCount > 2)
        {


            

            
            Debug.Log(Math.Ceiling(Math.Log(positionCount, 2)));
            int index = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Vector3 position = new Vector3(startPosition.x + i * 2, startPosition.y, startPosition.z + j * 2);
                    positionList[index] = position;
                    index++;
                }
            }
        }
        else
        {
            positionList = new Vector3[2];
            positionList[0] = startPosition;
            positionList[1] = new Vector3(startPosition.x + 1, startPosition.y, startPosition.z + 1);
        }

        return positionList;
    }



}
