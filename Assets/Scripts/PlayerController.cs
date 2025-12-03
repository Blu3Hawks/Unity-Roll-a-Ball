using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _speed = 0;

    [Header("References")]
    [SerializeField] private Rigidbody _rb;
    public TextMeshProUGUI _countText; // ref to UI text component
    public GameObject _winTextObject;

    //private variables
    private float _movementX;
    private float _movementY;
    private int _count;
    private bool _isMovingWithMouse = false;

    // Start is called before the first frame update
    // check every frame for player input
    // apply that to player game object as movement
    // fixed update <= called just before physics calculations
    private void Start()
    {
        _count = 0; //sets initial count to zero
        SetCountText();

        _winTextObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        MouseMovement();
        KeyboardMovement();
    }

    private void KeyboardMovement()
    {
        Vector3 movement = new Vector3(_movementX, 0.0f, _movementY);
        _rb.AddForce(movement * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            //if the game object has a pickup tag, set active status to false
            other.gameObject.SetActive(false);
            //every time "coin" gets picked up add 1 to the count
            _count += 1;
            SetCountText();
        }
    }

    private void OnMove(InputValue movementVal)
    {
        //getting input value from player
        // gets vector 2 data from movement value and stores it into the vector2 variable = movementVector
        Vector2 movementVector = movementVal.Get<Vector2>();
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }

    private void MouseMovement()
    {

        //check if we have any mouse inputs
        if (Input.GetMouseButton(0))
        {
            _isMovingWithMouse = true;
            //if we do then we create a raycast from our screen to the world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //if the ray hits something on the ground layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundMask))
            {
                //we can store the position of the hit point, and calculate the direction from the player to that point
                Vector3 targetPosition = hit.point;
                Vector3 direction = (targetPosition - transform.position);
                direction.y = 0f;
                direction.Normalize();
                //and now set our movement vectors to that direction
                _movementX = direction.x;
                _movementY = direction.z;
            }
        }
        //otherwise we set movement to zero
        else
        {
            if (_isMovingWithMouse)
            {
                _movementX = 0f;
                _movementY = 0f;
                _rb.linearVelocity = Vector3.zero;
                _isMovingWithMouse = false;
            }
            return;
        }
    }


    private void SetCountText()
    {
        _countText.text = "Count: " + _count.ToString();
        if (_count >= 12)
        {
            _winTextObject.SetActive(true);
        }
    }
}
