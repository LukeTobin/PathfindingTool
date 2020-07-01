using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGrid
{
    int width;
    int height;

    int[,] gridArray;

    // constructor
    public NewGrid(int width, int height, int cellSize, GameObject tile, GameObject newParent) 
    {
        this.width = width;
        this.height = height;

        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) 
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                GameObject node = GameObject.Instantiate(tile, GetWorldPosition(x, y), Quaternion.identity); 
                node.transform.parent = newParent.transform;
                node.GetComponent<Node>().x = x;
                node.GetComponent<Node>().y = y;
            }
        }
    }

    Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y, 1);
    }

    public Node GetNode(int x, int y)
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].GetComponent<Node>().x == x && nodes[i].GetComponent<Node>().y == y)
            {
                return nodes[i].GetComponent<Node>();
            }
        }
        return null;
    }
}

