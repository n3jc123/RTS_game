using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : Unit
{


    

    public int gold, wood, stone, food = 0;

    private float timer = 0.5f;



    public bool isGathering = false;

    public bool isBuilding = false;

    public bool isMoving = false;

    public bool there = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {
        if(isBuilding)
        {
            
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
            isBuilding = false;
        }

        if (this.tag == "Villager")
        {
            RaycastHit rayHit;
            Vector3 rayOrigin = this.transform.position;
            Vector3 ray = this.transform.forward * 10;


            if (Physics.Raycast(rayOrigin, ray, out rayHit, ray.magnitude))
            {

                if (rayHit.transform.tag == "Townhall" || rayHit.transform.tag == "Warehouse" || rayHit.transform.tag == "House" || rayHit.transform.tag == "Barracks" || rayHit.transform.tag == "Stables")
                {
                    Debug.Log("theeereeee");
                    this.there = true;
                }
                else
                {
                    this.there = false;
                }

            }
        }
        /*
        if (isGathering && target != transform.position && !isMoving)
        {

            isMoving = true;
        }
        else if(isBuilding && !isMoving)
        {

            isMoving = true;
        }
        else if(isGathering && target == transform.position)
        {
            isMoving = false;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0.5f;
                wood++;
            }
        }
        */


    }

   

   

}
