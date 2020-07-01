using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public NewGrid grid;
    public int x;
    public int y;
    [Space]
    public GameObject node;
    [Space]
    public Node startingNode;
    public Node endingNode;

    // Start is called before the first frame update
    void Start()
    {
        GameObject StoredNodes = new GameObject("StoredNodes");
        grid = new NewGrid(x, y, 1, node, StoredNodes);
    }

    public void Highlight(List<Node> nodeList)
    {
        foreach(Node node in nodeList)
        {
            node.isPath();
        }
    }

    public void Clear()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].GetComponent<Node>().ResetNode();
        }
    }
}
