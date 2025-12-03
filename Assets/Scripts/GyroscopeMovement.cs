using UnityEngine;

public class GyroscopeMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _threshhold = 0.2f;
    
    private Gyroscope _gyro;
    private bool _gyroEnabled;
    private Vector3 _tiltDir;

#if UNITY_ANDROID || UNITY_IOS
    private void Start()
    {
        _tiltDir = Vector3.zero;
        _gyroEnabled = EnableGyro();
        Debug.Log("Gyro enabled: " + _gyroEnabled);
    }

    private void Update()
    {
        CalculateGyroDirection();
    }

    private void FixedUpdate()
    {
        ApplyGyroMovement();
    }

    private void ApplyGyroMovement()
    {
        //safety checks
        if (!_gyroEnabled || _rb == null)
            return;

        //and now we move the rigidbody based on the tilt
        _rb.AddForce(_tiltDir * _speed, ForceMode.Acceleration);
    }

    private void CalculateGyroDirection()
    {
        if (!_gyroEnabled)
            return;
        //get the gravity vector from the gyroscope
        Vector3 gravity = _gyro.gravity;

        //normalize the tilt vector to get direction only

        Vector3 tilt = new Vector3(gravity.x, 0f, gravity.y).normalized;
        if (tilt.magnitude < _threshhold)
        {
            _tiltDir = Vector3.zero;
        }
        else
        {
            _tiltDir = tilt.normalized;
        }
        Debug.Log("Tilt: " + tilt);
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            _gyro = Input.gyro;
            _gyro.enabled = true;
            return true;
        }
        return false;
    }
    
#endif
}
