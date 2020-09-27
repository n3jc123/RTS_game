using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarRes : MonoBehaviour
{
	GridC grid;

	void Awake()
	{
		grid = GetComponent<GridC>();
	}

    void Update()
    {
        
    }



    public List<ResState> FindPath(ResState startState, ResState targetState)
	{

		List<ResState> openSet = new List<ResState>();
		HashSet<ResState> closedSet = new HashSet<ResState>();
		openSet.Add(startState);

		int limit = 0;
		if (startState == null || targetState == null)
			return null;
		while (openSet.Count > 0)
		{
			ResState node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}
			//node.PrintInfo();

			openSet.Remove(node);
			closedSet.Add(node);

			if (node.IsEqual(targetState))
			{
				
				//Debug.Log("path found");
				
				
				return RetracePath(startState, node);
			}
			Debug.Log("Node info: ");
			node.PrintInfo();
			Debug.Log("Neighbour info: ");
			foreach (ResState neighbour in node.GetNeighbours())
			{
				
				if (IsInClosedSet(closedSet ,neighbour))
				{
					continue;
				}

				
				int newCostToNeighbour = node.gCost + 10;
				if (newCostToNeighbour < neighbour.gCost || !IsInOpenSet(openSet, neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = neighbour.Evaluate(targetState);
					//Debug.Log(neighbour.Evaluate(targetState));
					neighbour.parent = node;
					neighbour.PrintInfo();



					if (!IsInOpenSet(openSet, neighbour))
						openSet.Add(neighbour);
				}
			}
			limit++;
		}
		return null;
	}

	List<ResState> RetracePath(ResState startNode, ResState endNode)
	{
		List<ResState> path = new List<ResState>();
		ResState currentNode = endNode;

		while (!currentNode.IsEqual(startNode))
		{
			path.Add(currentNode);
			//currentNode.PrintInfo();
			currentNode = currentNode.parent;

			
		}
		path.Reverse();

		
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

	bool IsInClosedSet(HashSet<ResState> set, ResState state)
    {
		foreach(ResState s in set)
        {
			if (s.IsEqual(state))
				return true;
        }
		return false;
    }
	bool IsInOpenSet(List<ResState> set, ResState state)
	{
		foreach (ResState s in set)
		{
			if (s.IsEqual(state))
				return true;
		}
		return false;
	}
}
