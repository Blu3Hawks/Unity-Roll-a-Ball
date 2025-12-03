using UnityEngine;

public class GyroscopeMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 10;

    private Gyroscope _gyro;
    private bool _gyroEnabled;
    private Vector2 _tilt;


    private void Start()
    {
        _gyroEnabled = EnableGyro();
        Debug.Log("Gyro enabled: " + _gyroEnabled);
    }

    private void Update()
    {
        if (!_gyroEnabled)
            return;
        //get the gravity vector from the gyroscope
        Vector3 gravity = _gyro.gravity;

        //normalize the tilt vector to get direction only
        _tilt = new Vector2(gravity.x, gravity.y).normalized;
        Debug.Log("Tilt: " + _tilt);
    }

    private void FixedUpdate()
    {
        //safety checks
        if (!_gyroEnabled || _rb == null)
            return;

        //and now we move the rigidbody based on the tilt
        Vector3 move = new Vector3(_tilt.x, 0f, _tilt.y) * _speed;
        _rb.AddForce(move);
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
}
