using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private GridManager _grid;

    /// <summary>
    /// Return the sprite that is in a determinate position
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public Sprite GetSprite(int column, int row)
    {
        if (column < 0 || column >= _grid.GridDimension
            || row < 0 || row >= _grid.GridDimension)
            return null;
        GameObject tile = _grid.Grid[column, row];
        SpriteRenderer renderer = tile.gameObject.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    /// <summary>
    /// used to swap tile when clicking mouse button (see in Tile class)
    /// </summary>
    /// <param name="tile1Position"></param>
    /// <param name="tile2Position"></param>
    public void SpriteSwap(Vector2Int tile1Position, Vector2Int tile2Position)
    {

        //get the sprite of the first tile
        GameObject tile1 = _grid.Grid[tile1Position.x, tile1Position.y];
        SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();

        //get the sprite of the second tile
        GameObject tile2 = _grid.Grid[tile2Position.x, tile2Position.y];
        SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();

        //creat a temporary variable and associate it with the first tile
        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;

        //swap tiles
        bool changesOccurs = CheckMatches();
        if (!changesOccurs)
        {
            temp = renderer1.sprite;
            renderer1.sprite = renderer2.sprite;
            renderer2.sprite = temp;
        }
        else
        {
            do
            {
                _grid.FillHoles();
            } while (CheckMatches());
        }

    }
    /// <summary>
    /// Return the render value of the tile in a determinated position (used to change the sprite when swapping tiles)
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public SpriteRenderer GetRenderer(int column, int row)
    {
        if (column < 0 || column >= _grid.GridDimension
             || row < 0 || row >= _grid.GridDimension)
            return null;
        GameObject tile =  _grid.Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer;
    }

    /// <summary>
    /// Check if the are matches or not
    /// </summary>
    /// <returns></returns>
    bool CheckMatches()
    {
        //store the SpriteRenderers of the cells
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer>();

        //for cycle to check every cell of the grid
        for (int row = 0; row < _grid.GridDimension; row++)
        {
            for (int column = 0; column < _grid.GridDimension; column++)
            {
                SpriteRenderer current = GetRenderer(column, row);

                //The horizontal matching tiles (if it does exist)
                List<SpriteRenderer> horizontalMatches = FindMatchColumn(column, row, current.sprite);
                if (horizontalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(current);
                }

                //The vertical matching tiles (if it does exist)
                List<SpriteRenderer> verticalMatches = FindMatchRow(column, row, current.sprite);
                if (verticalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(verticalMatches);
                    matchedTiles.Add(current);
                }
            }
        }

        foreach (SpriteRenderer renderer in matchedTiles)
        {
            renderer.sprite = null;
        }
        return matchedTiles.Count > 0;
    }

    /// <summary>
    /// Check if there are matches on the tile column
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <param name="sprite"></param>
    /// <returns></returns>
    List<SpriteRenderer> FindMatchColumn(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < _grid.GridDimension; i++)
        {
            //check if the other sprites on the column are the same
            SpriteRenderer nextColumn = GetRenderer(i, row);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }

    /// <summary>
    /// Check if there are matches on the tile column
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <param name="sprite"></param>
    /// <returns></returns>
    List<SpriteRenderer> FindMatchRow(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < _grid.GridDimension; i++)
        {
            //check if the other sprites on the row are the same
            SpriteRenderer nextRow = GetRenderer(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }

}


