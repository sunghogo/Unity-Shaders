using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] [Range(1, 50)] private float _cameraDistance = 3f;
    [SerializeField] [Range(-25, 25)] private  float _cameraHeight = 0.6f;
    [SerializeField] [Range(25, 100)] private  float _cameraRotationSpeed = 50f;
    [SerializeField] [Range(25, 100)] private  float _cameraZoomSpeed = 25f;
    [SerializeField] private float _circularAngle = 0f;
    [SerializeField] private bool _fixedHeight = false;
    [SerializeField] private bool _manualRotation = false;
    private float _closeUpX = -0.1f;
    private float _closeUpMax = 8f;
    private float _closeUpHeight = 0.6f;
    private float _closeUpMin = 2f;
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

        if (Input.GetKey(KeyCode.UpArrow)) ZoomIn();
        if (Input.GetKey(KeyCode.DownArrow)) ZoomOut();

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
            if (Input.GetKey(KeyCode.LeftArrow)) RotateLeft();
            if (Input.GetKey(KeyCode.RightArrow)) RotateRight();
        } else {
            _manualRotation = false;
        }

    }

    private void UpdateCameraTransform()
    {
        CalculateNewCameraPosition();
        LookAtTarget();
    }

    private void CalculateNewCameraPosition() {
        if (!_manualRotation) _circularAngle += _cameraRotationSpeed * Time.deltaTime;
        if (_cameraDistance < _closeUpMax) { 
            _fixedHeight = true;
            InterpolateCameraHeight();
        } else _fixedHeight = false;

        float newXOffset = Mathf.Sin(_circularAngle * Mathf.Deg2Rad) * _cameraDistance;
        float newZOffset = Mathf.Cos(_circularAngle * Mathf.Deg2Rad) * _cameraDistance;

        float newXPosition = (_targetTransform?.position.x ?? 0) + newXOffset + InterpolateCameraX();
        float newZPosition = (_targetTransform?.position.z ?? 0) + newZOffset;
        float newYPosition = _fixedHeight ? _cameraHeight : _targetTransform.position.y;

        transform.position = new Vector3(newXPosition, newYPosition, newZPosition);
    }
    private void LookAtTarget()
    {
        transform.LookAt(_targetTransform?.position ?? Vector3.zero);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void ZoomOut() {
        _cameraDistance = Mathf.Clamp(_cameraDistance + Time.deltaTime * _cameraZoomSpeed, _closeUpMin, 100);
    }

    private void ZoomIn() {
        _cameraDistance = Mathf.Clamp(_cameraDistance - Time.deltaTime * _cameraZoomSpeed, _closeUpMin, 100);
    }

    private void RotateLeft() {
        _manualRotation = true;
        _circularAngle -= Time.deltaTime * _cameraRotationSpeed * 2;
    }

    private void RotateRight() {
        _manualRotation = true;
        _circularAngle += Time.deltaTime * _cameraRotationSpeed * 2;
    }

    private void InterpolateCameraHeight() {
        float linearFormula = _cameraDistance >= _closeUpMax ? 1 : (1 - ((_cameraDistance - _closeUpMin) / (_closeUpMax - _closeUpMin)));
        _cameraHeight = linearFormula * _closeUpHeight; 
    }

    private float InterpolateCameraX() {
        float linearFormula = _cameraDistance >= _closeUpMax ? 1 : (1 - ((_cameraDistance - _closeUpMin) / (_closeUpMax - _closeUpMin)));
        return linearFormula * _closeUpX; 
    }
}
