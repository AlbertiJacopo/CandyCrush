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

    [HideInInspector] public GameObject[,] Grid;

    private TileManager _tileMan;

    public static GridManager Instance { get; private set; }

    void Awake() 
    {
        Instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[GridDimension, GridDimension];
        InstanceGrid();
    }

    /// <summary>
    /// Set up the grid and manage the tiles
    /// </summary>
    void InstanceGrid()
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
                Sprite left1 = _tileMan.GetSprite(column - 1, row);
                Sprite left2 = _tileMan.GetSprite(column - 2, row);
                //remove the sprite on the second list to avoid accidential matches
                if (left2 != null && left1 == left2)
                {
                    possibleSprites.Remove(left1);
                }

                //do the same thing but instead of checking the tiles on the left it checks the ones down
                Sprite down1 = _tileMan.GetSprite(column, row - 1);
                Sprite down2 = _tileMan.GetSprite(column, row - 2);
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
    /// used to fill holes in the grid whenmatches occures
    /// </summary>
    public void FillHoles()
    {
        for (int column = 0; column < GridDimension; column++)
        {
            for (int row = 0; row < GridDimension; row++) // 1
            {
                while (_tileMan.GetRenderer(column, row).sprite == null) // 2
                {
                    for (int filler = row; filler < GridDimension - 1; filler++) // 3
                    {
                        SpriteRenderer current = _tileMan.GetRenderer(column, filler); // 4
                        SpriteRenderer next = _tileMan.GetRenderer(column, filler + 1);
                        current.sprite = next.sprite;
                    }
                    SpriteRenderer last = _tileMan.GetRenderer(column, GridDimension - 1);
                    last.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
                }
            }
        }
    }
}
