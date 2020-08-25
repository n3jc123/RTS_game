using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleScript : MonoBehaviour
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
        
    }
}
