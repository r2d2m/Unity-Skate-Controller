using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Rigidbody component for applying physics to the player
    private Rigidbody rb;

    // Movement variables
    public float speed;
    public float jumpForce;
    public float maxJumpTime;
    private float jumpTime;
    private bool isGrounded;

    // Skate trick variables
    public float flipSpeed;
    public float spinSpeed;
    private bool isFlipping;
    private bool isSpinning;

    // Grinding variables
    public float grindSpeed;
    private bool isGrinding;
    private GameObject currentRail;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component on the player
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // Move the player forward and backward
        float moveForward = Input.GetAxis("Vertical") * speed;
        float moveSideways = Input.GetAxis("Horizontal") * speed;
        Vector3 movement = new Vector3(moveSideways, 0, moveForward);
        movement = transform.rotation * movement;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Jump mechanic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpTime = maxJumpTime;
        }
        if (Input.GetKey(KeyCode.Space) && jumpTime > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpTime -= Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTime = 0;
        }

        // Skate trick mechanics
        if (Input.GetKeyDown(KeyCode.F) && !isFlipping)
        {
            StartCoroutine(Flip());
        }
        if (Input.GetKeyDown(KeyCode.G) && !isSpinning)
        {
            StartCoroutine(Spin());
        }

        // Grinding mechanic
        if (Input.GetKeyDown(KeyCode.E) && isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2))
            {
                if (hit.collider.CompareTag("Rail"))
                {
                    currentRail = hit.collider.gameObject;
                    isGrinding = true;
                }
            }
        }
        if (isGrinding)
        {
            rb.velocity = new Vector3(currentRail.transform.forward.x * grindSpeed, rb.velocity.y, currentRail.transform.forward.z * grindSpeed);
        }
    }

    // Coroutine for performing a kick flip
    IEnumerator Flip()
    {
        isFlipping = true;
        float flipTime = 0;
        while (flipTime < flipSpeed)
        {
            transform.Rotate(Vector3.right * 360 / flipSpeed * Time.deltaTime);
            flipTime += Time.deltaTime;
            yield return null;
        }
        isFlipping = false;
    }

    // Coroutine for performing a tre flip
    IEnumerator Spin()
    {
        isSpinning = true;
        float spinTime = 0;
        while (spinTime < spinSpeed)
        {
            transform.Rotate(Vector3.up * 360 / spinSpeed * Time.deltaTime);
            spinTime += Time.deltaTime;
            yield return null;
        }
        isSpinning = false;
    }
}
