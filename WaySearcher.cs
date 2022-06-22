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

        while(frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            foreach(var x in _graph)
            {
                if(!visited.Contains(next))
                {
                    frontier.Enqueue(next);
                    visited.Add(next);
                }
            }
        }
    }
}