using UnityEngine;

public class DirectionChanger : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Backward,
        Forward
    }
    public Transform cubeTransform;
    [Tooltip("The new direction the sheep should move in when it enters this cube.")]
    public Direction newDirection = Direction.Right;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is a sheep
        SheepBehavior sheep = other.GetComponent<SheepBehavior>();
        if (sheep != null)
        {
            // Map the enum to the corresponding Vector3 direction
            Vector3 directionVector = GetDirectionVector(newDirection);
            // Change the sheep's direction
            sheep.transform.position = cubeTransform.position;            
            sheep.ChangeDirection(directionVector);
            
        }
    }
    

    private Vector3 GetDirectionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            case Direction.Backward:
                return Vector3.forward;
            case Direction.Forward:
                return Vector3.back;
            default:
                return Vector3.zero; // Default to no movement if direction is invalid
        }
    }
}
