using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public GameObject villager;

	public int gCost;
	public int hCost;
	public TextMesh text;

	public bool goldAcquired;
	public bool woodAcquired;
	public int distanceToGold;
	public int distanceToWood;
	public int distanceToWarehouse;
	public int woodAmount;
	public int goldAmount;

	public NodeB goldNode;
	public NodeB woodNode;
	public NodeB warehouseNode;

	public string move;

	public State parent;
	public List<State> children;



	public State(bool _goldAcquired, bool _woodAcuired, NodeB _goldNode, NodeB _woodNode, NodeB _warehouseNode, int _goldAmount, int _woodAmount, NodeB node)
	{

		goldAcquired = _goldAcquired;
		goldNode = _goldNode;
		woodNode = _woodNode;
		warehouseNode = _warehouseNode;
		distanceToGold = GetDistance(node, _goldNode);
		distanceToWood = GetDistance(node, _woodNode);
		distanceToWarehouse = GetDistance(node, _warehouseNode);
		woodAmount = _woodAmount;
		goldAmount = _goldAmount;
		gridX = node.gridX;
		gridY = node.gridY;
		worldPosition = node.worldPosition;
		walkable = node.walkable;
	}

	public int evaluate(State state1, State state2)
    {
		int evaluation = 1000;
		int goldNeeded = state2.goldAmount - state1.goldAmount;
		int woodNeeded = state2.woodAmount - state1.woodAmount;
		//return goldNeeded* Mathf.RoundToInt(0.5f * state1.distanceToWarehouse + state1.distanceToGold) + woodNeeded * Mathf.RoundToInt(0.5f * state1.distanceToWarehouse + state1.distanceToWood);
		if((!state1.goldAcquired && state2.goldAcquired) || (!state1.woodAcquired && state2.woodAcquired))
        {
			evaluation = 0;
        }
		else if(state1.goldAmount < state2.goldAmount || state1.woodAmount < state2.woodAmount)
        {
			evaluation = 0;
        }
		else if (state1.goldAcquired && !state2.goldAcquired)
        {
			evaluation = 1000;
		}
		else if(state1.goldAcquired && state2.goldAcquired)
        {
			evaluation = goldNeeded * Mathf.RoundToInt(state2.distanceToWarehouse);
		}
		else if(!state1.goldAcquired && !state2.goldAcquired)
        {
			evaluation = goldNeeded * Mathf.RoundToInt(state2.distanceToGold);
		}
		
		return evaluation;

    }
	public int fCost
	{
		get
		{
			return gCost + hCost;
		}

	}

	public State GetNewState(string move)
    {
		State newState;


		switch (move)
        {
			case "left":
				newState = new State(goldAcquired, woodAcquired, goldNode, woodNode, warehouseNode, goldAmount, woodAmount, new NodeB(walkable, new Vector3(worldPosition.x - 10f, worldPosition.y, worldPosition.z), gridX - 1, gridY));
				newState.move = "left";
				return newState;
			case "right":
				newState = new State(goldAcquired, woodAcquired, goldNode, woodNode, warehouseNode, goldAmount, woodAmount, new NodeB(walkable, new Vector3(worldPosition.x + 10f, worldPosition.y, worldPosition.z), gridX + 1, gridY));
				newState.move = "right";
				return newState;
			case "up":
				newState = new State(goldAcquired, woodAcquired, goldNode, woodNode, warehouseNode, goldAmount, woodAmount, new NodeB(walkable, new Vector3(worldPosition.x, worldPosition.y, worldPosition.z + 10f), gridX, gridY + 1));
				newState.move = "up";
				return newState;
			case "down":
				newState = new State(goldAcquired, woodAcquired, goldNode, woodNode, warehouseNode, goldAmount, woodAmount, new NodeB(walkable, new Vector3(worldPosition.x, worldPosition.y, worldPosition.z - 10f), gridX, gridY - 1));
				newState.move = "down";
				return newState;
			case "gatherGold":
				newState = new State(true, woodAcquired, goldNode, woodNode, warehouseNode, goldAmount, woodAmount, new NodeB(walkable, new Vector3(worldPosition.x, worldPosition.y, worldPosition.z), gridX, gridY));
				newState.move = "gatherGold";
				return newState;
			case "gatherWood":
				newState = new State(goldAcquired, true, goldNode, woodNode, warehouseNode, goldAmount, woodAmount, new NodeB(walkable, new Vector3(worldPosition.x, worldPosition.y, worldPosition.z), gridX, gridY));
				newState.move = "gatherWood";
				return newState;
			case "returnGold":
				newState = new State(false, woodAcquired, goldNode, woodNode, warehouseNode, goldAmount + 1, woodAmount, new NodeB(walkable, new Vector3(worldPosition.x, worldPosition.y, worldPosition.z), gridX, gridY));
				newState.move = "returnGold";
				return newState;
			case "returnWood":
				newState = new State(goldAcquired, false, goldNode, woodNode, warehouseNode, goldAmount, woodAmount + 1, new NodeB(walkable, new Vector3(worldPosition.x, worldPosition.y, worldPosition.z), gridX, gridY));
				newState.move = "returnWood";
				return newState;
			default:
				break;
        }
		return null;
			
    }

	public void StateInfo()
    {
		Debug.Log("x: " + this.gridX + "y: " + this.gridY + "Dist Gold: " + distanceToGold + "Dist Wood: " + distanceToWood + "Dist Ware: " + distanceToWarehouse + "g/h/f: " + gCost + ", " + hCost + ", " + fCost + "walkable: " + walkable + "Gold Acquired: " + goldAcquired + "Gold Amount: " + goldAmount);
    }
	int GetDistance(NodeB nodeA, NodeB nodeB)
	{

		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		return 10 * (dstX + dstY);
	}

    public bool isEqual(State stateToCompare)
    {
		if (gridX == stateToCompare.gridX && gridY == stateToCompare.gridY && goldAcquired == stateToCompare.goldAcquired && goldAmount == stateToCompare.goldAmount && woodAcquired == stateToCompare.woodAcquired &&
			distanceToGold == stateToCompare.distanceToGold && distanceToWarehouse == stateToCompare.distanceToWarehouse && distanceToWood == stateToCompare.distanceToWood)
			return true;
		return false;
	}
}
