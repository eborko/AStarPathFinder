using Assets.Scripts.PathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayGround : MonoBehaviour
{
    public static Assets.Scripts.PathFinding.Grid Grid;
    public static readonly PathFinder PathFinder = new PathFinder();

    public GameObject CellPrefab;

    public int HorisontalDimension;
    public int VerticalDimension;
    // Start is called before the first frame update
    void Start()
    {


        if (HorisontalDimension <= 0 || VerticalDimension <= 0)
        {
            return;
        }

        Grid = new Assets.Scripts.PathFinding.Grid(HorisontalDimension, VerticalDimension);

        for (int row = 0; row < VerticalDimension; row++)
        {
            for (int column = 0; column < HorisontalDimension; column++)
            {
                GameObject cellInstance = Instantiate(CellPrefab);
                cellInstance.transform.Translate(new Vector3((float)row, (float)column, 0.0f));
                cellInstance.GetComponent<Cell>().XPosition = column;
                cellInstance.GetComponent<Cell>().YPosition = row;

                Grid[row, column] = new Node(cellInstance.GetComponent<Cell>());
                Grid[row, column].Cell.UpdateWalkable(true);

                int prevRow = row - 1;
                int prevColumn = column - 1;

                int nextColumn = column + 1;

                if (prevRow >= 0)
                {
                    Grid[row, column].TopNode = Grid[prevRow, column];
                    Grid[prevRow, column].BottomNode = Grid[row, column];
                }

                if (prevColumn >= 0)
                {
                    Grid[row, column].LeftNode = Grid[row, prevColumn];
                    Grid[row, prevColumn].RightNode= Grid[row, column];

                    if (prevRow >= 0)
                    {
                        Grid[row, column].TLNode= Grid[prevRow, prevColumn];
                        Grid[prevRow, prevColumn].BRNode = Grid[row, column];

                        if (nextColumn < HorisontalDimension)
                        {
                            Grid[row, column].TRNode = Grid[prevRow, nextColumn];
                            Grid[prevRow, nextColumn].BLNode = Grid[row, column];
                        }
                    }
                }
            }
        }

        // Testing obstacle
        int colum = 3;
        for (int row = 2; row < 4; row++)
        {
            Grid[row, colum].Cell.UpdateWalkable(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
