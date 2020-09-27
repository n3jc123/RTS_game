using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    public bool goldAcquired;
    public bool woodAcquired;

    int targetIndex = 0;
    List<Vector3> newPath;

    List<GameObject> warehouses = new List<GameObject>();
    List<GameObject> golds = new List<GameObject>();
    List<GameObject> forrests  = new List<GameObject>();

    GameObject closestGold;
    GameObject closestWood;
    GameObject closestWarehouse;
    EnemyScript enemy;

    ResState currentState;
    ResState targetState;

    bool gatheringGold = false;
    bool gatheringWood = false;
    bool moveComplete = true;
    public bool moving = false;

    GridC grid;

    public void Start()
    {
        //Debug.Log(GameObject.FindGameObjectsWithTag("Warehouse").Length);
        warehouses.AddRange(GameObject.FindGameObjectsWithTag("Warehouse"));
        golds.AddRange(GameObject.FindGameObjectsWithTag("Gold"));
        forrests.AddRange(GameObject.FindGameObjectsWithTag("Forrest"));
        enemy = GameObject.Find("EnemyPlayer").GetComponent<EnemyScript>();

        targetState = new ResState(0, 0, 0);
        currentState = new ResState(enemy.goldAmount, enemy.woodAmount, enemy.villagersDiff);
        //currentState.PrintInfo();
        grid = GameObject.Find("RTA").GetComponent<GridC>();

    }


    public void Update()
    {
        /*
        if(enemy.woodAmount < 4)
        {
            gatheringGold = false;
            gatheringWood = true;
        }
        else if(enemy.goldAmount < 3)
        {
            gatheringWood = false;
            gatheringGold = true;
        }

        if(gatheringGold)
        {
            GoldGatherMove();
        }
        if(gatheringWood)
        {
            WoodGatherMove();
        }
        
        if(Input.GetKeyDown("space"))
        {
            gatheringGold = false;
            gatheringWood = false;
        }
        if (Input.GetKeyDown("up"))
        {
            gatheringWood = false;
            gatheringGold = true;
        }
        if (Input.GetKeyDown("down"))
        {
            gatheringGold = false;
            gatheringWood = true;
        }*/
        List<ResState> states = new List<ResState>();

        if (moveComplete)
        {
            if(currentState != null)
            {
                states = GameObject.Find("RTA").GetComponent<AStarRes>().FindPath(currentState, targetState);
            }
            
            
            if(states != null && states.Count > 0)
            {
                currentState = states[0];
                Debug.Log(currentState.move);
            }
            else
            {
                currentState = null;
            }
            
                
        }
        
        if (currentState != null && currentState.move == "gatherGold")
        {
            gatheringGold = true;
            gatheringWood = false;
            GoldGatherMove();
        }
        if(currentState != null && currentState.move == "gatherWood")
        {
            gatheringWood = true;
            gatheringGold = false;
            WoodGatherMove();
        }
        if(currentState != null && currentState.move == "spawnVillager")
        {
            gatheringWood = true;
            gatheringGold = false;
            SpawnVillager();
        }
        

    }

    public void OnPathFound(List<NodeC> path)
    {
        newPath = new List<Vector3>();

        foreach(NodeC node in path)
        {
            newPath.Add(node.worldPosition);
        }
        
        targetIndex = 0;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
        
    }
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = newPath[targetIndex];
   
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= newPath.Count)
                {
                    moving = false;
                    yield break;
                }
                currentWaypoint = newPath[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, 30 * Time.deltaTime);
            yield return null;

        }
    }

    IEnumerator GoldRoutine()
    {
        List<NodeC> path = new List<NodeC>();
        if (!goldAcquired)
        {
            path = GameObject.Find("RTA").GetComponent<AStar>().FindPath(transform.position, closestGold.transform.position);
            OnPathFound(path);
        }
        //Debug.Log(Vector3.Distance(transform.position, closestGold.transform.position));
        yield return null;
    }

    public void GoToGold()
    {
        if(!goldAcquired && !moving && Vector3.Distance(GetClosestGold().transform.position, transform.position) >= 15f)
        {
            closestGold = GetClosestGold();
            StopCoroutine("GoldRoutine");
            StartCoroutine("GoldRoutine");
            moving = true;
        }
        
    }

    public void GoToWarehouse()
    {
        if((goldAcquired || woodAcquired) && !moving && Vector3.Distance(GetClosestWarehouse(closestGold).transform.position, transform.position) >= 15f)
        {
            closestWarehouse = GetClosestWarehouse(closestGold);
            StopCoroutine("WarehouseRoutine");
            StartCoroutine("WarehouseRoutine");
            moving = true;
        }
        
    }

    IEnumerator WarehouseRoutine()
    {
        List<NodeC> path = new List<NodeC>();
        if (goldAcquired || woodAcquired)
        {
            path = GameObject.Find("RTA").GetComponent<AStar>().FindPath(transform.position, closestWarehouse.transform.position);
            OnPathFound(path);
        }
        //Debug.Log(Vector3.Distance(transform.position, closestWarehouse.transform.position));
        yield return null;
    }

    public void GatherGold()
    {
        if(Vector3.Distance(GetClosestGold().transform.position, transform.position) <= 15f && !goldAcquired)
        {
            goldAcquired = true;
        }
    }

    public void ReturnGold()
    {
        if (Vector3.Distance(GetClosestWarehouse(closestGold).transform.position, transform.position) <= 15f && goldAcquired)
        {
            goldAcquired = false;
            GameObject.Find("EnemyPlayer").GetComponent<EnemyScript>().goldAmount++;
            moveComplete = true;
        }
    }

    public GameObject GetClosestWarehouse(GameObject resource)
    {
        GameObject closestWarehouse = warehouses[0];
        float minDist = float.MaxValue;
        foreach(GameObject ware in warehouses)
        {
            if(Vector3.Distance(ware.transform.position, resource.transform.position) < minDist)
            {
                minDist = Vector3.Distance(ware.transform.position, resource.transform.position);
                closestWarehouse = ware;
            }

        }
        return closestWarehouse;
    }

    public GameObject GetClosestGold()
    {
        closestGold = golds[0];
        float minDist = float.MaxValue;
        foreach (GameObject gold in golds)
        {
            if (Vector3.Distance(gold.transform.position, transform.position) < minDist)
            {
                minDist = Vector3.Distance(gold.transform.position, transform.position);
                closestGold = gold;
            }

        }
        return closestGold;
    }

    public void GoldGatherMove()
    {
        moveComplete = false;
        GoToGold();
        GatherGold();
        GoToWarehouse();
        ReturnGold();
    }

    public void WoodGatherMove()
    {
        moveComplete = false;
        GoToWood();
        GatherWood();
        GoToWarehouse();
        ReturnWood();
    }

    public void BarracksBuildMove()
    {
        moveComplete = false;
        CheckWhereToBuild();
        GoToBuild();
        BuildBarracks();

    }

    public void SpawnVillager()
    {
        enemy.SpawnVillager();
    }

    public void GoToWood()
    {
        if (!woodAcquired && !moving && Vector3.Distance(GetClosestWood().transform.position, transform.position) >= 15f)
        {
            closestGold = GetClosestGold();
            StopCoroutine("WoodRoutine");
            StartCoroutine("WoodRoutine");
            moving = true;
        }
    }

    public GameObject GetClosestWood()
    {
        closestWood = forrests[0];
        float minDist = float.MaxValue;
        foreach (GameObject wood in forrests)
        {
            if (Vector3.Distance(wood.transform.position, transform.position) < minDist)
            {
                minDist = Vector3.Distance(wood.transform.position, transform.position);
                closestGold = wood;
            }

        }
        return closestWood;
    }

    public void GatherWood()
    {
        if (Vector3.Distance(GetClosestWood().transform.position, transform.position) <= 15f && !woodAcquired)
        {
            woodAcquired = true;
        }
    }

    public void ReturnWood()
    {
        if (Vector3.Distance(GetClosestWarehouse(closestWood).transform.position, transform.position) <= 15f && woodAcquired)
        {
            woodAcquired = false;
            GameObject.Find("EnemyPlayer").GetComponent<EnemyScript>().woodAmount++;
            moveComplete = true;
        }
    }

    IEnumerator WoodRoutine()
    {
        List<NodeC> path = new List<NodeC>();
        if (!woodAcquired)
        {
            path = GameObject.Find("RTA").GetComponent<AStar>().FindPath(transform.position, closestWood.transform.position);
            OnPathFound(path);
        }
        
        yield return null;
    }

    public void CheckWhereToBuild()
    {
        NodeC warehouseNode = grid.NodeFromWorldPoint(warehouses[0].transform.position);
        List<NodeC> neighbours = grid.GetNeighbours(warehouseNode);
    }

    public void GoToBuild()
    {

    }

    public void BuildBarracks()
    {

    }

    
}
