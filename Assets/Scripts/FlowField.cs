using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowField
{
    
    //I would have a vector list for the direction we can traverse

    private Vector2[] _vecArr = new Vector2[8]
    {
        Vector2.up,
        Vector2.up + Vector2.right,
        Vector2.right,
        Vector2.right + Vector2.down,
        Vector2.down,
        Vector2.down + Vector2.left,
        Vector2.left,
        Vector2.left + Vector2.up,
    };
    
    //The game board is made up of an l length by w width Array
    public int length;
    public int width;
    public Vector2[,] GameBoard;
    
    //BFS needs a starting position, a dictionary with the positions we have already visited and a queue for the next cell to visit.
    public Vector2 StartPos;
    public Dictionary<Vector2, bool> Visited = new();
    public Queue<Vector2> Queue = new();
    
    //Flood Fill will fill a dictionary with the cost for each position on the game grid
    public Dictionary<Vector2, int> TileCost = new();
    
    //I'll need a current position to iterate from
    public Vector2 CurrentPos;
    //An increment for neighbour cost
    public int Increment = 0;
    
    void setup()
    {
        GameBoard = new Vector2[length, width];
        foreach (Vector2 pos in GameBoard)
        {
            Visited.Add(pos, false);
            TileCost.Add(pos, 0);
        }
        
        CurrentPos = StartPos;
        
    }

    //Let's say we have a 4*4 field
    //X X X X
    //X X X X
    //X X X X
    //X X X X
    //Target point is (2,2)
    //X X X X
    //X X X X
    //X X 0 X
    //X X X X
    //It should floof fill to give this
    //2 2 2 2
    //2 1 1 1
    //2 1 0 1
    //2 1 1 1
    
    //Let's try it

    
    //First we build a BFS algorithm, this will have the pathfinding logic
    void BFS()
    {
        //First we add to the queue the starting position to the queue
        Queue.Enqueue(CurrentPos);
        
        while (Queue.Count > 0)
        {
            //We set ourselves to the next position in queue, whilst removing it.
            CurrentPos = Queue.Dequeue();

            //If that position has not been visited, thus we can avoid revisiting 
            if (!Visited[CurrentPos])
            {
                //Thus we have visited it
                Visited[CurrentPos] = true;
                
                //Then we visit all neighbours, put them all in the queue to be checked next
                foreach (Vector2 direction in _vecArr)
                {
                    if (!Visited[CurrentPos + direction])
                    {
                        Queue.Enqueue(CurrentPos + direction);
                    }
                }
            }
        }
    }

    //Then the FloodFill which should contain the 'fill logic'
    //Since we are going to do Flow Field, we need to think about filling with numerical values, or costs for the AI
    void FloodFill(Vector2 PosToFill, int Depth)
    {
        //So for every neighbour we visited in BFS it should call Floodfill and fill in the cost associated.
        

        
    }
}