using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraSmoothMover : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float movementSpeed = 2f;       // Speed of the movement
    public float intervalY = 2f;           // Distance to move in the Y-axis per click
    public float intervalZ = 2f;           // Distance to move in the Z-axis per click

    [Header("Position Limits")]
    public float minY = 0f;                // Minimum Y position limit
    public float maxY = 20f;               // Maximum Y position limit
    public float minZ = -10f;              // Minimum Z position limit
    public float maxZ = 10f;               // Maximum Z position limit

    [Header("Smooth LookAt Settings")]
    public float lookAtSpeed = 2f;         // Speed of camera rotation toward target
    public float yOffset = 2f;             // Offset to apply to Y-axis when looking at the target
    public float lookCooldown = 0.5f;      // Cooldown in seconds for updating the target

    private Vector3 targetPosition;        // The position to smoothly move to
    private float lastUpdateTime = 0f;          // The position to smoothly move to
    private Quaternion targetRotation;

    private Vector3 initialPosition; // Camera's initial position
    private Quaternion initialRotation; // Camera's initial rotation
    private bool isToggled = false; // Tracks toggle state


    private void Start()
    {
        // Initialize the target position to the camera's current position
        targetPosition = transform.position;
        targetRotation = transform.rotation;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movementSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * movementSpeed);

        // SmoothCamTransaction();



    }


    public void CamSwitchPos(Transform CamPos)
    {
        if (!isToggled)
        {
            // Switch to the specified CamPos
            targetPosition = CamPos.position;
            targetRotation = CamPos.rotation;
        }
        else
        {
            // Switch back to the initial position and rotation
            targetPosition = initialPosition;
            targetRotation = initialRotation;
        }

        // Toggle the state
        isToggled = !isToggled;
    }


    void SmoothCamTransaction()
    {
        if (SheepManager.instance.FirstSheep != null && Time.time >= lastUpdateTime + lookCooldown)
        {
            Transform target = SheepManager.instance.FirstSheep.transform;

            // Add offset to the target's Y position
            Vector3 targetPositionWithOffset = target.position;
            targetPositionWithOffset.y += yOffset;

            // Calculate direction to the adjusted target
            Vector3 directionToTarget = targetPositionWithOffset - transform.position;

            // Smoothly rotate toward the target
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookAtSpeed);

            // Update cooldown timer
            lastUpdateTime = Time.time;
        }
    }
    // Public methods to be called by the UI buttons

    public void MoveUp()
    {
        targetPosition.y += intervalY;
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY); // Constrain Y position
    }

    public void MoveDown()
    {
        targetPosition.y -= intervalY;
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY); // Constrain Y position
    }

    public void MoveRight()
    {
        targetPosition.z -= intervalZ;
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ); // Constrain Z position
    }

    public void MoveLeft()
    {
        targetPosition.z += intervalZ;
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ); // Constrain Z position
    }
}
