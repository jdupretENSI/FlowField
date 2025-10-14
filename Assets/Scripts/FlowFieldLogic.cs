using System.Collections.Generic;
using UnityEngine;

public class FlowFieldLogic
{
    private Vector2Int[] _vecArr = new Vector2Int[8]
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
    
    public void Setup(Cell[,] cellMatrix, Vector2Int startPos)
    {
        Matrix = cellMatrix;
        BFS(startPos);
        
        var costMap = new CostMap();
        var HeatMap = new HeatMap();
        var flowField = new FlowField();
        
        costMap.Main(Matrix);
    }

    void BFS(Vector2Int destination)
    {
        // Reset visited set for new BFS
        Visited.Clear();
        Queue.Clear();

        // Check if destination is within bounds and not an obstacle
        if (destination.x < 0 || destination.x >= Matrix.GetLength(0) || 
            destination.y < 0 || destination.y >= Matrix.GetLength(1) ||
            Matrix[destination.x, destination.y].Cost == -1)
        {
            Debug.LogError("Destination is out of bounds or an obstacle!");
            return;
        }

        Cell destinationCell = Matrix[destination.x, destination.y];
        destinationCell.Cost = 0;
        destinationCell.Direction = Vector2Int.zero;
        Matrix[destination.x, destination.y] = destinationCell;

        Queue.Enqueue(destinationCell);
        Visited.Add(destinationCell);

        while (Queue.Count > 0)
        {
            Cell currentCell = Queue.Dequeue();

            foreach (Vector2Int direction in _vecArr)
            {
                Vector2Int nextPos = currentCell.Position + direction;
            
                // Check bounds
                if (nextPos.x < 0 || nextPos.x >= Matrix.GetLength(0) || 
                    nextPos.y < 0 || nextPos.y >= Matrix.GetLength(1))
                    continue;

                Cell nextCell = Matrix[nextPos.x, nextPos.y];
            
                // Skip if visited or is an obstacle
                if (Visited.Contains(nextCell) || nextCell.Cost == -1)
                    continue;

                // Update cost and direction
                nextCell.Cost = currentCell.Cost + 1;
                nextCell.Direction = direction;
                Matrix[nextPos.x, nextPos.y] = nextCell;
                
                Visited.Add(nextCell);
                Queue.Enqueue(nextCell);
            }
        }
    }
}