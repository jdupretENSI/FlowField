using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAI : MonoBehaviour
{
    private Tilemap _tileMap;
    private Tilemap _flowFieldTilemap;
    private Manager _manager;
    private EnemyPool _enemyPool;
    
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 5f;
    
    private Vector3Int _currentCellPosition;
    private Vector3 _targetWorldPosition;

    void Start()
    {
        InitializeReferences();
        SnapToNearestTile();
        UpdateTargetPosition();
    }

    void Update()
    {
        MoveAlongFlowField();
    }

    private void InitializeReferences()
    {
        _enemyPool = GetComponentInParent<EnemyPool>();
        _tileMap = GetComponentInParent<Tilemap>();
        _manager = FindFirstObjectByType<Manager>();
        
        if (_manager != null)
        {
            _flowFieldTilemap = _manager._flowField;
        }
    }

    private void SnapToNearestTile()
    {
        Vector3 currentWorldPos = transform.position;
        Vector3Int cellPosition = _tileMap.WorldToCell(currentWorldPos);
        
        if (IsValidTile(cellPosition))
        {
            _currentCellPosition = cellPosition;
            transform.position = _tileMap.GetCellCenterWorld(cellPosition);
            return;
        }

        Vector3Int nearestValidTile = FindNearestValidTile(cellPosition);
        if (nearestValidTile != Vector3Int.zero)
        {
            _currentCellPosition = nearestValidTile;
            transform.position = _tileMap.GetCellCenterWorld(nearestValidTile);
        }
        else
        {
            _enemyPool.ReturnToPool(this.gameObject);
        }
    }

    private void MoveAlongFlowField()
    {
        if (!_flowFieldTilemap) return;

        transform.position = Vector3.MoveTowards(transform.position, _targetWorldPosition, _moveSpeed * Time.deltaTime);

        if ((_targetWorldPosition - transform.position).sqrMagnitude > 0.01f)
        {
            Vector3 direction = (_targetWorldPosition - transform.position).normalized;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, _targetWorldPosition) < 0.1f)
        {
            UpdateTargetPosition();
        }
    }

    private void UpdateTargetPosition()
    {

        TileBase currentTile = _flowFieldTilemap.GetTile(_currentCellPosition);
        
        if (!currentTile)
        {
            Vector3Int destinationCell = new Vector3Int(
                _manager.Destination.x + _tileMap.cellBounds.xMin, 
                _manager.Destination.y + _tileMap.cellBounds.yMin, 0);
                
                _enemyPool.ReturnToPool(this.gameObject);
            return;
        }

        Vector2Int direction = GetDirectionFromTile(currentTile);
        if (direction == Vector2Int.zero)
        {
            _enemyPool.ReturnToPool(this.gameObject);
            return;
        }

        Vector3Int nextCellPosition = _currentCellPosition + new Vector3Int(direction.x, direction.y, 0);
        
        if (!IsValidTile(nextCellPosition))
        {
            _enemyPool.ReturnToPool(this.gameObject);
            return;
        }

        _currentCellPosition = nextCellPosition;
        _targetWorldPosition = _tileMap.GetCellCenterWorld(_currentCellPosition);
    }

    private Vector2Int GetDirectionFromTile(TileBase tile)
    {
        if (tile == _manager._arrows[0]) return Vector2Int.up;
        if (tile == _manager._arrows[1]) return Vector2Int.up + Vector2Int.right;
        if (tile == _manager._arrows[2]) return Vector2Int.right;
        if (tile == _manager._arrows[3]) return Vector2Int.right + Vector2Int.down;
        if (tile == _manager._arrows[4]) return Vector2Int.down;
        if (tile == _manager._arrows[5]) return Vector2Int.down + Vector2Int.left;
        if (tile == _manager._arrows[6]) return Vector2Int.left;
        if (tile == _manager._arrows[7]) return Vector2Int.left + Vector2Int.up;
        
        return Vector2Int.zero;
    }

    private bool IsValidTile(Vector3Int cellPosition)
    {
        TileBase tile = _tileMap.GetTile(cellPosition);
        return tile == _manager._emptyTile;
    }

    private Vector3Int FindNearestValidTile(Vector3Int startPosition, int maxSearchRadius = 10)
    {
        for (int radius = 1; radius <= maxSearchRadius; radius++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Mathf.Abs(x) == radius || Mathf.Abs(y) == radius)
                    {
                        Vector3Int checkPosition = new Vector3Int(
                            startPosition.x + x,
                            startPosition.y + y,
                            startPosition.z
                        );

                        if (IsValidTile(checkPosition))
                        {
                            return checkPosition;
                        }
                    }
                }
            }
        }

        return Vector3Int.zero;
    }
}