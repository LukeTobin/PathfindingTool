using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int x, y;
    [Space]
    public int g;
    public int h;
    public int f;
    [Space]
    public Node origin;
    [Space]
    public bool isWalkable;
    bool selected;
    [Space]
    public Color start;
    public Color end;
    public Color block;
    public Color path;
    public Color clear;

    SpriteRenderer sr;
    Manager gm;
    AStar pathfinding;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        gm = FindObjectOfType<Manager>();
        pathfinding = GetComponent<AStar>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isWalkable)
            {
                if (gm.startingNode == null && !selected)
                {
                    gm.endingNode = null;
                    gm.startingNode = this;
                    sr.color = start;
                    selected = true;
                }
                else if (gm.startingNode != null && gm.endingNode == null && !selected)
                {
                    gm.endingNode = this;
                    sr.color = end;
                    selected = true;

                    List<Node> path = pathfinding.FindPath(gm.startingNode.transform.position, transform.position);
                    if (path != null)
                        gm.Highlight(path);

                }
                else if (gm.startingNode == this && selected)
                {
                    gm.startingNode = null;
                    sr.color = clear;
                    selected = false;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (!isWalkable)
            {
                isWalkable = true;
                sr.color = clear; 
            }
            else if (isWalkable)
            {
                isWalkable = false;
                sr.color = block;
            }
        }
        
    }

    public void CalcF()
    {
        f = g + h;
    }

    public void isPath()
    {
        sr.color = path;
    }

    public void ResetNode()
    {
        selected = false;
        isWalkable = true;
        sr.color = clear;
    }
}
