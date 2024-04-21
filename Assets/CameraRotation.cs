using UnityEngine;
using System.Linq;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float _cameraDistance = 10f;
    [SerializeField] private  float _cameraHeight = 2f;
    [SerializeField] private  float _cameraRotationSpeed = 50f;
    [SerializeField] private float _circularAngle = 0f;
    [SerializeField] private bool _fixedHeight = false;
    
    private Transform _targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        _targetTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraTransform();
    }

    private void UpdateCameraTransform()
    {
        CalculateNewCameraPosition();
        LookAtTarget();
    }

    private void CalculateNewCameraPosition() {
        _circularAngle += _cameraRotationSpeed * Time.deltaTime;

        float newXOffset = Mathf.Sin(_circularAngle * Mathf.Deg2Rad) * _cameraDistance;
        float newZOffset = Mathf.Cos(_circularAngle * Mathf.Deg2Rad) * _cameraDistance;

        float newXPosition = (_targetTransform?.position.x ?? 0) + newXOffset;
        float newZPosition = (_targetTransform?.position.z ?? 0) + newZOffset;
        float newYPosition = _fixedHeight ? _cameraHeight : _targetTransform.position.y;

        transform.position = new Vector3(newXPosition, newYPosition, newZPosition);
    }
    private void LookAtTarget()
    {
        transform.LookAt(_targetTransform?.position ?? Vector3.zero);
    }
}
