using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float speed = 0;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    public TextMeshProUGUI countText; // ref to UI text component
    public GameObject winTextObject;

    //private variables
    private float movementX;
    private float movementY;
    private int count;

    // Start is called before the first frame update
    // check every frame for player input
    // apply that to player game object as movement
    // fixed update <= called just before physics calculations
    private void Start()
    {
        Input.gyro.enabled = true;//for our bonus - make the gyro enabled
        count = 0; //sets initial count to zero
        SetCountText();

        winTextObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        MouseMovement();

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            //if the game object has a pickup tag, set active status to false
            other.gameObject.SetActive(false);
            //every time "coin" gets picked up add 1 to the count
            count += 1;
            SetCountText();
        }
    }

    private void OnMove(InputValue movementVal)
    {
        //getting input value from player
        // gets vector 2 data from movement value and stores it into the vector2 variable = movementVector
        Vector2 movementVector = movementVal.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void MouseMovement()
    {
        //check if we have any mouse inputs
        if (Input.GetMouseButton(0))
        {
            //if we do then we create a raycast from our screen to the world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //if the ray hits something on the ground layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
            {
                //we can store the position of the hit point, and calculate the direction from the player to that point
                Vector3 targetPosition = hit.point;
                Vector3 direction = (targetPosition - transform.position);
                direction.y = 0f;
                direction.Normalize();
                //and now set our movement vectors to that direction
                movementX = direction.x;
                movementY = direction.z;
            }
        }
        //otherwise we set movement to zero
        else
        {
            movementX = 0;
            movementY = 0;
            rb.angularVelocity = Vector3.zero;
            return;
        }
    }

   
    private void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }
}
