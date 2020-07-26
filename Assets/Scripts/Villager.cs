using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Villager : Unit
{


    

    public int gold, wood, stone, food = 0;

    private float timer = 0.5f;

    private GameObject ground;

    private bool isGathering = false;

    private bool isMoving = false;

    private string name;

    private Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        ground = GameObject.Find("Ground");
        name = this.gameObject.tag;
    }

    // Update is called once per frame
    void Update()

    {
        if(isGathering && destination != transform.position && !isMoving)
        {
            MoveUnit(destination);
            isMoving = true;
        }
        else if(isGathering && destination == transform.position)
        {
            isMoving = false;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0.5f;
                wood++;
            }
        }

        if (isSelected)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void TakeAction()
    {
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform.tag == "DenseForrest" && name == "Villager")
            {
                destination = hit.transform.position;
                isGathering = true; 
            }
            else
            {
                
                MoveUnit(Grid.GetMouseWorldPosition());
            }
        }
    }

    public void MoveUnit(Vector3 destination)
    {
        StartCoroutine(MoveCoroutine(this.gameObject, destination));
    }
    IEnumerator MoveCoroutine(GameObject unit, Vector3 destination)
    {
        while (destination.x != unit.transform.position.x || destination.z != unit.transform.position.z)
        {


            float step = 5f * Time.deltaTime;
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, destination, step);
            yield return null;


        }


    }

}
