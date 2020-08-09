using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridA : MonoBehaviour
{
    
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;
    public TerrainType[] walkableRegions;
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDict = new Dictionary<int, int>();

    public bool drawGizmos;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public int numberOfNodes
    {
        get { return gridSizeX * gridSizeY; }
    }

    void Awake()
    {
        GameObject.Find("Ground").GetComponent<BuildingManager>().CreateGrid();
        GameObject.Find("Ground").GetComponent<MapGeneration>().GenerateMap();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach(TerrainType region in walkableRegions)
        {
            walkableMask.value = walkableMask | region.terrainMask.value;
            walkableRegionsDict.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();

    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - new Vector3(1, 0, 0) * gridWorldSize.x / 2 - new Vector3(0, 0, 1) * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                /*
                if(GetComponent<BuildingManager>().grid.GetValue(x, y) != "Empty")
                {
                    grid[x, y] = new Node(false, worldPoint, x, y);
                }
                else
                {
                    grid[x, y] = new Node(true, worldPoint, x, y);

                }
                */
                int movementPenalty = 0;

                //raycast
                if(walkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 100, Vector3.down);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit, 120, walkableMask))
                    {
                        walkableRegionsDict.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);


            }
        }
    }

    public void UpdateGrid()
    {
        Vector3 worldBottomLeft = transform.position - new Vector3(1, 0, 0) * gridWorldSize.x / 2 - new Vector3(0, 0, 1) * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                //TODO: robni resourci morajo biti walkable
                /*
                if(GameObject.Find("Ground").GetComponent<Grid>().GetValue(x, y) == "Empty")
                {
                    grid[x, y] = new Node(true, worldPoint, x, y);
                }
                else if(GameObject.Find("Ground").GetComponent<Grid>().GetValue(x, y) == "Empty")
                {
                    grid[x, y] = new Node(true, worldPoint, x, y);

                }
                */

                int movementPenalty = 0;

                //raycast
                if (walkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 100, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 120, walkableMask))
                    {
                        walkableRegionsDict.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);


            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> path;

    void OnDrawGizmos()
    {
        if(drawGizmos)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    if (path != null)
                        if (path.Contains(n))
                            Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        
        
    }
    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

}
