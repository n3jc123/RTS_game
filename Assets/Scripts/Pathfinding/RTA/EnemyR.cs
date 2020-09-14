using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyR : MonoBehaviour
{
    public int resources;
    public int unitsCount;
    public int allUnitsCount;
    public float enemyUnitsDistance;

    public Vector3 positionOfUnits;

    State currentState;
    /*
    PlayerR player;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerR player = GameObject.Find("Player").GetComponent<PlayerR>();
        float distanceToEnemyUnits = Vector3.Distance(positionOfUnits, player.positionOfUnits);
        currentState = new State(player.numberOfUnits, distanceToEnemyUnits, resources, allUnitsCount);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("Space"))
        {
            RTA.FindAction();
        }
    }

    public int calculateFn(State currentState, State initialState)
    {
        int gn = currentState.calculate_gn(initial_state);
        int hn = self.__evaluation_heuristic.score(state);
        return gn + hn;
 
    }
    */
}
