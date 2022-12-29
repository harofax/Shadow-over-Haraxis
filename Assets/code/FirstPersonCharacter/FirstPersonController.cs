using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]

public class FirstPersonController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpPower = 8.0f;

    public float gravity = 9.81f;
    
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 85.0f;

    public bool useGravity = true;

    private bool playerGrounded;

    CharacterController characterController;
    Vector3 player_velocity = Vector3.zero;
    float rotationX = 0;
    
    public bool canLook = true;
    public bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Shift to run
        bool is_running = Input.GetKey(KeyCode.LeftShift);

        float move_speed =  is_running ?
                            runningSpeed :
                            walkingSpeed;
        
        bool player_grounded = characterController.isGrounded;

        // newton
        if (player_grounded)
        {
            player_velocity.y = 0;
        }
        
        // if you get skiing, go to settings -> input -> axis -> set gravity to 5.5, sensitivity to 7
        Vector3 move_right = transform.forward *  Input.GetAxis("Vertical");
        Vector3 move_forward = transform.right * Input.GetAxis("Horizontal");

        float prev_y_velocity = player_velocity.y;

        player_velocity = move_right + move_forward;
        
        // clamp to make diagonal not faster (and use clamp so analogue stick can move less than max speed)
        player_velocity = Vector3.ClampMagnitude(player_velocity * move_speed, move_speed);

        if (Input.GetButton("Jump") && player_grounded)
        {
            player_velocity.y = jumpPower;
        }
        else
        {
            player_velocity.y = prev_y_velocity;
        }
        
        // if in air, fall down lol
        if (!player_grounded && useGravity)
        {
            player_velocity.y -= gravity * Time.deltaTime;
        }

        if (canMove)
        {
            characterController.Move(player_velocity * Time.deltaTime);
        }

        // Camera / player rotation
        if (canLook)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        
    }
}
