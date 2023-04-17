using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f; // Movement speed of the player
    [SerializeField] private float jumpForce = 10f; // Force applied when jumping
    [SerializeField] private float gravity = -9.81f; // Gravity force
    [SerializeField] private float groundDistance = 0.4f; // Distance to the ground for detecting if the player is grounded
    [SerializeField] private LayerMask groundMask; // Layer mask for the ground objects

    private CharacterController controller; // Reference to the Character Controller component
    private Vector3 playerVelocity; // The velocity of the player
    private bool isGrounded; // Flag for checking if the player is grounded

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        // Reset the player's velocity if they are grounded
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        // Get the player's input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Move the player based on their input
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity to the player's velocity
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Check if the player is jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
}

// This script uses Unity's built-in CharacterController component to handle the movement and physics of the player. It applies gravity to the player's velocity and allows the player to jump. You can adjust the values of the moveSpeed, jumpForce, gravity, groundDistance, and groundMask variables to fit your game's needs.
