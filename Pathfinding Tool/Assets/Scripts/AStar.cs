using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    Manager gm;

    private const int MOVE_STRAIGHT_COST = 10;

    public static AStar Instance 
    { 
        get; 
        private set; 
    }

    List<Node> openList;
    List<Node> closedList;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();
        Instance = this;
    }

    public List<Node> FindPath(Vector2 startPos, Vector2 endPos)
    {
        List<Node> path = FindPath((int)startPos.x, (int)startPos.y, (int)endPos.x, (int)endPos.y);
        if (path == null)
        {
            Debug.Log("couldnt make path from " + (int)startPos.x + "," + (int)startPos.y + " : to : " + (int)endPos.x + "," + (int)endPos.y);
            return null;
        }
        else
        {
            return path;
        }
    }

    private List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        Node startNode = gm.grid.GetNode(startX, startY);
        Node endNode = gm.grid.GetNode(endX, endY);

        openList = new List<Node>() { startNode };
        closedList = new List<Node>();

        for (int x = 0; x < gm.x; x++)
        {
            for (int y = 0; y < gm.y; y++)
            {
                Node node = gm.grid.GetNode(x, y);
                if (node != null)
                {
                    node.g = int.MaxValue;
                    node.CalcF();
                    node.origin = null;
                }
            }
        }

        startNode.g = 0;
        startNode.h = CalculateDistanceCost(startNode, endNode);
        startNode.CalcF();

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                    continue;

                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.g + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.g)
                {
                    neighbourNode.origin = currentNode;
                    neighbourNode.g = tentativeGCost;
                    neighbourNode.h = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalcF();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // no path
        Debug.Log("No path found.");
        return null;
    }

    private List<Node> GetNeighbourList(Node currentNode)
    {
        List<Node> neighbour = new List<Node>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            if (gm.grid.GetNode(currentNode.x - 1, currentNode.y) != null)
                neighbour.Add(gm.grid.GetNode(currentNode.x - 1, currentNode.y));
        }

        if (currentNode.x + 1 < gm.x)
        {
            // Right
            if (gm.grid.GetNode(currentNode.x + 1, currentNode.y) != null)
                neighbour.Add(gm.grid.GetNode(currentNode.x + 1, currentNode.y));
        }

        if (currentNode.y - 1 >= 0)
        {
            if (gm.grid.GetNode(currentNode.x, currentNode.y - 1) != null)
                neighbour.Add(gm.grid.GetNode(currentNode.x, currentNode.y - 1));
        }

        if (currentNode.y + 1 < gm.y)
        {
            if (gm.grid.GetNode(currentNode.x, currentNode.y + 1) != null)
                neighbour.Add(gm.grid.GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighbour;
    }

    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.origin != null)
        {
            path.Add(currentNode.origin);
            currentNode = currentNode.origin;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_STRAIGHT_COST * remaining;
    }

    private Node GetLowestNode(List<Node> PathNodes)
    {
        Node lowestCostNode = PathNodes[0];
        for (int i = 0; i < PathNodes.Count; i++)
        {
            if (PathNodes[i].f < lowestCostNode.f)
            {
                lowestCostNode = PathNodes[i];
            }
        }
        return lowestCostNode;
    }
}
