using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingBFS : MonoBehaviour
{

    public Transform seeker, target;

    GridA grid;


    void Awake()
    {
        grid = GetComponent<GridA>();
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        
        
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();        

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(targetPos);

        queue.Enqueue(startNode);
        

        while (queue.Count != 0)
        {
            Node node = queue.Dequeue();
            visited.Add(node);
            grid.visited.Add(node); //only for drawing purposes
            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                
                if (!visited.Contains(neighbour) && neighbour.walkable)
                {
                    neighbour.parent = node;
                    queue.Enqueue(neighbour);
                    visited.Add(neighbour);
                    if (neighbour == endNode)
                    {
                        RetracePath(startNode, endNode);
                        return;
                    }
                }
            }
            
            
        }
        

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

        grid.path = path;
        

    }
}
