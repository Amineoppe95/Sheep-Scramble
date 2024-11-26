using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DirectionChanger : MonoBehaviour
{
    [Tooltip("The new direction the sheep should move in when it enters this cube.")]
    public enum Direction
    {
        Left = 1 ,
        Right = 2 ,
        Backward = 3,
        Forward = 4
    }
    public Transform cubeTransform;

    public Direction newDirection = Direction.Right;
    // Event that will be triggered when direction changes
  
    public Transform ArrowDir;
    public float ArrowParentYrot = 0; 
    public int dirValue = 0;


    private void Start()
    {
        OnArrowChangeDir(1);
        OnArrowChangeDir(2);
        OnArrowChangeDir(3);
        OnArrowChangeDir(4);
    }

    #region DictionarydirectionRotations

    // Dictionary to map Direction enums to rotation values
    private readonly Dictionary<Direction, float> directionRotations = new Dictionary<Direction, float>
    {
        { Direction.Left, 180f },
        { Direction.Right, 0f },
        { Direction.Backward, 90f },
        { Direction.Forward, -90f }
    };

    public void OnArrowChangeDir(int dirValue)
    {
        // Check if the input integer corresponds to a valid Direction enum
        if (System.Enum.IsDefined(typeof(Direction), dirValue))
        {
            // Convert the integer to the corresponding Direction enum
            Direction selectedDirection = (Direction)dirValue;
            

            // Use the dictionary to retrieve the rotation value
            if (directionRotations.TryGetValue(selectedDirection, out float yRotation))
            {
                ArrowParentYrot = yRotation;
                ArrowDir.rotation = Quaternion.Euler(0, yRotation, 0); 
            }
            else
            {
                Debug.LogWarning("No rotation defined for this direction.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid direction value: " + dirValue);
        }
    }


    #endregion



    private void Update()
    {
        ArrowDir.rotation = Quaternion.Euler(0, ArrowParentYrot, 0);
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
            dirValue = (int) newDirection;
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
