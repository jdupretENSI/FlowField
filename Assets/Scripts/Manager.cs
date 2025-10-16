using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class Manager : MonoBehaviour
{
    [Header("Maps")]
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private Tilemap _costMap;
    [SerializeField] private Tilemap _heatMap;
    [SerializeField] public Tilemap _flowField;
    
    [Header("Cells")]
    [SerializeField] private TileBase _fullTile;
    [SerializeField] public TileBase _emptyTile;
    [SerializeField] private TileBase _flowFieldTile;

    [Header("Directions")] 
    //Clockwise from 12 OClock
    [SerializeField] public TileBase[] _arrows = new TileBase[8];
    
    [Header("Destination")]
    [SerializeField] public Vector2Int Destination;
    
    [Header("Entities")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;
    
    [Header("Stuff to have fun with")]
    [SerializeField] public int SpawnCount;
    
    private readonly Dictionary<Vector2Int, int> _vecDict = new ()
    {
        { Vector2Int.up, 0 },
        { Vector2Int.up + Vector2Int.right, 1 } ,
        { Vector2Int.right, 2 } ,
        { Vector2Int.right + Vector2Int.down, 3 } ,
        { Vector2Int.down, 4 } ,
        { Vector2Int.down + Vector2Int.left, 5 } ,
        { Vector2Int.left, 6 } ,
        { Vector2Int.left + Vector2Int.up, 7 }
    };
    
    public Dictionary<Vector2Int, Cell[,]> _flowFieldsDict = new();
    
    
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
                    cell.Cost = -1;
                }
                
                cellMatrix[x, y] = cell;
            }
        }
        
        var flowField = new FlowFieldLogic();
        flowField.Setup(cellMatrix, Destination, this);
    }
    

    [ContextMenu("Spawn Enemy")]
    public void SpawnEnemy()
    {
        var rand =  new Random();
        Vector3 position = new Vector3();
        for (int enemy = 0; enemy <= SpawnCount; enemy++)
        {
            position.x = rand.Next(_tileMap.cellBounds.xMin, _tileMap.cellBounds.xMax);
            position.y = rand.Next(_tileMap.cellBounds.yMin, _tileMap.cellBounds.yMax);
            position.z = 0;
            
            var e = Instantiate(_enemy, _tileMap.transform);
            e.transform.position = position;
        }
    }

    public void CostMap(Cell[,] cellMatrix)
    {
        BoundsInt bounds = _costMap.cellBounds;

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(
                    bounds.xMin + x,
                    bounds.yMin + y,
                    bounds.zMin + 2 //+ 2 because it should be overlayed above the map or heatmap 
                );
                
                Cell cell = new Cell(new Vector2Int(x, y));
                
                TileBase tile = _costMap.GetTile(tilePosition);
                //Idk how to display numbers really
            }
        }
    }
    
    public void HeatMap(Cell[,] cellMatrix)
    {
        BoundsInt bounds = _heatMap.cellBounds;

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(
                    bounds.xMin + x,
                    bounds.yMin + y,
                    bounds.zMin + 1 //+ 1 because it should be overlayed above the map 
                );
                
                Cell cell = new Cell(new Vector2Int(x, y));
                
                TileBase tile = _heatMap.GetTile(tilePosition);
                
                //I think I need to use a floodfill here? unsure
            }
        }
    }
    public void FlowField(Cell[,] cellMatrix)
    {
        
        BoundsInt bounds = _tileMap.cellBounds;
        
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(
                    bounds.xMin + x,
                    bounds.yMin + y,
                    bounds.zMin // this might break it+ 2 //+ 2 because it should be overlayed above the map and heatmap
                );
                
                Cell cell = new Cell(new Vector2Int(x, y));

                if (_vecDict.ContainsKey(cellMatrix[x, y].Direction))
                {
                    _flowField.SetTile(tilePosition, _arrows[_vecDict[cellMatrix[x, y].Direction]]); 
                }
                
            }
        }
    }

    public void StoreFlowField(Vector2Int Destination, Cell[,] CellMatrix)
    {
        //Is called from Flow Field Logic, so we can store it for reference later.
        _flowFieldsDict.Add(Destination, CellMatrix);
        FlowField(CellMatrix);
    }
}











































//Space... The final frontier