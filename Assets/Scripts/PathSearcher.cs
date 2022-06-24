using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathSearcher
{
    private MapGenerator _map;

    public PathSearcher(MapGenerator map)
    {
        _map = map;
    }

    public Vector2 GetNextPosition(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 startVector = CorrectionPosition(startPoint);
        var newPaths = CreatePath(startPoint, endPoint);
        return newPaths.First(path => path != startVector);
    }

    public List<Vector2> CreatePath(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 startVector = CorrectionPosition(startPoint);
        Vector2 targetVector = CorrectionPosition(endPoint);
        return SearchWay(startVector, targetVector);
    }

    private Vector2 CorrectionPosition(Vector2 position)
    {
        float startVectorX = (float)Math.Round(position.x);
        float startVectorY = (float)Math.Round(position.y);
        return new Vector2(startVectorX, startVectorY);
    }

    public List<Vector2> SearchWay(Vector2 startPosition, Vector2 endPosition)
    {
        var cameFrom = SearchEndPosition(startPosition);
        return BuildingPathToGoal(cameFrom, startPosition, endPosition);
    }

    private Dictionary<Vector2, Vector2> SearchEndPosition(Vector2 startPosition)
    {
        Queue<Vector2> frontier = new Queue<Vector2>();
        frontier.Enqueue(startPosition);
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
        Vector2 current;
        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();
            var neighbourPositions = GetNeighbourCells(current);
            foreach (var next in neighbourPositions)
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

        return cameFrom;
    }

    private List<Vector2> BuildingPathToGoal(Dictionary<Vector2, Vector2> cameFrom, Vector2 startPosition, Vector2 endPosition)
    {
        Vector2 current = endPosition;
        List<Vector2> path = new List<Vector2>() { current };
        while (current != startPosition)
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