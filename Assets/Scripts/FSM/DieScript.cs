using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour
{
    FSM fsm;

    void Start()
    {
        fsm = gameObject.GetComponent<FSM>();
    }

    void Update()
    {
        //ce enota umre se odstrani iz seznama nasprotnikov, seznama enot igralca in seznama izbranih enot.
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
