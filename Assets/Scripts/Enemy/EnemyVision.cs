using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshFilter))]
public class EnemyVision : MonoBehaviour
{
    public UnityEvent<Transform> DetectEvent;

    [SerializeField] private LayerMask _visionLayerMask;
    [SerializeField] private Mesh _fieldOfViewMesh;
    [SerializeField] private float _fieldOfView = 90f;
    [SerializeField] private float _viewDistance = 2f;
    [SerializeField] private int _rayVisionCount = 50;
    private float _fieldOfViewAngle;
    private float _angleBetweenRays;

    private void Start()
    {
        _fieldOfViewMesh = new Mesh();
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = _fieldOfViewMesh;
        _angleBetweenRays = _fieldOfView / _rayVisionCount;
    }

    public void Scan(Vector3 originPoint)
    {
        float currentAngle = _fieldOfViewAngle;
        Vector3[] vertices = new Vector3[_rayVisionCount + 2];
        int[] triangles = new int[_rayVisionCount * 3];

        vertices[0] = Vector3.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= _rayVisionCount; i++)
        {
            var direction = transform.TransformDirection(GetVectorFromAngle(currentAngle));
            RaycastHit2D detectionHit = Physics2D.Raycast(originPoint, direction, _viewDistance, _visionLayerMask);

            CheckDetection(detectionHit);
            vertices[vertexIndex] = GetVectorFromAngle(currentAngle) * _viewDistance;

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
        _fieldOfViewMesh.triangles = triangles;
        _fieldOfViewMesh.bounds = new Bounds(Vector3.zero, Vector3.one * _viewDistance);
    }

    private void CheckDetection(RaycastHit2D detectionHit)
    {
        if (detectionHit.collider != null)
        {
            if (detectionHit.transform.TryGetComponent(out Player player))
            {
                DetectEvent?.Invoke(player.transform);
            }
        }
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        _fieldOfViewAngle = GetAngleFromVectorFloat(aimDirection) + _fieldOfView / 2f;
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
