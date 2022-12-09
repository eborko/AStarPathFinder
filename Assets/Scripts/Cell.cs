using Assets.Scripts.PathFinding;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool IsWalkable { get; set; }

    public int XPosition { get; set; }

    public int YPosition { get; set; }

    public Node Owner { get; set; }

    private IList<Node> _path;

    /// <summary>
    /// test
    /// </summary>
    /// <param name="isWalkable"></param>
    public void UpdateWalkable(bool isWalkable)
    {
        this.IsWalkable = isWalkable;
        if (!isWalkable)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    void OnMouseEnter()
    {
        if (IsWalkable)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            Debug.Log($"(XPos: {XPosition},YPos: {YPosition})");

            PlayGround.PathFinder.CalculatePath(this.Owner, PlayGround.Grid[2, 4]);
            _path = PlayGround.PathFinder.Path;

            if (_path != null)
            {
                Debug.Log("Start path:");
                foreach (Node node in _path)
                {
                    Debug.Log($"XPos: {node.Cell.XPosition}, YPos: {node.Cell.YPosition}");
                    node.Cell.GetComponent<SpriteRenderer>().color = Color.white;
                }
                Debug.Log("End path:");
            }
        }
    }

    void OnMouseExit()
    {
        if (IsWalkable)
        {
            GetComponent<SpriteRenderer>().color = Color.green;

            // Demark path
            if (_path != null)
            {
                foreach (Node node in _path)
                {
                    node.Cell.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }
    }
}
