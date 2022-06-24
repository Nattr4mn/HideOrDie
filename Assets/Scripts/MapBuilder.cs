using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private UnityEvent _mapBuildComplete;
    [SerializeField] private Tilemap _floorTilemap;
    [SerializeField] private Tilemap _wallsTilemap;
    [SerializeField] private TileBase _wallTile;
    [SerializeField] private TileBase _floorTile;
    [SerializeField] private int _mapSize;
    private MapGenerator _map;

    public MapGenerator Map => _map;

    private void Awake()
    {
        BuildMap();
    }

    public void BuildMap()
    {
        _floorTilemap.ClearAllTiles();
        _wallsTilemap.ClearAllTiles();
        _map = new MapGenerator(_mapSize);
        _map.GenerateMap();
        var mapGraph = _map.Scheme;
        for (int x = 0; x < _mapSize + 2; x++)
        {
            for (int y = 0; y < _mapSize + 2; y++)
            {
                var tilePosition = new Vector3Int(x, y);
                SetTile(mapGraph, tilePosition);
            }
        }
        _mapBuildComplete?.Invoke();
    }

    private void SetTile(int[,] mapGraph, Vector3Int tilePosition)
    {
        if (mapGraph[tilePosition.y, tilePosition.x] == MapGenerator.WallMark)
        {
            _wallsTilemap.SetTile(tilePosition, _wallTile);
        }
        else if (mapGraph[tilePosition.y, tilePosition.x] == MapGenerator.FloorMark)
        {
            _floorTilemap.SetTile(tilePosition, _floorTile);
        }
    }

    public List<Vector2Int> GetFloorPositions()
    {
        return _map.GetFloorPositions();
    }
}
