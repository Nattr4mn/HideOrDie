﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator
{
    public const int WallMark = 0;
    public const int FloorMark = 1;

    private int[,] _map;
    private int _mapWidth;
    private int _mapHeight;
    private Vector2 _startPosition;
    private Vector2 _endPosition;

    public int[,] Map => _map;
    public Vector2 StartPosition => _startPosition;
    public Vector2 EndPosition => _endPosition;

    public MapGenerator(int mapSize):
        this(mapSize, mapSize)
    {
    }

    public MapGenerator(int mapWidth, int mapHeight)
    {
        _mapWidth = mapWidth + 2;
        _mapHeight = mapHeight + 2;
        _map = new int[_mapWidth, _mapHeight];
        SetWalls();
        GenerateSpawnPoint();
    }

    private void SetWalls()
    {
        for (int y = 1; y < _mapHeight - 1; y++)
        {
            for (int x = 1; x < _mapWidth - 1; x++)
            {
                if (y % 2 != 0 && x % 2 != 0)
                {
                    _map[y, x] = FloorMark;
                }
            }
        }
    }

    public void GenerateMap()
    {
        List<Vector2> unvisitedCells = GetUnvisitedCells();
        Stack<Vector2> visitedCells = new Stack<Vector2>();
        Vector2 startCell = new Vector2(1f, 1f);
        Vector2 currentCell = startCell;
        visitedCells.Push(currentCell);
        unvisitedCells.Remove(currentCell);

        while (unvisitedCells.Count > 0)
        {
            var neighbourCells = GetNeighbourCells(unvisitedCells, currentCell);
            if(neighbourCells.Count > 0)
            {
                var neighbourCellIndex = Random.Range(0, neighbourCells.Count);
                Vector2 neighbourCell = neighbourCells[neighbourCellIndex];
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

    private List<Vector2> GetUnvisitedCells()
    {
        List<Vector2> unvisitedCells = new List<Vector2>();
        for (int y = 1; y < _mapHeight - 1; y++)
        {
            for (int x = 1; x < _mapWidth - 1; x += 2)
            {
                if (_map[y, x] == FloorMark)
                {
                    unvisitedCells.Add(new Vector2(x, y));
                }
            }
        }
        return unvisitedCells;
    }

    private List<Vector2> GetNeighbourCells(List<Vector2> unvisitedCells, Vector2 startPoint)
    {
        List<Vector2> neighbourCells = new List<Vector2>();
        var up = new Vector2(startPoint.x, startPoint.y - 2);
        var down = new Vector2(startPoint.x, startPoint.y + 2);
        var left = new Vector2(startPoint.x - 2, startPoint.y);
        var right = new Vector2(startPoint.x + 2, startPoint.y);

        if (up.y > 0 && unvisitedCells.Contains(up)) neighbourCells.Add(up);
        if (down.y < _mapHeight - 1 && unvisitedCells.Contains(down)) neighbourCells.Add(down);
        if (left.x > 0 && unvisitedCells.Contains(left)) neighbourCells.Add(left);
        if (right.x < _mapWidth - 1 && unvisitedCells.Contains(right)) neighbourCells.Add(right);

        return neighbourCells;
    }

    private void RemoveWall(Vector2 startCell, Vector2 endCell)
    {
        var xDifference = endCell.x - startCell.x;
        var yDifference = endCell.y - startCell.y;
        Vector2 target;
        var targetX = (xDifference != 0) ? (xDifference / Mathf.Abs(xDifference)) : 0;
        var targetY = (yDifference != 0) ? (yDifference / Mathf.Abs(yDifference)) : 0;

        target = new Vector2(startCell.x + targetX, startCell.y + targetY);
        _map[(int)target.y, (int)target.x] = FloorMark;
    }

    private void GenerateSpawnPoint()
    {
        int[] xBorderPositions = new int[2] { 0, _mapWidth - 1 };
        int xPosition = xBorderPositions[Random.Range(0, 1)];
        int[] yBorderPositions = new int[2] { 1, _mapHeight - 2 };
        int yPosition = yBorderPositions[Random.Range(0, 1)];

        _startPosition = new Vector2(xPosition, yPosition);
        _map[xPosition, yPosition] = FloorMark;

        xPosition = xBorderPositions.First(position => position != xPosition);
        yPosition = yBorderPositions.First(position => position != yPosition);

        _endPosition = new Vector2(xPosition, yPosition);
        _map[xPosition, yPosition] = FloorMark;
    }
}