using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeekScript : MonoBehaviour
{

    private float distance;
    private float aggroDistance;
    private float minDistance;
    
    FSM fsm;
    // Start is called before the first frame update
    void Start()
    {
        fsm = gameObject.GetComponent<FSM>();

        aggroDistance = 15f;
        minDistance = 15f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach(GameObject enemy in fsm.EnemyList)
        {
            distance = Vector3.Distance(enemy.transform.position, transform.position);
            if(distance < aggroDistance)
            {
                if(minDistance >= distance)
                {
                    minDistance = distance;
                    fsm.closestEnemy = enemy;
                }
            }
        }

        

    }
}
