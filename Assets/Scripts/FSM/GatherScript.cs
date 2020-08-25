using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherScript : MonoBehaviour
{
    FSM fsm;
    Player player1;
    Player player2;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        fsm = gameObject.GetComponent<FSM>();
        player1 = GameObject.Find("Player1").GetComponent<Player>();
        player2 = GameObject.Find("Player2").GetComponent<Player>();
        timer = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(fsm.resource == "Forrest")
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
