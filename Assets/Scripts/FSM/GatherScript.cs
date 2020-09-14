using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherScript : MonoBehaviour
{
    FSM fsm;

    float timer;

    void Start()
    {
        fsm = gameObject.GetComponent<FSM>();
        timer = 0.5f;
    }

    void Update()
    {
        GatherResource();
    }


    //doda resource v katerem je villager trenutno
    private void GatherResource()
    {
        if (fsm.resource == "Forrest")
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0.5f;
                fsm.resourceAmount++;

            }
        }
        else if (fsm.resource == "Gold")
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0.5f;
                fsm.resourceAmount++;

            }
        }
        else if (fsm.resource == "Stone")
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0.5f;
                fsm.resourceAmount++;

            }
        }
        else if (fsm.resource == "Farm")
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0.5f;
                fsm.resourceAmount++;

            }
        }
    }
}
