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

    private bool isMoving = false;

    private string name;

    private Vector3 destination;

    private Coroutine coroutine;

    


    // Start is called before the first frame update
    void Start()
    {

        name = this.gameObject.tag;
        
    }

    // Update is called once per frame
    void Update()

    {

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

        
    }

   

}
