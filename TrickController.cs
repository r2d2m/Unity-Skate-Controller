using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickController : MonoBehaviour
{
    public float flipSpeed = 500f; // Speed at which the board flips
    public float rotationSpeed = 720f; // Speed at which the board rotates
    public float spinSpeed = 1000f; // Speed at which the player spins
    public float trickHeight = 1f; // Height at which the board rotates during a flip trick
    public float trickDistance = 1f; // Distance at which the board moves forward during a flip trick

    private bool isFlipping = false;
    private bool isSpinning = false;
    private Rigidbody boardRb;
    private Animator animator;

    void Start()
    {
        boardRb = GetComponentInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for flip tricks
        if (Input.GetKeyDown(KeyCode.F) && !isFlipping)
        {
            StartCoroutine(FlipTrick());
        }

        // Check for spin tricks
        if (Input.GetKeyDown(KeyCode.R) && !isSpinning)
        {
            StartCoroutine(SpinTrick());
        }
    }

    IEnumerator FlipTrick()
    {
        isFlipping = true;

        // Rotate the board around the X-axis for a flip trick
        float flipTime = 0;
        while (flipTime < flipSpeed)
        {
            float flipAngle = Mathf.Lerp(0, 360, flipTime / flipSpeed);
            boardRb.transform.localRotation = Quaternion.Euler(-flipAngle, 0, 0);
            flipTime += Time.deltaTime;
            yield return null;
        }

        // Move the board forward and rotate it around the Z-axis for a kickflip
        boardRb.AddForce(transform.forward * trickDistance, ForceMode.Impulse);
        float kickflipTime = 0;
        while (kickflipTime < flipSpeed)
        {
            float kickflipAngle = Mathf.Lerp(0, 360, kickflipTime / flipSpeed);
            boardRb.transform.localRotation = Quaternion.Euler(-180, 0, kickflipAngle);
            kickflipTime += Time.deltaTime;
            yield return null;
        }

        // Reset the board's rotation
        boardRb.transform.localRotation = Quaternion.identity;

        isFlipping = false;
    }

    IEnumerator SpinTrick()
    {
        isSpinning = true;

        // Spin the player around the Y-axis for a spin trick
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
