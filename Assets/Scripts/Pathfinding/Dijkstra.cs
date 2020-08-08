using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Dijkstra : MonoBehaviour
{
    public Transform seeker, target;

    GridA grid;


    void Awake()
    {
        grid = GetComponent<GridA>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        
        HashSet<Node> visited = new HashSet<Node>();
        MinHeap nodesHeap = new MinHeap(grid.numberOfNodes);
        

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(targetPos);

        
        startNode.dist = 0;

        foreach (Node n in grid.grid)
        {
            nodesHeap.Add(n);
        }
        
        while (!nodesHeap.IsEmpty())
        {
            
                Node node = nodesHeap.Pop();
                visited.Add(node);

                //grid.visited.Add(node); //only for drawing purposes

            if (node == endNode)
            {
                
                sw.Stop();
                //print("Found path in: " + sw.ElapsedMilliseconds);
                RetracePath(startNode, endNode);
                return;
            }


            foreach (Node neighbour in grid.GetNeighbours(node))
                {

                    if (!visited.Contains(neighbour) && neighbour.walkable)
                    {

                        
                        if (neighbour.dist >= node.dist + GetDistance(node, neighbour))
                        {
                            neighbour.dist = node.dist + GetDistance(node, neighbour);
                            print("Distance: " + neighbour.dist);

                            neighbour.parent = node;
                        }

                        //queue.Enqueue(neighbour);

                    }
                }


                nodesHeap.ReCalculateUp();
                nodesHeap.ReCalculateDown();
            

        }
        

    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            if(currentNode.parent == null)
            {
                return;
            }
            currentNode = currentNode.parent;
        }
        path.Reverse();

        //grid.path = path;


    }
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }

    Node GetLowestDistanceNode(List<Node> nodes)
    {
        int lowest = int.MaxValue;
        Node lowestNode = null;
        foreach(Node n in nodes)
        {
            if(n.dist < lowest)
            {
                lowest = n.dist;
                lowestNode = n;
            }
        }
        return lowestNode;
    }

}
