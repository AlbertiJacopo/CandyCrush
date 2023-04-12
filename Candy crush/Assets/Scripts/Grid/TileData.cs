using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public int row;
    public int column;
    public GridManager gm;

    public TileData(GridManager gridManager, int newRow, int newColumn)
    {
        row = newRow;
        column = newColumn;
        gm = gridManager;
    }
}
