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

    private Vector2 endUIPosition;

    private List<GameObject> selectedUnitsList;

    private GameObject selectedBuilding;

    private List<GameObject> playerUnits;

    private Villager vila;

    private List<Villager> playerVillagers;

    private List<Villager> selectedVillagers;

    public Player player;

    public RectTransform selectionBox;



    private Coroutine co;


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
                selectedBuilding = null;
            }


            startPosition = Grid.GetMouseWorldPosition();
            startUIPosition = Input.mousePosition;
            foreach (GameObject unit in selectedUnitsList)
            {

                //izklopi indikator enotam, ki so bile prej izbrane
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                player.GetComponent<UIController>().VillagerUI.enabled = false;
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            player.GetComponent<UIController>().VillagerUI.enabled = false;
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

                    player.GetComponent<UIController>().VillagerUI.enabled = true;

                }
                else if (hit.collider.transform.tag == "Knight")
                {
                    selectedUnitsList.Clear();
                    selectedUnitsList.Add(hit.collider.gameObject);

                }
                else if (hit.collider.transform.tag == "Soldier")
                {
                    selectedUnitsList.Clear();
                    selectedUnitsList.Add(hit.collider.gameObject);

                }
                else if ((hit.collider.transform.tag == "Stables" || hit.collider.transform.tag == "Townhall" || hit.collider.transform.tag == "Barracks") && selectedUnitsList.Count == 0)
                {
                    selectedBuilding = hit.collider.gameObject;
                    selectedBuilding.GetComponent<BuildingScript>().isClicked = true;

                }
            }

            


        }
        if (Input.GetMouseButton(0))
        {

            UpdateSelectionBox(Input.mousePosition);
        }
     

        if (Input.GetMouseButton(1))
        {
            Vector3 destination = Grid.GetMouseWorldPosition();

            foreach (GameObject unit in selectedUnitsList)
            {
                co = StartCoroutine(MoveUnit(unit, destination));
            }

        }

        if (Input.GetKeyDown("escape"))
        {
            player.GetComponent<UIController>().VillagerUI.enabled = false;
            selectedBuilding.GetComponent<BuildingScript>().isClicked = false;
            selectedBuilding = null;
            foreach (GameObject unit in playerUnits)
            {
                selectedUnitsList.Clear();
            }
        }
        foreach (GameObject unit in playerUnits)
        {
            unit.GetComponent<Villager>().isSelected = false;
        }
        foreach (GameObject unit in selectedUnitsList)
        {
            unit.GetComponent<Villager>().isSelected = true;
        }
        if(selectedUnitsList.Count == 1 && selectedUnitsList[0].tag == "Villager")
        {
            player.GetComponent<UIController>().VillagerUI.enabled = true;
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

    IEnumerator MoveUnit(GameObject unit, Vector3 destination)
    {
        while (destination.x != unit.transform.position.x || destination.z != unit.transform.position.z)
        {


            float step = 5f * Time.deltaTime;
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, destination, step);
            yield return null;


        }


    }

    public void SpawnVillager()
    {
        GameObject spawnedunit = Instantiate(player.getAvailibleUnits()[0], new Vector3(selectedBuilding.transform.position.x + 10f, selectedBuilding.transform.position.y, selectedBuilding.transform.position.z), Quaternion.Euler(0, 0, 0));
        playerUnits.Add(spawnedunit); 
    }

    public void SpawnSoldier()
    {
        GameObject spawnedunit = Instantiate(player.getAvailibleUnits()[1], new Vector3(selectedBuilding.transform.position.x + 5f, selectedBuilding.transform.position.y, selectedBuilding.transform.position.z), Quaternion.Euler(0, 0, 0));
        playerUnits.Add(spawnedunit);
    }

    public void SpawnKnight()
    {
        GameObject spawnedunit = Instantiate(player.getAvailibleUnits()[2], new Vector3(selectedBuilding.transform.position.x + 10f, selectedBuilding.transform.position.y, selectedBuilding.transform.position.z), Quaternion.Euler(0, 0, 0));
        playerUnits.Add(spawnedunit);
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
