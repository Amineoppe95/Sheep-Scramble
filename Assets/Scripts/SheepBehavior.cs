using UnityEngine;

public class SheepBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;           // Desired speed of the sheep
    public float speedStartCooldown = 3f; // Time to wait before setting the speed

    private Rigidbody rb;
    public Animator animator;             // Reference to the Animator
    private Vector3 currentDirection = Vector3.forward; // Default direction

    private float currentSpeed = 0f;      // Current speed of the sheep
    private float speedTimer = 0f;        // Timer to track the cooldown

    private bool isGrounded = true;       // State tracking for grounded status
    public float gravityScale = 1f;       // Gravity multiplier adjustable in the Inspector

    private BoxCollider boxCollider;      // Reference to the sheep's BoxCollider
    [SerializeField, Tooltip("Distance for the ground detection ray")]
    private float rayDistance;            // Adjustable ray distance

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        // Automatically calculate rayDistance if a BoxCollider is attached
        if (boxCollider != null)
        {
            rayDistance = boxCollider.bounds.extents.y + 0.1f; // Default value with a small buffer
        }
        else
        {
            Debug.LogWarning("BoxCollider not found on the sheep!");
        }

        currentSpeed = 0f;                // Start with 0 speed
        animator.SetFloat("Speed", 0f);   // Ensure animation starts as idle

        isGrounded = IsGroundAhead();     // Initialize grounded status
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

        bool grounded = IsGroundAhead();

        if (grounded)
        {
            rb.useGravity = true; // Enable Unity's default gravity
            animator.applyRootMotion = true;
            animator.SetBool("Grounded", true);
            print("Grounded!");
            MoveForward();
        }
        else
        {
            rb.useGravity = false; // Disable default gravity
            rb.AddForce(Vector3.down * gravityScale * Physics.gravity.y, ForceMode.Acceleration); // Apply custom gravity
            animator.applyRootMotion = false;
            animator.SetBool("Grounded", false);
            print("Not on ground");
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
        // Calculate the ray origin based on the current direction and collider center
        Vector3 rayOrigin = boxCollider.bounds.center + currentDirection * boxCollider.bounds.extents.z * 0.5f;

        Ray ray = new Ray(rayOrigin, Vector3.down);

        Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.red); // Visualize the ray

        // Check if the ray hits an object tagged as "Ground"
        return Physics.Raycast(ray, out RaycastHit hit, rayDistance) && hit.collider.CompareTag("Ground");
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

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Vector3 rayOrigin = boxCollider.bounds.center + currentDirection * boxCollider.bounds.extents.z * 0.5f;
            Gizmos.color = Color.red; // Use green for the gizmo
            Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * rayDistance);
            Gizmos.DrawSphere(rayOrigin, 0.05f);
        }
    }
}
