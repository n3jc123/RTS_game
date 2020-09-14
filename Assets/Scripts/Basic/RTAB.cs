using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;

public class RTAB : MonoBehaviour
{

	public Transform seeker, target;
	public bool pathFound;
	GridB grid;

	void Awake()
	{
		grid = GetComponent<GridB>();
	}

	void Update()
	{

		if(Input.GetKeyUp("space"))
        {
			NodeB startNode = grid.NodeFromWorldPoint(seeker.position);
			NodeB targetNode = grid.NodeFromWorldPoint(target.position);
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					//grid.getGrid()[i, j].hCost = GetDistance(grid.getGrid()[i, j], targetNode);
				}
			}
			StartCoroutine(FindPath(startNode, targetNode));
		}
	}

	IEnumerator FindPath(NodeB startNode, NodeB targetNode)
	{
		
		
		
		
		UtilsClass.CreateWorldText("Target", null, targetNode.worldPosition, 20, Color.white, TextAnchor.MiddleCenter);

		List<NodeB> openSet = new List<NodeB>();
		
		openSet.Add(startNode);
		NodeB node = openSet[0];

		while (openSet.Count > 0 && !pathFound)
		{
			//izbere naslednje vozlišče za raziskovanje glede na to, katero vozlišče v open setu ima najmanjsi F cost
			
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost <= node.fCost)
				{
					node = openSet[i];
				}
			}
			

			openSet.Remove(node);
			

			//ce smo izbrali ciljno vozlisce je pot najdena
			if (node == targetNode)
			{
				//RetracePath(startNode, targetNode);
				pathFound = true;
			}

			//za izbrano vozlisce pregledamo nejgove sosede
			List<NodeB> neighbours = grid.GetNeighbours(node);
			
			foreach (NodeB neighbour in neighbours)
			{
				//ce sosed ni walkable ga preskocimo
				if (!neighbour.walkable)
				{
					continue;
				}


				int newCostToNeighbour = GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost += newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					//neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
				if(neighbour.fCost <= node.fCost)
                {
					//node = neighbour;
                }
				UtilsClass.CreateWorldText("G: " + neighbour.gCost + "\nH: " + neighbour.hCost + "\nF: " + neighbour.fCost, null, neighbour.worldPosition, 20, Color.white, TextAnchor.MiddleCenter);
			}
			node.hCost = SecondMin(neighbours);
			
			
			
			UtilsClass.CreateWorldText("G: " + node.gCost + "\nH: " + node.hCost + "\nF: " + node.fCost, null, node.worldPosition, 20, Color.white, TextAnchor.MiddleCenter);
			seeker.position = node.worldPosition;

			yield return new WaitForSeconds(0.1f);
		}
		





	}

	void RetracePath(NodeB startNode, NodeB endNode)
	{
		List<NodeB> path = new List<NodeB>();
		NodeB currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	int GetDistance(NodeB nodeA, NodeB nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}

	int SecondMin(List<NodeB> neighbors)
    {
		int min = int.MaxValue;
		int secondMin = int.MaxValue;
		foreach (NodeB node in neighbors)
        {
			if (node.fCost < min && node.fCost != 0)
			{
				Debug.Log(node.fCost);
				secondMin = min;
				min = node.fCost;
			}
			else if (node.fCost < secondMin && node.fCost != 0)
				secondMin = node.fCost;
		}
		return secondMin;
	}

	int MinminLookAhead(NodeB startNode, NodeB endNode, int limit)
    {

		List<NodeB> openSet = new List<NodeB>();
		openSet.Add(startNode);
		int max = 1000000;
		int depth = 0;

		NodeB node = openSet[0];
		for (int i = 1; i < openSet.Count; i++)
		{
			if (openSet[i].fCost <= node.fCost)
			{
				node = openSet[i];
			}
		}

		while (openSet.Count > 0 && depth < limit)
		{

			
			openSet.Remove(node);

			List<NodeB> neighbors = grid.GetNeighbours(node);
			foreach (NodeB neighbour in neighbors)
			{
				if (!neighbour.walkable)
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, endNode);
					if(neighbour.fCost >= max)
                    {
						continue;
					}
                    if(neighbour.fCost < max )
                    {
						neighbour.parent = node;
						if(neighbour.fCost < max)
                        {
							if (depth == limit || neighbour == endNode)
								max = neighbour.fCost;
                        }
                    }
                    else
                    {
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
			depth++;

		}
		return max;
	}

	int MiniMin(NodeB node, NodeB targetNode, int depth)
    {
		if (depth == 0 || node == targetNode)
        {
			return node.hCost;
		}

		int minEval = int.MaxValue;
		foreach(NodeB neighbour in grid.GetNeighbours(node))
        {
			if (!neighbour.walkable)
			{
				continue;
			}
			int evaluation = MiniMin(neighbour, targetNode, depth - 1);
			minEval = Mathf.Min(minEval, evaluation);
        }
		Debug.Log(minEval);
		return minEval;
    }

	int MiniMin2(NodeB node, NodeB targetNode, int depth)
	{
		int minEval = int.MaxValue;
		for (int i = 0; i < depth; i++)
        {
			
			foreach (NodeB neighbour in grid.GetNeighbours(node))
			{
				
				int evaluation = GetDistance(neighbour, targetNode);
				if(evaluation < minEval)
                {
					minEval = Mathf.Min(minEval, evaluation);
				}
			}

		}
		return 0;
	}

}