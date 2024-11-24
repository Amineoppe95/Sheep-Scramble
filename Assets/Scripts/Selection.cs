using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{
    public Material highlightMaterial;    // Material for highlighting objects
    public Material selectionMaterial;    // Material for selected objects

    private Material originalMaterialHighlight;    // To store original highlight material
    private Material originalMaterialSelection;    // To store original selection material
    private Transform highlight;                    // Transform of the highlighted object
    private Transform selection;                    // Transform of the selected object
    private RaycastHit raycastHit;                  // Raycast hit data

    private Vector3 mouseStartPosition;            // Starting position of the mouse
    private bool isDragging = false;               // Flag to check if dragging is in progress
    public float forceMultiplier = 10f;            // Multiplier for the applied force

    private Rigidbody rb;                          // Rigidbody of the selected object

    void Update()
    {
        SelectionViusualLogic();
       
    }

    void SelectionViusualLogic()
    {
        // Highlight logic
        if (highlight != null)
        {
            highlight.GetComponent<MeshRenderer>().sharedMaterial = originalMaterialHighlight;
            highlight = null;  // Reset the highlighted object
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Raycasting from camera to 3D world

        // Ensure raycast is not over UI and is hitting a valid object
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            Transform hitObject = raycastHit.transform;

            // Only highlight objects with the "Ground" tag and having a Rigidbody
            if (hitObject.CompareTag("Ground") && hitObject.GetComponent<Rigidbody>() != null && hitObject != selection)
            {
                highlight = hitObject;

                // If the object is not already highlighted, change its material
                if (highlight.GetComponent<MeshRenderer>().material != highlightMaterial)
                {
                    originalMaterialHighlight = highlight.GetComponent<MeshRenderer>().material;
                    highlight.GetComponent<MeshRenderer>().material = highlightMaterial;
                }
            }
            else
            {
                highlight = null;  // Don't highlight if the object is invalid or already selected
            }
        }

        // Selection logic (when the user clicks)
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (highlight)
            {
                print("Highlighted");
                // If an object is already selected, revert its material
                if (selection != null)
                {
                    selection.GetComponent<MeshRenderer>().material = originalMaterialSelection;
                }

                // Select the new highlighted object
                selection = raycastHit.transform;

                if (selection.GetComponent<MeshRenderer>().material != selectionMaterial)
                {
                    originalMaterialSelection = originalMaterialHighlight;  // Store original material of highlighted object
                    selection.GetComponent<MeshRenderer>().material = selectionMaterial;  // Change material to selection
                }

                // Start drag if object has DirectionChanger script
                if (selection.parent.GetComponent<DirectionChanger>() != null)
                {
                    mouseStartPosition = Input.mousePosition;
                    isDragging = true;
                    rb = selection.GetComponent<Rigidbody>(); // Get the Rigidbody when dragging starts
                    print("DirectionChanger Selected isdragging true");
                }

                // Reset highlight once an object is selected
                highlight = null;
            }
            else
            {
                // Deselect the current selected object if clicked outside
                if (selection)
                {
                    selection.GetComponent<MeshRenderer>().material = originalMaterialSelection;
                    selection = null;  // Deselect object
                }
            }
        }
    }

     
}
