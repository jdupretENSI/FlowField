using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Manager : MonoBehaviour
{
    [Header("Maps")]
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private Tilemap _costMap;
    [SerializeField] private Tilemap _heatMap;
    [SerializeField] private Tilemap _FlowField;
    
    [Header("Cells")]
    [SerializeField] private TileBase _fullTile;
    [SerializeField] private TileBase _emptyTile;
    
    private void Start()
    {
        
        BoundsInt bounds = _tileMap.cellBounds;
        Cell[,] cellMatrix = new Cell[bounds.size.x, bounds.size.y];
        
        // Fill the Matrix array with all positions and set obstacle costs
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(
                    bounds.xMin + x, 
                    bounds.yMin + y, 
                    bounds.zMin
                );
                
                Cell cell = new Cell(new Vector2Int(x, y));
                
                // Check if this position has an obstacle tile
                TileBase tile = _tileMap.GetTile(tilePosition);
                if (tile == _fullTile)
                {
                    cell.Cost = -1; // Mark as obstacle
                }
                if  (tile == _emptyTile)
                {
                    cell.Cost = 0;
                }
                else
                {
                    cell.Cost = 1000000;
                }
                
                cellMatrix[x, y] = cell;
            }
        }
        
        var flowField = new FlowFieldLogic();
        flowField.Setup(cellMatrix, Vector2Int.zero);
    }
}