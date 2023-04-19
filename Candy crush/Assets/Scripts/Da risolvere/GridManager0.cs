using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager0 : MonoBehaviour
{
    [Tooltip("Interchangeable sprites of the object")]
    public List<Sprite> Sprites = new List<Sprite>();
    [Tooltip("Empty gameobject prefab, used to assign the sprites")]
    public GameObject TilePrefab;
    [Tooltip("Grid array for columns and rows")]
    private GameObject[,] m_Grid;
    [Tooltip("Distance between one object and another")]
    public float Distance;
    [Tooltip("Max number of rows")]
    public int maxRow;
    [Tooltip("Max number of columns")]
    public int maxColumn;
    private Grid m_GridData;
    public Dictionary<Vector2Int, TileData0> mapTiles = new Dictionary<Vector2Int, TileData0>();

    private void Awake()
    {
        m_GridData = GetComponent<Grid>();
    }

    private void Start()
    {
        m_Grid = new GameObject[maxRow, maxColumn];
        GenerateGrid();
    }

    /// <summary>
    /// generate the grid and the objects in it
    /// </summary>
    private void GenerateGrid()
    {
        Vector3 startPosition = new Vector3(maxColumn * (m_GridData.cellSize.x + m_GridData.cellGap.x) / 2, 0, maxRow * (m_GridData.cellSize.z + m_GridData.cellGap.z) / 2);
        float x = startPosition.x;
        float y = startPosition.z;

        for (int row = maxRow; row > 0; row--)
        {
            for(int column = 0; column < maxColumn; column++)
            {
                List<Sprite> possibleSprites = new List<Sprite>(Sprites);

                //Choose what sprite to not use for this cell and removes it from the "options"
                //check on the left tile
                Sprite left1 = GetSpriteAt(column - 1, row);
                Sprite left2 = GetSpriteAt(column - 2, row);
                if (left2 != null && left1 == left2)
                {
                    possibleSprites.Remove(left1);
                }
                //check on the bottom tile
                Sprite down1 = GetSpriteAt(column, row - 1);
                Sprite down2 = GetSpriteAt(column, row - 2);
                if (down2 != null && down1 == down2)
                {
                    possibleSprites.Remove(down1);
                }

                //instantiate a empty gameobject and assign a sprite
                GameObject tile = Instantiate(TilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                renderer.sprite = Sprites[Random.Range(0, Sprites.Count)];
                tile.transform.localScale = m_GridData.cellSize;
                x -= 1 * (m_GridData.cellSize.x + m_GridData.cellGap.x);
                //tile.Initialize(this, row, column);
                tile.name = "Tile - (" + row.ToString() + " - " + column.ToString() + ")";
                m_Grid[column, row] = tile;
            }
            x = startPosition.x;
            y -= 1 * (m_GridData.cellSize.z + m_GridData.cellGap.z);
        }
    }
    /// <summary>
    /// return the prite of the object in a determinate cell of the grid
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    Sprite GetSpriteAt(int row, int column)
    {
        if (row < 0 || row >= maxRow && column < 0 || column >= maxColumn)
            return null;
        GameObject tile = m_Grid[column, row];
        SpriteRenderer renderer = tile.gameObject.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

}
