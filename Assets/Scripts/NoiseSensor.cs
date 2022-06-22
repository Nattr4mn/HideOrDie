using UnityEngine;
using UnityEngine.Events;

public class NoiseSensor : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> _detectionEvent;
    [SerializeField] private Rigidbody2D _rigidbodyNoiseSource;
    [SerializeField] private float _maxNoiseLevel;
    private float _noise;
    private float _noiseTimer;

    private void Update()
    {
        if (_rigidbodyNoiseSource.velocity != Vector2.zero)
        {
            _noiseTimer += Time.deltaTime;
            if (_noiseTimer > 1f)
            {
                _noiseTimer = 0f;
                _noise += 3f;
                if (_noise >= _maxNoiseLevel)
                {
                    _noise = Mathf.Clamp(_noise, 0f, _maxNoiseLevel);
                }
                _detectionEvent?.Invoke(_noise / _maxNoiseLevel);
            }

        }
        else if (_noise > 0f)
        {
            _noiseTimer += Time.deltaTime;
            if (_noiseTimer > 0.5f)
            {
                _noiseTimer = 0f;
                _noise -= 1;
                _detectionEvent?.Invoke(_noise / _maxNoiseLevel);
            }
        }
    }
}
