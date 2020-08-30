
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FSM : MonoBehaviour
{
    public int team;
    //public bool selected;

    //links to the different behaviour components
    public IdleScript idle;
    public SeekScript seek;
    public AttackScript attack;
    public GatherScript gather;
    public BuildScript build;
    public MoveScript move;
    public DieScript die;

    public bool moving;
    public bool selected;
    public bool building;
    public bool gathering;
    public bool returningResource;
    public bool attackingBuilding;
    public bool goingToBuilding;


    public string resource;

    public int resourceAmount;
    public int health;
    public int oldHealth;
    public int attackDamage;

    public UnitState state;

    public GameObject closestEnemy;
    public GameObject targetEnemy;
    public GameObject closestBuilding;
    public GameObject targetBuilding;
    public GameObject buildingA;

    public Vector3 targetWarehouse;
    public Vector3 target;

    public Player player;
    public Player enemyPlayer;

    public List<GameObject> EnemyList;
    public List<GameObject> EnemyBuildings;

    public Image healthBar;

    public enum UnitState
    {
        Idle,
        Seek,
        Move,
        Attack,
        Build,
        Gather,
        Die
    }
    void Start()
    {
        health = 100;
        oldHealth = health;
        ChangeState(UnitState.Idle);
        
        if (team == 0)
        {
            enemyPlayer = GameObject.Find("Player2").GetComponent<Player>();
            player = GameObject.Find("Player1").GetComponent<Player>();

        }
        else
        {
            enemyPlayer = GameObject.Find("Player1").GetComponent<Player>();
            player = GameObject.Find("Player2").GetComponent<Player>();
        }
        EnemyList = enemyPlayer.getUnits();
        EnemyBuildings = enemyPlayer.getBuildings();

        if (this.tag == "Soldier")
        {
            attackDamage = 2;
        }
        else if(this.tag == "Knight")
        {
            attackDamage = 4;
        }
    }

    // Update is called once per frame
    void Update()
    {

        UpdateHealthBar();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            if ((hit.collider.transform.tag == "Villager" || hit.collider.transform.tag == "Soldier" || hit.collider.transform.tag == "Knight") && Input.GetMouseButtonUp(1))
            {
                targetEnemy = hit.collider.gameObject;
            }
            
            if ((hit.collider.transform.tag == "Stables" || hit.collider.transform.tag == "House" || hit.collider.transform.tag == "Warehouse"
                || hit.collider.transform.tag == "Barracks" || hit.collider.transform.tag == "Townhall") && Input.GetMouseButtonUp(1) && selected && team != hit.collider.gameObject.GetComponent<BuildingScript>().team)
            {
                Debug.Log("lololololo");
                targetBuilding = hit.collider.gameObject;
                target = new Vector3(targetBuilding.transform.position.x, targetBuilding.transform.position.y, targetBuilding.transform.position.z + 7);
                goingToBuilding = true;
                
            }
            
        }
        if(Input.GetMouseButtonDown(1))
        {
            attackingBuilding = false;
        }

        
        if ((Input.GetMouseButtonUp(1) && selected && !attackingBuilding) || resourceAmount == 12 || returningResource || goingToBuilding)
        {
            Debug.Log("nekaneakajnekaj");
            building = false;
            ChangeState(UnitState.Move);
        }
        else if (health < 1)
        {
            ChangeState(UnitState.Die);
        }
        

        else if ((this.tag == "Knight" || this.tag == "Soldier") && !moving && (closestEnemy != null || attackingBuilding))// || targetBuilding != null))
        {
            ChangeState(UnitState.Attack);
        }

        else if ((this.tag == "Knight" || this.tag == "Soldier") && !moving && !selected)
        {
            ChangeState(UnitState.Seek);
        }

        else if(!moving && !gathering && !building)
        {
            ChangeState(UnitState.Seek);
        }
        else if(!moving && gathering && !building)
        {
            ChangeState(UnitState.Gather);
        }
        else if (building)
        {
            ChangeState(UnitState.Build);
        }




        //indicator
        if (selected)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
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
                DestroyImmediate(gather);
                DestroyImmediate(die);
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
                DestroyImmediate(gather);
                DestroyImmediate(die);
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
                DestroyImmediate(gather);
                DestroyImmediate(die);
                break;

            case UnitState.Build:
                if (gameObject.GetComponent<BuildScript>() == null)
                {
                    build = gameObject.AddComponent<BuildScript>();
                }
                
                DestroyImmediate(seek);
                DestroyImmediate(attack);
                DestroyImmediate(idle);
                DestroyImmediate(gather);
                DestroyImmediate(move);
                DestroyImmediate(die);
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
                DestroyImmediate(gather);
                DestroyImmediate(die);
                break;

            case UnitState.Gather:
                if (gameObject.GetComponent<GatherScript>() == null)
                {
                    gather = gameObject.AddComponent<GatherScript>();
                }
                DestroyImmediate(seek);
                DestroyImmediate(attack);
                DestroyImmediate(idle);
                DestroyImmediate(build);
                DestroyImmediate(move);
                DestroyImmediate(die);
                break;

            case UnitState.Die:
                if (gameObject.GetComponent<DieScript>() == null)
                {
                    die = gameObject.AddComponent<DieScript>();
                }
                DestroyImmediate(seek);
                DestroyImmediate(attack);
                DestroyImmediate(idle);
                DestroyImmediate(build);
                DestroyImmediate(move);
                DestroyImmediate(gather);
                break;
        }
        

    }

    private void UpdateHealthBar()
    {
        if (health == 100)
        {
            healthBar.enabled = false;
        }
        else
        {
            healthBar.enabled = true;

        }

        healthBar.fillAmount = health / 100f;
    }
    
}
