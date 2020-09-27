using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int goldAmount;
    public int woodAmount;
    public int villagersDiff;

    public GameObject villager;
    public List<GameObject> villagers;
    public GameObject warehouse;

    GridC grid;
    void Awake()
    {
        
        villagers.Add(GameObject.Find("Villager"));
        grid = GameObject.Find("RTA").GetComponent<GridC>();
        
    }

    // Update is called once per frame
    public void SpawnVillager()
    {
        if(goldAmount >= 1 && woodAmount >= 1)
        {
            NodeC warehouseNode = grid.NodeFromWorldPoint(warehouse.transform.position);
            List<NodeC> neighbours = grid.GetNeighbours(warehouseNode);
            Vector3 spawnPos = Vector3.zero;
            foreach (NodeC n in neighbours)
            {
                if (n.walkable)
                {
                    spawnPos = n.worldPosition;
                    break;
                }
            }
            GameObject newVillager = Instantiate(villager, spawnPos, Quaternion.identity);
            villagers.Add(newVillager);
            goldAmount--;
            woodAmount--;
            villagersDiff--;
        }
        
    }
}
