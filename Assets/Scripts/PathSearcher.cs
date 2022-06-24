using System.Collections.Generic;
using UnityEngine;

public class PathSearcher
{
    private MapGenerator _map;

    public PathSearcher(MapGenerator map)
    {
        _map = map;
    }

    public List<Vector2> SearchWay(Vector2 startPosition, Vector2 endPosition)
    {
        Queue<Vector2> frontier = new Queue<Vector2>();
        frontier.Enqueue(startPosition);
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
        Vector2 current;
        while(frontier.Count > 0)
        {
            current = frontier.Dequeue();
            var neighbourPositions = GetNeighbourCells(current);
            foreach(var next in neighbourPositions)
            {
                var nextX = (int)next.x;
                var nextY = (int)next.y;
                if (!cameFrom.ContainsKey(next) && _map.Scheme[nextY, nextX] == MapGenerator.FloorMark)
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                }
            }
        }

        current = endPosition;
        List<Vector2> path = new List<Vector2>() { current };
        while(current != startPosition)
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Add(startPosition);
        path.Reverse();

        return path;
    }

    private List<Vector2> GetNeighbourCells(Vector2 startPoint)
    {
        List<Vector2> neighbourCells = new List<Vector2>();
        var up = new Vector2(startPoint.x, startPoint.y + 1);
        var down = new Vector2(startPoint.x, startPoint.y - 1);
        var left = new Vector2(startPoint.x - 1, startPoint.y);
        var right = new Vector2(startPoint.x + 1, startPoint.y);

        if (up.y <= _map.Height - 1) 
            neighbourCells.Add(up);
        if (down.y >= 0)
            neighbourCells.Add(down);
        if (left.x >= 0) 
            neighbourCells.Add(left);
        if (right.x <= _map.Width - 1) 
            neighbourCells.Add(right);

        return neighbourCells;
    }

}
