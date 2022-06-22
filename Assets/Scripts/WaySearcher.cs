using System.Collections.Generic;
using UnityEngine;

public class WaySearcher
{
    private MapGenerator _map;

    public WaySearcher(MapGenerator map)
    {
        _map = map;
    }

    public List<Vector2Int> SearchWay(Vector2Int startPosition, Vector2Int endPosition)
    {
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(startPosition);
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Vector2Int current;
        while(frontier.Count > 0)
        {
            current = frontier.Dequeue();
            var neighbourPositions = GetNeighbourCells(current);
            foreach(var next in neighbourPositions)
            {
                if(!cameFrom.ContainsKey(next) && _map.Scheme[next.y, next.x] == MapGenerator.FloorMark)
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                }
            }
        }

        current = endPosition;
        List<Vector2Int> path = new List<Vector2Int>() { current };
        while(current != startPosition)
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Add(startPosition);
        path.Reverse();

        return path;
    }

    private List<Vector2Int> GetNeighbourCells(Vector2Int startPoint)
    {
        List<Vector2Int> neighbourCells = new List<Vector2Int>();
        var up = new Vector2Int(startPoint.x, startPoint.y - 1);
        var down = new Vector2Int(startPoint.x, startPoint.y + 1);
        var left = new Vector2Int(startPoint.x - 1, startPoint.y);
        var right = new Vector2Int(startPoint.x + 1, startPoint.y);

        if (up.y > 0) 
            neighbourCells.Add(up);
        if (down.y < _map.Height - 1)
            neighbourCells.Add(down);
        if (left.x > 0) 
            neighbourCells.Add(left);
        if (right.x < _map.Width - 1) 
            neighbourCells.Add(right);

        return neighbourCells;
    }

}
