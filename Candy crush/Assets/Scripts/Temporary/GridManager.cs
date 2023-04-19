using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Tooltip("List of sprites")]
    public List<Sprite> Sprites = new List<Sprite>();

    [Tooltip("Gameobject associated to the sprites")]
    public GameObject TilePrefab;

    [Tooltip("How many rows and column the grid has")]
    public int GridDimension = 8;

    [Tooltip("Distance between the objects in the grid")]
    public float Distance = 1.0f;

    private GameObject[,] Grid;

    public static GridManager Instance { get; private set; }

    void Awake() 
    {
        Instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[GridDimension, GridDimension];
        InitGrid();
    }

    /// <summary>
    /// Set up the grid and manage the tiles
    /// </summary>
    void InitGrid()
    {
        //creates the grid
        Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0);

        //for cycle to set the dimension
        for (int row = 0; row < GridDimension; row++)

            //for cycle to populate the grid
            for (int column = 0; column < GridDimension; column++)
            {
                //second list of sprites used for utility
                List<Sprite> possibleSprites = new List<Sprite>(Sprites);

                //Choose what sprite to use for this cell
                Sprite left1 = GetSpriteAt(column - 1, row);
                Sprite left2 = GetSpriteAt(column - 2, row);
                //remove the sprite on the second list to avoid accidential matches
                if (left2 != null && left1 == left2)
                {
                    possibleSprites.Remove(left1);
                }

                //do the same thing but instead of checking the tiles on the left it checks the ones down
                Sprite down1 = GetSpriteAt(column, row - 1);
                Sprite down2 = GetSpriteAt(column, row - 2);
                if (down2 != null && down1 == down2)
                {
                    possibleSprites.Remove(down1);
                }

                //choose the tile and instantiate it
                GameObject newTile = Instantiate(TilePrefab);
                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                renderer.sprite = Sprites[Random.Range(0, Sprites.Count)];
                newTile.transform.parent = transform;
                newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;

                //associate it witha grid position
                Grid[column, row] = newTile;

                renderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];

                Tile tile = newTile.AddComponent<Tile>();

                //set a position value in Tile class
                tile.Position = new Vector2Int(column, row);
                newTile.transform.parent = transform;
            }
    }

    /// <summary>
    /// Return the sprite that is in a determinate position
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    Sprite GetSpriteAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
            || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.gameObject.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    /// <summary>
    /// used to swap tile when clicking mouse button (see in Tile class)
    /// </summary>
    /// <param name="tile1Position"></param>
    /// <param name="tile2Position"></param>
    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position)
    {

        //get the sprite of the first tile
        GameObject tile1 = Grid[tile1Position.x, tile1Position.y];
        SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();

        //get the sprite of the second tile
        GameObject tile2 = Grid[tile2Position.x, tile2Position.y];
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

    }

    /// <summary>
    /// Return the render value of the tile in a determinated position (used to change the sprite when swapping tiles)
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    SpriteRenderer GetSpriteRendererAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
             || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
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
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                SpriteRenderer current = GetSpriteRendererAt(column, row);

                //The horizontal matching tiles (if it does exist)
                List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(column, row, current.sprite); 
                if (horizontalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(current);
                }

                //The vertical matching tiles (if it does exist)
                List<SpriteRenderer> verticalMatches = FindRowMatchForTile(column, row, current.sprite);
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
    List<SpriteRenderer> FindColumnMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < GridDimension; i++)
        {
            //check if the other sprites on the column are the same
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
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
    List<SpriteRenderer> FindRowMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < GridDimension; i++)
        {
            //check if the other sprites on the row are the same
            SpriteRenderer nextRow = GetSpriteRendererAt(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }
}
