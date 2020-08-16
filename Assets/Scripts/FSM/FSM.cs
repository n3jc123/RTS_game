
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class FSM : MonoBehaviour
{
    public int team;
    //public bool selected;

    //links to the different behaviour components
    public IdleScript idle;
    public SeekScript seek;
    public AttackScript attack;
    public BuildScript build;
    public MoveScript move;

    public bool moving;
    public bool selected;
    public bool building;
    public bool gathering;

    public UnitState state;

    public enum UnitState
    {
        Idle,
        Seek,
        Move,
        Attack,
        Build
    }
    void Start()
    {
        ChangeState(UnitState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if (state == UnitState.Idle)
                ChangeState(UnitState.Move);
        }
        else if(!moving)
        {
            ChangeState(UnitState.Idle);
        }
       
        
    }

    public void ChangeState(UnitState newState)
    {
        state = newState;
        switch(state)
        {
            case UnitState.Idle:
                if (gameObject.GetComponent<IdleScript>() == null)
                {
                    idle = gameObject.AddComponent<IdleScript>();
                }
                DestroyImmediate(seek);
                DestroyImmediate(attack);
                DestroyImmediate(build);
                DestroyImmediate(move);
                break;

            case UnitState.Seek:
                if (gameObject.GetComponent<SeekScript>() == null)
                {
                    seek = gameObject.AddComponent<SeekScript>();
                }
                DestroyImmediate(idle);
                DestroyImmediate(attack);
                DestroyImmediate(build);
                DestroyImmediate(move);
                break;

            case UnitState.Attack:
                if (gameObject.GetComponent<AttackScript>() == null)
                {
                    attack = gameObject.AddComponent<AttackScript>();
                }
                DestroyImmediate(seek);
                DestroyImmediate(idle);
                DestroyImmediate(build);
                DestroyImmediate(move);
                break;

            case UnitState.Build:
                if (gameObject.GetComponent<BuildScript>() == null)
                {
                    build = gameObject.AddComponent<BuildScript>();
                }
                DestroyImmediate(seek);
                DestroyImmediate(attack);
                DestroyImmediate(idle);
                DestroyImmediate(move);
                break;

            case UnitState.Move:
                if (gameObject.GetComponent<MoveScript>() == null)
                {
                    move = gameObject.AddComponent<MoveScript>();
                }
                DestroyImmediate(seek);
                DestroyImmediate(attack);
                DestroyImmediate(idle);
                DestroyImmediate(build);
                break;
        }
        

    }
}
