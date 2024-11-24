using UnityEngine;

public class SheepBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;         // Desired speed of the sheep
    public float rayDistance = 1f;      // Distance for edge detection using a downward ray
    public float speedStartCooldown = 3f; // Time to wait before setting the speed

    private Rigidbody rb;
    public Animator animator;           // Reference to the Animator
    private Vector3 currentDirection = Vector3.forward; // Default direction

    private float currentSpeed = 0f;    // Current speed of the sheep
    private float speedTimer = 0f;      // Timer to track the cooldown

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = 0f;              // Start with 0 speed
        animator.SetFloat("Speed", 0f); // Ensure animation starts as idle
    }

    void FixedUpdate()
    {
        // Gradually increase speed after cooldown
        if (speedTimer < speedStartCooldown)
        {
            speedTimer += Time.fixedDeltaTime;
        }
        else
        {
            currentSpeed = moveSpeed; // Set the speed to the desired value after cooldown
        }

        MoveForward();

        if (!IsGroundAhead())
        {
            rb.useGravity = true;
            animator.SetFloat("Speed", 0f); // Stop animation if no ground ahead
        }
    }

    void MoveForward()
    {
        // Move the sheep in the forward direction with the current speed
        transform.Translate(transform.forward * currentSpeed * Time.fixedDeltaTime, Space.World);

        // Update the animator's Speed parameter
        animator.SetFloat("Speed", currentSpeed);
    }

    bool IsGroundAhead()
    {
        Vector3 rayOrigin = transform.position + currentDirection * 0.5f;
        Ray ray = new Ray(rayOrigin, Vector3.down);

        Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.red); // Debug ray for visualization

        return Physics.Raycast(ray, rayDistance);
    }

    public void ChangeDirection(Vector3 newDirection)
    {
        // Update the sheep's direction
        float targetAngle = 0f;

        if (newDirection == Vector3.right)        // Right
            targetAngle = -90f;
        else if (newDirection == Vector3.left)   // Left
            targetAngle = 90f;
        else if (newDirection == Vector3.forward) // Forward
            targetAngle = 0f;
        else if (newDirection == Vector3.back)   // Backward
            targetAngle = 180f;

        currentDirection = newDirection.normalized;

        // Rotate the sheep to face the new direction
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = targetRotation;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 rayOrigin = transform.position + currentDirection * 0.5f;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * rayDistance);
    }
}
