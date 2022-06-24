using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private NoiseSensor _noiseSensor;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _enemyQuantity;

    public void SpawnEnemies()
    {
        var floorPositions = _mapBuilder.GetFloorPositions();
        for(int i = 0; i< _enemyQuantity; i++)
        {
            var startPosition = GetPosition(ref floorPositions);
            var endPosition = GetPosition(ref floorPositions);
            var enemy = Instantiate(_enemyPrefab);
            enemy.Init(_mapBuilder, startPosition, endPosition);
            _noiseSensor?.DetectionEvent.AddListener(enemy.SetSearchState);
        }
    }

    private Vector2 GetPosition(ref List<Vector2Int> positions)
    {
        var positionIndex = Random.Range(0, positions.Count);
        var position = positions[positionIndex];
        positions.Remove(position);
        return position;
    }
}

