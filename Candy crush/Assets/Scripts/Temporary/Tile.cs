using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    private static Tile selected;
    private SpriteRenderer Renderer;
    //position of this tile
    public Vector2Int Position;
    private TileManager _tileManager;

    // Start is called before the first frame update
    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Change color if this tile selected
    /// </summary>
    public void Select()
    {
        Renderer.color = Color.grey;
    }

    /// <summary>
    /// Return to normal color if not selected
    /// </summary>
    public void Unselect()
    {
        Renderer.color = Color.white;
    }

    /// <summary>
    /// Actives when the player press mouse button
    /// </summary>
    private void OnMouseDown()
    {
        //check if this tile is selected
        if (selected != null)
        {
            if (selected == this)
                return;

            //or else it deselect it
            selected.Unselect();

            //swap position with other adiacent tiles
            if (Vector2.Distance(selected.Position, Position) == GridManager.Instance.Distance)
            {
                _tileManager.SpriteSwap(Position, selected.Position);
                selected = null;
            }

            //if it's not adiacent the selected tile remains still
            else
            {
                selected = this;
                Select();
            }
        }
        else
        {
            selected = this;
            Select();
        }
    }
}
