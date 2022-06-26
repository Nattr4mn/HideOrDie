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

    public Vector2 TryGetNextPosition(Vector2 startPoint, Vector2 endPoint)
    {
        try
        {
            return GetNextPosition(startPoint, endPoint);
        }
        catch(Exception exception)
        {
            Debug.LogWarning(exception);
            return startPoint;
        }
    }

    private Vector2 GetNextPosition(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 startVector = CorrectionPosition(startPoint);
        var newPaths = CreatePath(startPoint, endPoint);
        return newPaths.First(path => path != startVector);
    }

    private Vector2 CorrectionPosition(Vector2 position)
    {
        float startVectorX = (float)Math.Round(position.x);
        float startVectorY = (float)Math.Round(position.y);
        return new Vector2(startVectorX, startVectorY);
    }

    public List<Vector2> CreatePath(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 startVector = CorrectionPosition(startPoint);
        Vector2 targetVector = CorrectionPosition(endPoint);
        return SearchWay(startVector, targetVector);
    }

    public List<Vector2> SearchWay(Vector2 startPosition, Vector2 endPosition)
    {
        var cameFrom = WidthTraversal(startPosition);
        return TryFindingPaths(cameFrom, startPosition, endPosition);
    }

    private Dictionary<Vector2, Vector2> WidthTraversal(Vector2 startPosition)
    {
        Queue<Vector2> frontier = new Queue<Vector2>();
        frontier.Enqueue(startPosition);
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
        Vector2 current;

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();
            var neighbourPositions = GetNeighbourCells(current);
            foreach (var nextPosition in neighbourPositions)
            {
                var notContainsPosition = !cameFrom.ContainsKey(nextPosition);
                var nextPositionIsFloor = _map.Scheme[(int)nextPosition.y, (int)nextPosition.x] == MapGenerator.FloorMark;
                if (notContainsPosition && nextPositionIsFloor)
                {
                    frontier.Enqueue(nextPosition);
                    cameFrom.Add(nextPosition, current);
                }
            }
        }

        return cameFrom;
    }

    private List<Vector2> TryFindingPaths(Dictionary<Vector2, Vector2> cameFrom, Vector2 startPosition, Vector2 endPosition)
    {
        try
        {
            return FindingPaths(cameFrom, startPosition, endPosition);
        }
        catch(Exception exception)
        {
            Debug.LogWarning(exception);
            return new List<Vector2> { startPosition };
        }
    }

    private List<Vector2> FindingPaths(Dictionary<Vector2, Vector2> cameFrom, Vector2 startPosition, Vector2 endPosition)
    {
        Vector2 currentPosition = endPosition;
        List<Vector2> path = new List<Vector2>() { currentPosition };
        while (currentPosition != startPosition)
        {
            currentPosition = cameFrom[currentPosition];
            path.Add(currentPosition);
        }
        path.Add(currentPosition);
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