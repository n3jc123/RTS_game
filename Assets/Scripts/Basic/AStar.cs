using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour
{

	GridC grid;

	void Awake()
	{
		grid = GetComponent<GridC>();
	}

	

	public List<NodeC> FindPath(Vector3 startPos, Vector3 targetPos)
	{
		NodeC startNode = grid.NodeFromWorldPoint(startPos);
		NodeC targetNode = grid.NodeFromWorldPoint(targetPos);

		if(!targetNode.walkable)
        {
			float minDistance = float.MaxValue;
			foreach (NodeC n in grid.GetNeighbours(targetNode))
            {
				
				if(n.walkable && GetDistance(startNode, n) < minDistance)
                {
					minDistance = GetDistance(startNode, n);
					targetNode = n;
                }
            }
        }

		List<NodeC> openSet = new List<NodeC>();
		HashSet<NodeC> closedSet = new HashSet<NodeC>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			NodeC node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
			{
				//Debug.Log("path found");
				return RetracePath(startNode, targetNode);
			}

			foreach (NodeC neighbour in grid.GetNeighbours(node))
			{
				if (!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
		return null;
	}

	List<NodeC> RetracePath(NodeC startNode, NodeC endNode)
	{
		List<NodeC> path = new List<NodeC>();
		NodeC currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;
		return path;

	}

	

	int GetDistance(NodeC nodeA, NodeC nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}

/*
void Awake()
{
	grid = GetComponent<GridB>();
	villagerScript = villager.GetComponent<Villager>();


}


void Update()
{
	if(Input.GetKeyDown("space"))
	{
		State startState = new State(villagerScript.goldAcquired, villagerScript.woodAcquired, grid.NodeFromWorldPoint(gold.transform.position), grid.NodeFromWorldPoint(wood.transform.position),
								grid.NodeFromWorldPoint(warehouse.transform.position), 0, 0, grid.NodeFromWorldPoint(villager.transform.position));

		State targetState = new State(false, false, grid.NodeFromWorldPoint(gold.transform.position), grid.NodeFromWorldPoint(wood.transform.position),
								grid.NodeFromWorldPoint(warehouse.transform.position), 1, 0, grid.NodeFromWorldPoint(villager.transform.position));

		StartCoroutine(FindPath(startState, targetState));

	}

}

IEnumerator FindPath(State startState, State targetState)
{


	//State targetState = new State(false, false, 90f, 50f, 10f, 2, 5);
	List<State> openSet = new List<State>();
	HashSet<State> closedSet = new HashSet<State>();
	openSet.Add(startState);

	while (openSet.Count > 0 && !pathFound)
	{
		State state = openSet[0];
		Debug.Log(state.gCost + "  " + state.hCost + "  " + state.fCost);
		for (int i = 1; i < openSet.Count; i++)
		{
			if (openSet[i].fCost < state.fCost || openSet[i].fCost == state.fCost)
			{
				if (openSet[i].hCost < state.hCost)
					state = openSet[i];
			}
		}

		openSet.Remove(state);
		closedSet.Add(state);

		if (state.goldAmount == targetState.goldAmount && state.woodAmount == targetState.woodAmount)
		{
			//RetracePath(startstate, targetstate);
			pathFound = true;
		}
		Debug.Log("current state: ");
		state.StateInfo();
		Debug.Log("Sosedi: ");
		foreach (State neighbour in GetStates(state))
		{

			if (!neighbour.walkable || closedSet.Contains(neighbour))
			{
				continue;
			}

			int newCostToNeighbour = state.gCost + 10; //cena vsake poteze je 10? tolko stane en premik in ker tudi nabiranje pobere eno potezo je najbolj smiselno da je tudi cena nabiranja enaka
			if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
			{


				neighbour.gCost += newCostToNeighbour;
				neighbour.hCost = state.evaluate(neighbour, targetState);
				neighbour.parent = state;
				neighbour.StateInfo();


				if (!openSet.Contains(neighbour))
					openSet.Add(neighbour);
			}
		}

		//villager.GetComponent<Villager>().MakeAction(state);

		Debug.Log("states move: " + state.move);
		villager.transform.position = state.worldPosition;
		yield return new WaitForSeconds(0.5f);
	}

}

State FindNextMove(State state)
{
	State bestState = state;
	int minFCost = 10000;
	foreach (State neighbour in GetStates(state))
	{

			neighbour.gCost += 10;
			neighbour.hCost = state.evaluate(state, neighbour);
			neighbour.parent = state;
			neighbour.StateInfo();
		if(neighbour.fCost < minFCost)
		{
			bestState = neighbour;
		}

	}

	villager.transform.position = bestState.worldPosition;
	return bestState;


}

void RetracePath(Node startNode, Node endNode)
{
	List<Node> path = new List<Node>();
	Node currentNode = endNode;

	while (currentNode != startNode)
	{
		path.Add(currentNode);
		currentNode = currentNode.parent;
	}
	path.Reverse();

	//grid.path = path;

}

int GetDistance(GameObject object1, GameObject object2)
{
	NodeB nodeA = grid.NodeFromWorldPoint(object1.transform.position);
	NodeB nodeB = grid.NodeFromWorldPoint(object2.transform.position);

	int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
	int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

	return 10 * (dstX + dstY);
}
bool GetValidMoves()
{

	return false;
}
List<State> GetStates(State state)
{

	NodeB[,] temGrid = grid.getGrid();
	List<State> validStates = new List<State>();

	if (state.gridX > 0 && temGrid[state.gridX - 1, state.gridY].walkable)
		validStates.Add(state.GetNewState("left"));

	if (state.gridX < temGrid.GetLength(1) - 1 && temGrid[state.gridX + 1, state.gridY].walkable)
		validStates.Add(state.GetNewState("right"));

	if (state.gridY > 0 && temGrid[state.gridX, state.gridY - 1].walkable)
		validStates.Add(state.GetNewState("down"));

	if (state.gridY < temGrid.GetLength(0) - 1 && temGrid[state.gridX, state.gridY + 1].walkable)
		validStates.Add(state.GetNewState("up"));

	if (!state.goldAcquired && !state.woodAcquired && state.distanceToGold < 11f)
		validStates.Add(state.GetNewState("gatherGold"));

	if (!state.woodAcquired && !state.goldAcquired && state.distanceToWood < 11f)
		validStates.Add(state.GetNewState("gatherWood"));

	if (state.goldAcquired && !state.woodAcquired && state.distanceToWarehouse < 11f)
		validStates.Add(state.GetNewState("returnGold"));

	if (state.woodAcquired && !state.goldAcquired && state.distanceToWarehouse < 11f)
		validStates.Add(state.GetNewState("returnWood"));

	return validStates;
}		 
}
*/
