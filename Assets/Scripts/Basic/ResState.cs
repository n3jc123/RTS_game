using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResState
{

    public int goldAmount;
    public int woodAmount;
    public int woodDistance;
    public int goldDistance;
    public int villagersDiff;

    public int fCost;
    public int gCost;
    public int hCost;

    public ResState parent;
    public string move;

    public ResState(int _goldAmount, int _woodAmount, int _villagersDiff)
    {
        GameObject gold = GameObject.FindGameObjectsWithTag("Gold")[0];
        GameObject wood = GameObject.FindGameObjectsWithTag("Forrest")[0];
        GameObject warehouse = GameObject.FindGameObjectsWithTag("Warehouse")[0];
        GameObject villager = GameObject.FindGameObjectsWithTag("Villager")[0];
        


        
        goldAmount = _goldAmount;
        woodAmount = _woodAmount;
        villagersDiff = _villagersDiff;
        woodDistance = Mathf.RoundToInt(Vector3.Distance(wood.transform.position, warehouse.transform.position));
        goldDistance = Mathf.RoundToInt(Vector3.Distance(gold.transform.position, warehouse.transform.position));

  
    }

    public List<ResState> GetNeighbours()
    {
        List<ResState> neighbours = new List<ResState>();
        ResState state1 = new ResState(goldAmount + 1, woodAmount, villagersDiff);
        state1.move = "gatherGold";
        neighbours.Add(state1);
        ResState state2 = new ResState(goldAmount, woodAmount + 1, villagersDiff);
        state2.move = "gatherWood";
        neighbours.Add(state2);
        if (goldAmount >= 1 && woodAmount >= 1)
        {
            ResState state3 = new ResState(goldAmount - 1, woodAmount - 1, villagersDiff - 1);
            state3.move = "spawnVillager";
            neighbours.Add(state3);
        }

        return neighbours;
    }

    public bool IsEqual(ResState otherState)
    {
        if(goldAmount == otherState.goldAmount && woodAmount == otherState.woodAmount && villagersDiff == otherState.villagersDiff)
            return true;
        return false;
    }

    public int Evaluate(ResState targetState)
    {
        int eval = 0;
        if(villagersDiff >= 0)
        {
            eval += villagersDiff * (goldDistance + woodDistance);
        }
        
        if(goldDistance <= woodDistance && targetState.goldAmount - goldAmount >= 0)
        {
            eval += Mathf.Abs((targetState.goldAmount - goldAmount) * goldDistance + (woodDistance - goldDistance));
        }
        
        if(targetState.woodAmount - woodAmount >= 0)
        {

            eval += Mathf.Abs((targetState.woodAmount - woodAmount) * woodDistance + (goldDistance - woodDistance));
        }
        

        return eval;
        
    }

    public void PrintInfo()
    {
        Debug.Log("Gold/Wood: " + goldAmount + ", " + woodAmount + ", " + move + ", " + villagersDiff + "g/h: " + gCost + ", " + hCost);
    }


}
