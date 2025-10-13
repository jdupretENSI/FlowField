using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Manager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _fullTile;
    [SerializeField] private TileBase _emptyTile;
    
    private void Start()
    {
        var FlowField = new FlowField();
        FlowField.Setup(_tilemap.size.x, _tilemap.size.y, Vector2.zero);
        // for (int i = 0; i < 10; i++)
        // {
        //     for (int j = 0; j < 10; j++)
        //     {
        //         TileBase tileBase = _tilemap.GetTile(new Vector3Int(i, j, 0));
        //         if (tilebase == _fullTile)
        //         {
        //             
        //         }
        //         else if (tileBase == _emptyTile)
        //         {
        //             
        //         }
        //     }
        // }
    }
}