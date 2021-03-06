﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridB : MonoBehaviour
{

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	NodeB[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}

	public NodeB[,] getGrid()
    {
		return grid;
    }

	void CreateGrid()
	{
		grid = new NodeB[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				grid[x, y] = new NodeB(walkable, worldPoint, x, y);
			}
		}
	}

	public List<NodeB> GetNeighbours(NodeB node)
	{
		List<NodeB> neighbours = new List<NodeB>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}

	public List<NodeB> GetNeighbours1(NodeB node)
	{
		List<NodeB> neighbours = new List<NodeB>();
		
		if (node.gridX - 1 >= 0)
			neighbours.Add(grid[node.gridX - 1, node.gridY]);
		if (node.gridX + 1 <= gridSizeX)
			neighbours.Add(grid[node.gridX + 1, node.gridY]);
		if (node.gridY - 1 >= 0)
			neighbours.Add(grid[node.gridX, node.gridY - 1]);
		if (node.gridY + 1 < gridSizeY)
			neighbours.Add(grid[node.gridX, node.gridY + 1]);

		return neighbours;
	}


	public NodeB NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid[x, y];
	}

	public List<NodeB> path;
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null)
		{
			foreach (NodeB n in grid)
			{
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				if (path != null)
					if (path.Contains(n))
						Gizmos.color = Color.black;
				//Gizmos.DrawCube(new Vector3(n.worldPosition.x, -nodeRadius + 0.1f, n.worldPosition.z), Vector3.one * (nodeDiameter - .1f));
			}
		}
	}
}