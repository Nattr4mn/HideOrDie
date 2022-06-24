using UnityEngine;
using UnityEngine.Events;

public class NoiseSensor : MonoBehaviour
{
    public UnityEvent<float> NoiseEvent;
    public UnityEvent<Transform> DetectionEvent;

    [SerializeField] private Rigidbody2D _rigidbodyNoiseSource;
    [SerializeField] private float _maxNoiseLevel;
    private float _noise;
    private float _noiseTimer;

    private void Update()
    {
        _noiseTimer += Time.deltaTime;
        if (_rigidbodyNoiseSource.velocity != Vector2.zero)
        {
            NoiseIncrease();
        }
        else if (_noise > 0f)
        {
            NoiseReduction();
        }
    }

    private void NoiseIncrease()
    {
        if (_noiseTimer >= 1f)
        {
            _noiseTimer = 0f;
            _noise += 3f; 
            CheckNoiseLevel();
            NoiseEvent?.Invoke(_noise / _maxNoiseLevel);
        }
    }

    private void CheckNoiseLevel()
    {
        if (_noise >= _maxNoiseLevel)
        {
            _noise = Mathf.Clamp(_noise, 0f, _maxNoiseLevel);
            DetectionEvent?.Invoke(_rigidbodyNoiseSource.transform);
        }
    }

    private void NoiseReduction()
    {
        if (_noiseTimer >= 0.5f)
        {
            _noiseTimer = 0f;
            _noise -= 1;
            NoiseEvent?.Invoke(_noise / _maxNoiseLevel);
        }
    }
}
