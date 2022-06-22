using System.Collections.Generic;
using UnityEngine;

public class WaySearcher
{
    private int[,] _graph;

    public WaySearcher(int[,] graph)
    {
        _graph = graph;
    }

    public HashSet<Vector2> WaySearch(Vector2 start, Vector2 goal)
    {
        var frontier = new Queue<Vector2>();
        frontier.Enqueue(start);
        var visited = new HashSet<Vector2>();
        visited.Add(start);

        while (unvisitedCells.Count > 0)
        {
            var neighbourCells = GetNeighbourCells(unvisitedCells, currentCell);
            if (neighbourCells.Count > 0)
            {
                Vector2Int neighbourCell = neighbourCells[neighbourCellIndex];
                RemoveWall(currentCell, neighbourCell);
                currentCell = neighbourCell;
                visitedCells.Push(currentCell);
                unvisitedCells.Remove(currentCell);
            }
            else if (visitedCells.Count > 0)
            {
                currentCell = visitedCells.Pop();
            }
            else
            {
                var randomCellIndex = Random.Range(0, unvisitedCells.Count);
                currentCell = unvisitedCells[randomCellIndex];
                unvisitedCells.Remove(currentCell);
                visitedCells.Push(currentCell);
            }
        }
    }

    private void NextPosition(Vector2Int startCell, Vector2Int endCell)
    {
        var xDifference = endCell.x - startCell.x;
        var yDifference = endCell.y - startCell.y;
        Vector2Int target;
        var targetX = (xDifference != 0) ? (xDifference / Mathf.Abs(xDifference)) : 0;
        var targetY = (yDifference != 0) ? (yDifference / Mathf.Abs(yDifference)) : 0;

        target = new Vector2Int(startCell.x + targetX, startCell.y + targetY);
        _map[target.y, target.x] = FloorMark;
    }


    private List<Vector2Int> GetNeighbourCells(List<Vector2Int> unvisitedCells, Vector2Int startPoint)
    {
        List<Vector2Int> neighbourCells = new List<Vector2Int>();
        var up = new Vector2Int(startPoint.x, startPoint.y - 1);
        var down = new Vector2Int(startPoint.x, startPoint.y + 1);
        var left = new Vector2Int(startPoint.x - 1, startPoint.y);
        var right = new Vector2Int(startPoint.x + 1, startPoint.y);

        if (up.y > 0 && unvisitedCells.Contains(up)) neighbourCells.Add(up);
        if (down.y < _mapHeight - 1 && unvisitedCells.Contains(down)) neighbourCells.Add(down);
        if (left.x > 0 && unvisitedCells.Contains(left)) neighbourCells.Add(left);
        if (right.x < _mapWidth - 1 && unvisitedCells.Contains(right)) neighbourCells.Add(right);

        return neighbourCells;
    }
}