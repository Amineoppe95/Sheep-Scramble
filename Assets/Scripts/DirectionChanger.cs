using UnityEngine;
using UnityEngine.Events;

public class DirectionChanger : MonoBehaviour
{
    public enum Direction
    {
        Left = 1 ,
        Right = 2 ,
        Backward = 3,
        Forward = 4
    }
    public Transform cubeTransform;
    [Tooltip("The new direction the sheep should move in when it enters this cube.")]
    public Direction newDirection = Direction.Right;
    // Event that will be triggered when direction changes
    public UnityEvent<int> onDirectionValueChanged;
    public Transform Arrow;
    // Private backing field for DirValue
    public int dirValue = 0;
    public int DirValue
        {
        get => dirValue;
        set
        {
            // Only trigger if the value actually changes
            if (dirValue != value)
            {
                dirValue = value;
                OnDirValueChanged();
}
        }
    }

private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is a sheep
        SheepBehavior sheep = other.GetComponent<SheepBehavior>();
        if (sheep != null)
        {
            // Map the enum to the corresponding Vector3 direction
            Vector3 directionVector = GetDirectionVector(newDirection);
            // Change the sheep's direction
            DirValue = (int)newDirection;
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

    // Method that gets called when DirValue changes
    private void OnDirValueChanged()
    {
        // Invoke the UnityEvent with the new value
        onDirectionValueChanged.Invoke(dirValue);
        print("CHANGED");
        switch (dirValue)
        {
            case 1:
                Arrow.rotation = Quaternion.Euler(90f, 0f, 90f); 
                break;  // Add break statement
            case 2:
                Arrow.rotation = Quaternion.Euler(90f, 0f, -90f);
                break;  // Add break statement
            case 3:
                Arrow.rotation = Quaternion.Euler(90f, 0f, 180f);
                break;  // Add break statement
            case 4:
                Arrow.rotation = Quaternion.Euler(90f, 0f, 0f);
                break;  // Add break statement
        }
        // You can add additional logic here that should happen when the direction changes
        Debug.Log($"Direction changed to: {(Direction)dirValue}");
    }
}
