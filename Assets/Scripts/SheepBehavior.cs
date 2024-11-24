using UnityEngine;

public class SheepBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;        // Speed at which the sheep moves forward
    public float rayDistance =  1f;     // Distance for edge detection using a downward ray

    private Rigidbody rb;
    private Vector3 currentDirection = Vector3.forward; // Default direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveForward();

        if (!IsGroundAhead()) 
        {
            rb.useGravity = true;
           
        }
    }

    void MoveForward()
    {
        // Move the sheep in the current direction
        transform.Translate(currentDirection * moveSpeed * Time.fixedDeltaTime);
    }

    bool IsGroundAhead()
    {
        Vector3 rayOrigin = transform.position + currentDirection * 0.5f;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        return Physics.Raycast(ray, rayDistance);
    }

    public void ChangeDirection(Vector3 newDirection)
    {
        // Update the sheep's direction
       // currentDirection = newDirection.normalized;
        transform.rotation = Quaternion.Euler(0, transform.rotation.y +  -90f , 0 );


    }

    private void OnDrawGizmosSelected()
    {
        Vector3 rayOrigin = transform.position + -currentDirection * 0.5f;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * rayDistance);
    }
}