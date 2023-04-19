using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile0 : MonoBehaviour, IPointerClickHandler
{
    public TileData0 data;

    public void Initialize(GridManager0 gridM, int rowInit, int columnInit)
    {
        data = new TileData0(gridM, rowInit, columnInit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
    }
}
