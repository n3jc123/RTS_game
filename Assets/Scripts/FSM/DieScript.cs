using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour
{
    FSM fsm;
    // Start is called before the first frame update
    void Start()
    {
        fsm = gameObject.GetComponent<FSM>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fsm.health < 1)
        {
            fsm.EnemyList.Remove(this.gameObject);
            if (fsm.team == 0)
            {
                GameObject.Find("Player1").GetComponent<Player>().unitsList.Remove(this.gameObject);
                GameObject.Find("GameRTSController").GetComponent<GameRTSController>().selectedUnitsList.Remove(this.gameObject);
            }
            else
            {
                GameObject.Find("Player2").GetComponent<Player>().unitsList.Remove(this.gameObject);
                GameObject.Find("GameRTSController").GetComponent<GameRTSController>().selectedUnitsList.Remove(this.gameObject);
            }

            DestroyImmediate(gameObject);
        }
    }
}
