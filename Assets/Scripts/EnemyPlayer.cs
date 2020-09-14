using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : MonoBehaviour
{

    public Player player;
    public List<GameObject> unitsList;
    public bool attacked;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Player>();
        unitsList = player.getUnits();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.getUnits().Count > 10 && !attacked)
        {
            AttackPlayerBase();
            attacked = true;
        }
        

    }

    public void AttackPlayerBase()
    {
        Vector3[] positionsList = GameObject.Find("GameRTSController").GetComponent<GameRTSController>().CreatePositions(new Vector3(-25, 0, -25), unitsList.Count);
        int index = 0;
        foreach (GameObject unit in unitsList)
        {

            unit.GetComponent<FSM>().target = positionsList[index];
            unit.GetComponent<FSM>().test = true;
            index++;
           
        }
    }
}
