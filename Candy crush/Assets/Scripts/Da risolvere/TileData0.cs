using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData0
{
    public int row;
    public int column;
    public GridManager0 gm;

    public TileData0(GridManager0 gridManager, int newRow, int newColumn)
    {
        row = newRow;
        column = newColumn;
        gm = gridManager;
    }
}
