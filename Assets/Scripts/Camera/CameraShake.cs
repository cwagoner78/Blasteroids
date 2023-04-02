using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Camera Information")]
    private Transform _cameraTransform;
    private Vector3 _originalCameraPos;

    [Header("Shake Parameters")]
    [SerializeField] float _shakeDuration = 2f;
    [SerializeField] float _shakeAmount = 0.7f;

    private bool _canShake = false;
    private float _shakeTimer;


    private void Start()
    {
        _cameraTransform = GetComponent<Transform>();
    }

    void Update()
    {
        _originalCameraPos = transform.localPosition;

        if (_canShake) StartCameraShakeEffect();
    }

    public void ShakeCamera()
    {
        _canShake = true;
        _shakeTimer = _shakeDuration;
    }

    void StartCameraShakeEffect()
    {
        if (_shakeTimer > 0)
        {
            _cameraTransform.localPosition = _originalCameraPos + Random.insideUnitSphere * _shakeAmount;
            _shakeTimer -= Time.deltaTime;
        }
        else
        {
            _shakeTimer = 0f;
            _cameraTransform.position = _originalCameraPos;
            _canShake = false;
        }
    }
}