using UnityEngine;
using UnityEngine.Events;

public class NoiseSensor : MonoBehaviour
{
    public UnityEvent<float> NoiseEvent;
    public UnityEvent<Transform> DetectionEvent;

    [SerializeField] private Rigidbody2D _rigidbodyNoiseSource;
    [SerializeField] private float _maxNoiseLevel;
    private float _noiseLevel;
    private float _noiseIncreaseTimer;
    private float _noiseReductionTimer;

    private void Update()
    {
        if (_rigidbodyNoiseSource.velocity != Vector2.zero)
        {
            NoiseIncrease();
        }
        else if (_noiseLevel > 0f)
        {
            NoiseReduction();
        }
    }

    private void NoiseIncrease()
    {
        _noiseReductionTimer = 0f;
        _noiseIncreaseTimer += Time.deltaTime;
        if (_noiseIncreaseTimer >= 1f)
        {
            _noiseIncreaseTimer = 0f;
            _noiseLevel += 3f; 
            CheckNoiseLevel();
            NoiseEvent?.Invoke(_noiseLevel / _maxNoiseLevel);
        }
    }

    private void CheckNoiseLevel()
    {
        if (_noiseLevel >= _maxNoiseLevel)
        {
            _noiseLevel = Mathf.Clamp(_noiseLevel, 0f, _maxNoiseLevel);
            DetectionEvent?.Invoke(_rigidbodyNoiseSource.transform);
        }
    }

    private void NoiseReduction()
    {
        _noiseIncreaseTimer = 0f;
        _noiseReductionTimer += Time.deltaTime;
        if (_noiseReductionTimer >= 0.5f)
        {
            _noiseReductionTimer = 0f;
            _noiseLevel -= 1;
            NoiseEvent?.Invoke(_noiseLevel / _maxNoiseLevel);
        }
    }
}
