using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap _floorTilemap;
    [SerializeField] private Tilemap _wallsTilemap;
    [SerializeField] private TileBase _wallTile;
    [SerializeField] private TileBase _floorTile;
    [SerializeField] private int _mapSize;
    private MapGenerator _map;

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
        _map.GenerateMap();
        var mapGraph = _map.Map;
        for (int x = 0; x < _mapSize + 2; x++)
        {
            for (int y = 0; y < _mapSize + 2; y++)
            {
                var tilePosition = new Vector3Int(x, y);
                SetTile(mapGraph, tilePosition);
            }
        }
    }

    private void SetTile(int[,] mapGraph, Vector3Int tilePosition)
    {
        if (mapGraph[tilePosition.y, tilePosition.x] == MapGenerator.WallMark)
        {
            _floorTilemap.SetTile(tilePosition, _wallTile);
        }
        else if (mapGraph[tilePosition.y, tilePosition.x] == MapGenerator.FloorMark)
        {
            _floorTilemap.SetTile(tilePosition, _floorTile);
        }
    }
}
