using UnityEngine;

public struct Cell
{
    public Vector2Int Position;
    public int Cost;
    public Vector2Int Direction;

    public Cell(Vector2Int position) : this()
    {
        Position = position;
    }
    
    public override bool Equals(object obj)
    {
        return obj is Cell cell && Position.Equals(cell.Position);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }
}