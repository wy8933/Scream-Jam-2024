using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float initialSpeed = 2f;         // Initial movement speed
    public float maxSpeed = 5f;              // Maximum horizontal speed
    public float mouseSensitivity = 100f;
    public float jumpForce = 5f;             // Initial jump force
    public float wallClimbSpeed = 3f;        // Initial climb speed
    private bool _isMoving;

    [Header("Scaling Limits")]
    public float minSpeed = 1f;              // Minimum movement speed after collecting all puzzle pieces
    public float minJumpForce = 1f;          // Minimum jump force
    public float minClimbSpeed = 1f;         // Minimum climb speed
    public int totalPuzzlePieces = 5;        // Total puzzle pieces

    private bool isClimbing = false;
    private bool isGrounded = false;
    private Rigidbody _playerRB;
    private PlayerInputAction _playerInputAction;

    private GameObject nearbyItem = null;
    private int puzzlePiecesCollected = 0;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
        _playerRB.constraints = RigidbodyConstraints.FreezeRotation;
        _playerRB.drag = 0;  // Ensure no drag that could block movement
        _playerRB.useGravity = true;

        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Player.Enable();
        _playerInputAction.Player.Jump.performed += Jump;
        _playerInputAction.Player.Interact.performed += InteractItem;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        DialogueSystem.instance.ShowDialogue("I'm sorry, Eeka. I couldn't do it for you. *cough* It has the bell.");
    }

    private void FixedUpdate()
    {
        HandleMouseLook();
        CheckGroundStatus();

        if (isClimbing)
        {
            ClimbWall();
        }
        else
        {
            HandleMovement();
            LimitMaxSpeed();
        }

        HandleWalkingSound();
    }

    private void HandleMovement()
    {
        Vector2 playerInput = _playerInputAction.Player.Movement.ReadValue<Vector2>();
        Vector3 moveDirection = transform.TransformDirection(playerInput.x, 0, playerInput.y);

        // Ensure there is input before applying force
        if (moveDirection != Vector3.zero)
        {
            float scaledSpeed = Mathf.Lerp(initialSpeed, minSpeed, (float)puzzlePiecesCollected / totalPuzzlePieces);
            _playerRB.AddForce(moveDirection * scaledSpeed, ForceMode.VelocityChange);
        }

        _isMoving = playerInput.magnitude > 0.1f;
    }

    private void HandleWalkingSound()
    {
        if (_isMoving)
        {
            // Start looping the walking sound if not already playing
            if (!SoundManager.instance.effectsSource.isPlaying)
            {
                SoundManager.instance.effectsSource.clip = SoundManager.instance.walkingSFX;
                SoundManager.instance.effectsSource.loop = true;
                SoundManager.instance.effectsSource.Play();
            }
        }
        else
        {
            // Stop the walking sound if the player stops moving
            if (SoundManager.instance.effectsSource.isPlaying &&
                SoundManager.instance.effectsSource.clip == SoundManager.instance.walkingSFX)
            {
                SoundManager.instance.effectsSource.Stop();
            }
        }
    }

    private void LimitMaxSpeed()
    {
        Vector3 horizontalVelocity = new Vector3(_playerRB.velocity.x, 0, _playerRB.velocity.z);

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            _playerRB.velocity = new Vector3(horizontalVelocity.x, _playerRB.velocity.y, horizontalVelocity.z);
        }
    }

    private void HandleMouseLook()
    {
        Vector2 lookInput = _playerInputAction.Player.Look.ReadValue<Vector2>();

        float _mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * _mouseX);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && !isClimbing)
        {
            float scaledJumpForce = Mathf.Lerp(jumpForce, minJumpForce, (float)puzzlePiecesCollected / totalPuzzlePieces);
            _playerRB.AddForce(Vector3.up * scaledJumpForce, ForceMode.Impulse);
        }
    }

    private void ClimbWall()
    {
        Vector2 climbInput = _playerInputAction.Player.Movement.ReadValue<Vector2>();
        float scaledClimbSpeed = Mathf.Lerp(wallClimbSpeed, minClimbSpeed, (float)puzzlePiecesCollected / totalPuzzlePieces);

        Vector3 climbVelocity = (transform.up * climbInput.y + transform.right * climbInput.x) * scaledClimbSpeed;
        _playerRB.velocity = new Vector3(climbVelocity.x, climbVelocity.y, 0);
    }

    private void InteractItem(InputAction.CallbackContext context)
    {
        if (nearbyItem != null)
        {
            if (nearbyItem.CompareTag("Pickup"))
            {
                CollectPuzzlePiece();
                Destroy(nearbyItem);  // Destroy the item after picking it up
            }
            else
            {
                ShowInteractionDialogue(nearbyItem.name);  // Handle interaction
            }

            nearbyItem = null;

            // Clear interaction text after interaction
            DialogueSystem.instance.ClearInteractionText();
        }
    }

    private void ShowInteractionDialogue(string itemName)
    {
        switch (itemName)
        {
            case "Louey's Body":
                DialogueSystem.instance.ShowDialogue("Louey... no... *sniffle* I'll try my best.");
                break;

            case "Cheese Wedge":
                DialogueSystem.instance.ShowDialogue("Maybe I can bring that back with me... if I survive--stop. I will bring that back to celebrate.");
                break;

            case "Mouse Hole":
                DialogueSystem.instance.ShowDialogue("I'll need to rush here after I've belled the cat.");
                break;

            case "Cat Bed":
                DialogueSystem.instance.ShowDialogue("Aesop's lair. Where is he?");
                break;

            case "Cheese Crumbs":
                DialogueSystem.instance.ShowDialogue("When did we come get this cheese?");
                break;

            case "A Toy Mouse":
                DialogueSystem.instance.ShowDialogue("Taxidermists never get our bodies right.");
                break;

            case "Catnip Baggy":
                DialogueSystem.instance.ShowDialogue("Ew, smells like a skunk.");
                break;

            case "Mouse Trap":
                DialogueSystem.instance.ShowDialogue("I'm too young for the guillotine.");
                break;

            case "Rat Poison":
                DialogueSystem.instance.ShowDialogue("I've seen this before... in the council-mouse's nest.");
                break;

            case "Glue Trap w/mouse bones":
                DialogueSystem.instance.ShowDialogue("I miss you, Mom.");
                break;

            case "Cat toy Bell with feathers":
                DialogueSystem.instance.ShowDialogue("Obtaining the Bell Content Line.");
                break;

            case "A Dead Mouse":
                DialogueSystem.instance.ShowDialogue("I'm sorry... I should have never asked you to come with me.");
                break;

            case "Cage with bell":
                DialogueSystem.instance.ShowDialogue("I need the puzzle pieces. I can't unlock this yet.");
                break;

            case "Record Player":
                DialogueSystem.instance.ShowDialogue("How is this still playing? Louey chewed through its wires long ago.");
                break;

            default:
                DialogueSystem.instance.ShowDialogue("You found something interesting.");
                break;
        }
    }

    private void CollectPuzzlePiece()
    {
        puzzlePiecesCollected++;
        SoundManager.instance.PlayEffect(SoundManager.instance.keyPickupSFX);
        switch (puzzlePiecesCollected)
        {
            case 1:
                DialogueSystem.instance.ShowDialogue("I need more of these if I want the bell back.");
                break;

            case 2:
                DialogueSystem.instance.ShowDialogue("Another one found.");
                break;

            case 3:
                DialogueSystem.instance.ShowDialogue("The third one, check. Where could the last two be?");
                break;

            case 4:
                DialogueSystem.instance.ShowDialogue("That makes four. I just need one more.");
                break;

            case 5:
                DialogueSystem.instance.ShowDialogue("Okay, I have all the pieces to get the bell back.");
                StartCoroutine(ShowGameOverAfterDelay());
                break;
        }
    }
    private IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOver_Good");
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.CompareTag("Climbable"))
        {
            isClimbing = true;
            _playerRB.useGravity = false;
            _playerRB.velocity = Vector3.zero;
        }

        if (collidedObject.CompareTag("Pickup") || collidedObject.CompareTag("Interactable"))
        {
            nearbyItem = collidedObject;

            // Show interaction text using the Dialogue System
            string actionText = collidedObject.CompareTag("Pickup") ? "Press E to Pick Up" : "Press E to Interact";
            DialogueSystem.instance.ShowInteractionText(actionText);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            isClimbing = false;
            _playerRB.useGravity = true;
        }

        if (collision.gameObject == nearbyItem)
        {
            nearbyItem = null;
            DialogueSystem.instance.ClearInteractionText();
        }
    }
}
