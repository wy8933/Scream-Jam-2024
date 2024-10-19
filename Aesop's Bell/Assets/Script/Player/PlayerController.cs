using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed;
    public float mouseSensitivity;
    public float jumpForce;
    public float wallClimbSpeed;
    private bool isClimbing = false;

    private CharacterController _characterController;
    private Rigidbody _playerRB;
    private PlayerInput _playerInput;
    private PlayerInputAction _playerInputAction;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();

        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Player.Enable();
        _playerInputAction.Player.Jump.performed += Jump;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FixedUpdate()
    {
        HandleMouseLook();

        if (isClimbing)
        {
            ClimbWall();
        }
        else {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        Vector2 playerInput = _playerInputAction.Player.Movement.ReadValue<Vector2>();
        _playerRB.AddForce(transform.TransformDirection(playerInput.x, 0, playerInput.y) * speed, ForceMode.Force);
    }

    void HandleMouseLook()
    {
        Vector2 lookInput = _playerInputAction.Player.Look.ReadValue<Vector2>();

        // Get mouse movement input
        float _mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float _mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Rotate the player on the Y-axis (horizontal rotation)
        transform.Rotate(Vector3.up * _mouseX);
    }

    public void Jump(InputAction.CallbackContext context) {
        if (!isClimbing)
        {
            _playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    private void ClimbWall()
    {
        Vector2 climbInput = _playerInputAction.Player.Movement.ReadValue<Vector2>();
        Vector3 climbVelocity = (transform.up * climbInput.y + transform.right * climbInput.x) * wallClimbSpeed;

        // Move the player along the wall
        _playerRB.velocity = new Vector3(climbVelocity.x, climbVelocity.y, _playerRB.velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided surface is climbable
        if (collision.gameObject.CompareTag("Climbable"))
        {
            isClimbing = true;
            _playerRB.useGravity = false;  // Disable gravity while climbing
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Stop climbing when leaving the wall
        if (collision.gameObject.CompareTag("Climbable"))
        {
            isClimbing = false;
            _playerRB.useGravity = true;  // Re-enable gravity
        }
    }

}
