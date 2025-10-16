using System.Collections.Generic;
using UnityEngine;

public class FlowFieldLogic
{
    private readonly Vector2Int[] _vecArr = new Vector2Int[8]
    {
        Vector2Int.up,
        Vector2Int.up + Vector2Int.right,
        Vector2Int.right,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.down,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.left,
        Vector2Int.left + Vector2Int.up,
    };
    
    public Cell[,] Matrix;
    public HashSet<Cell> Visited = new();
    public Queue<Cell> Queue = new();

    private Manager _manager;
    
    
    public void Setup(Cell[,] cellMatrix, Vector2Int startPos, Manager manager)
    {
        Matrix = cellMatrix;
        GenerateFlowField(cellMatrix, startPos);


        _manager = manager;
        var Flowfield = _manager.GetComponent<Manager>();
        Flowfield.StoreFlowField(startPos, Matrix);
    }
    
    
     public void GenerateFlowField(Cell[,] cellMatrix, Vector2Int destination)
    {
        // First we fill in the costs
        FloodFillCosts(cellMatrix, destination);
        // Then we get the directions once the costs have all been added.
        CalculateDirections(cellMatrix);
    }

    void FloodFillCosts(Cell[,] matrix, Vector2Int destination)
    {
        // Reset and initialize
        Visited.Clear();
        Queue.Clear();
        
        if (!IsValidDestination(matrix, destination)) return;
        
        InitializeDestination(matrix, destination);
        
        while (Queue.Count > 0)
        {
            Cell current = Queue.Dequeue();
            
            foreach (Vector2Int dir in _vecArr)
            {
                Vector2Int neighborPos = current.Position + dir;
                
                if (IsNotInBounds(matrix, neighborPos)) continue;
                
                Cell neighbor = matrix[neighborPos.x, neighborPos.y];
                if (ShouldSkipCell(neighbor)) continue;
                
                // Update neighbor cost
                neighbor.Cost = current.Cost + 1;
                matrix[neighborPos.x, neighborPos.y] = neighbor;
                
                Visited.Add(neighbor);
                Queue.Enqueue(neighbor);
            }
        }
    }

    void CalculateDirections(Cell[,] matrix)
    {
        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                Cell current = matrix[x, y];
                
                if (current.Cost <= 0) // Destination or obstacle
                {
                    current.Direction = Vector2Int.zero;
                    matrix[x, y] = current;
                    continue;
                }
                
                current.Direction = FindBestDirection(matrix, current);
                matrix[x, y] = current;
            }
        }
    }

    Vector2Int FindBestDirection(Cell[,] matrix, Cell current)
    {
        Vector2Int bestDirection = Vector2Int.zero;
        int lowestCost = current.Cost;
        
        foreach (Vector2Int dir in _vecArr)
        {
            Vector2Int neighborPos = current.Position + dir;
            if (IsNotInBounds(matrix, neighborPos)) continue;
            
            Cell neighbor = matrix[neighborPos.x, neighborPos.y];
            if (neighbor.Cost == -1) continue; // Skip obstacles
            
            if (neighbor.Cost < lowestCost)
            {
                lowestCost = neighbor.Cost;
                bestDirection = dir;
            }
        }
        
        return bestDirection;
    }

    // Helper methods
    bool IsValidDestination(Cell[,] matrix, Vector2Int destination)
    {
        // For now if it's an obstacle then return false,
        // I bet I Could move this logic to the manager, or whatever destination setter I make
        return matrix[destination.x, destination.y].Cost != -1;
    }
    
    bool IsNotInBounds(Cell[,] matrix, Vector2Int position)
    {
        // Check bounds
        return position.x < 0 || position.x >= Matrix.GetLength(0) || 
               position.y < 0 || position.y >= Matrix.GetLength(1);
    }
    
    bool ShouldSkipCell(Cell cell)
    {
        return Visited.Contains(cell) || cell.Cost == -1;
    }
    
    void InitializeDestination(Cell[,] matrix, Vector2Int destination)
    {
        Cell destCell = matrix[destination.x, destination.y];
        destCell.Cost = 0;
        matrix[destination.x, destination.y] = destCell;
        
        Queue.Enqueue(destCell);
        Visited.Add(destCell);
    }
}