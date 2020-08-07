using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDijkstra
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    

    public int dist;
    public int prev;
    public NodeDijkstra parent;

    public NodeDijkstra(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }


}