using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RTA
{

	GridR grid;

	/*
	public void FindAction(State startState, State targetState)
	{

		List<State> openSet = new List<State>();
		HashSet<State> closedSet = new HashSet<State>();
		openSet.Add(startState);

		while (openSet.Count > 0)
		{
			State node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].gCost < node.gCost || openSet[i].Evaluate() == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (NodeR neighbour in grid.GetNeighbours(node))
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
	}

	void RetracePath(NodeR startNode, NodeR endNode)
	{
		List<NodeR> path = new List<NodeR>();
		NodeR currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	int GetDistance(NodeR nodeA, NodeR nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
	*/
	
}
