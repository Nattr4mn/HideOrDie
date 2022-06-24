using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector3> _mapBuildComplete;
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
        ClearMap();
        FillingMap(_map.GenerateMap());
        SetExit();
        EntryInvisibleWall();
        _mapBuildComplete?.Invoke(_map.EntryPosition);
    }

    private void ClearMap()
    {
        _floorTilemap.ClearAllTiles();
        _wallsTilemap.ClearAllTiles();
        _map = new MapGenerator(_mapSize);
    }

    private void FillingMap(int[,] mapScheme)
    {
        for (int x = 0; x < _mapSize + 2; x++)
        {
            for (int y = 0; y < _mapSize + 2; y++)
            {
                var tilePosition = new Vector3Int(x, y);
                SetTile(mapScheme, tilePosition);
            }
        }
    }

    private void SetTile(int[,] mapScheme, Vector3Int tilePosition)
    {
        var mapMark = mapScheme[tilePosition.y, tilePosition.x];
        if (mapMark == MapGenerator.WallMark)
        {
            _wallsTilemap.SetTile(tilePosition, _wallTile);
        }
        else if (mapMark == MapGenerator.FloorMark)
        {
            _floorTilemap.SetTile(tilePosition, _floorTile);
        }
    }

    private void SetExit()
    {
        var finishObject = FindObjectOfType<Finish>();
        finishObject.transform.position = _map.ExitPosition;
    }

    private void EntryInvisibleWall()
    {
        var gameObject = new GameObject("Invisible wall");
        gameObject.AddComponent<BoxCollider2D>();
        var newPosition = _map.EntryPosition;
        if (_map.EntryPosition.x == 0)
        {
            newPosition.x -= 1;
        }
        else if (_map.EntryPosition.x == _map.Width - 1)
        {
            newPosition.x += 1;
        }
        else if (_map.EntryPosition.y == 0)
        {
            newPosition.y -= 1;
        }
        else if (_map.EntryPosition.y == _map.Height - 1)
        {
            newPosition.y += 1;
        }
        gameObject.transform.position = newPosition;
    }

    public List<Vector2Int> GetFloorPositions()
    {
        return _map.GetFloorPositions();
    }
}
