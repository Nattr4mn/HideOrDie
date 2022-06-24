using UnityEngine;
using UnityEngine.Events;

public class EnemyVision : MonoBehaviour
{
    public UnityEvent<Transform> DetectEvent;

    [SerializeField] private LayerMask _visionLayerMask;
    [SerializeField] private Mesh _fieldOfViewMesh;
    [SerializeField] private float _fieldOfView = 90f;
    [SerializeField] private float _viewDistance = 2f;
    [SerializeField] private int _rayVisionCount = 50;
    private Vector3 _origin;
    private float _fieldOfViewAngle;
    private float _angleBetweenRays;

    private void Start()
    {
        _fieldOfViewMesh = new Mesh();
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = _fieldOfViewMesh;
        _origin = Vector3.zero;
        _angleBetweenRays = _fieldOfView / _rayVisionCount;
    }

    private void LateUpdate()
    {
        float currentAngle = _fieldOfViewAngle;
        Vector3[] vertices = new Vector3[_rayVisionCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[_rayVisionCount * 3];

        vertices[0] = _origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= _rayVisionCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D detectionHit = Physics2D.Raycast(_origin, GetVectorFromAngle(currentAngle), _viewDistance, _visionLayerMask);
            if (detectionHit.collider == null)
            {
                vertex = _origin + GetVectorFromAngle(currentAngle) * _viewDistance;
            }
            else
            {
                vertex = detectionHit.point;
                if (detectionHit.transform.TryGetComponent(out Player player))
                {
                    DetectEvent?.Invoke(detectionHit.transform);
                }
                Debug.Log("Detect " + detectionHit.transform.name);
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            currentAngle -= _angleBetweenRays;
        }


        _fieldOfViewMesh.vertices = vertices;
        _fieldOfViewMesh.uv = uv;
        _fieldOfViewMesh.triangles = triangles;
        _fieldOfViewMesh.bounds = new Bounds(_origin, Vector3.one * _viewDistance);
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        _fieldOfViewAngle = GetAngleFromVectorFloat(aimDirection) + _fieldOfView / 2f;
    }

    public void SetOrigin(Vector3 origin)
    {
        _origin = origin;
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.Rad2Deg / _rayVisionCount);
        return new Vector3(Mathf.Sin(angleRad), Mathf.Cos(angleRad));
    }

    private void OnDrawGizmos()
    {
       Gizmos.DrawRay(_origin, transform.up); 
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
