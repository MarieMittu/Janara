using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToys : MonoBehaviour
{
    public GameObject[] draggableObjects; // Array to hold multiple draggable objects
    private GameObject selectedObject;
    public Camera dragCamera;
    private Vector3 screenPoint;
    private Vector3 offset;
    private float fixedY;

    private bool toysAligned = false;
    private HashSet<GameObject> objectsInTrigger = new HashSet<GameObject>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            OnMouseDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;  // Release the object after the mouse is released
        }
    }

    void OnMouseDown()
    {
        Ray ray = dragCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object hit is in the draggable objects list
            foreach (GameObject obj in draggableObjects)
            {
                if (hit.transform.gameObject == obj)
                {
                    selectedObject = obj;  // Set the selected object for dragging
                    screenPoint = dragCamera.WorldToScreenPoint(selectedObject.transform.position);
                    offset = selectedObject.transform.position - dragCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

                    fixedY = selectedObject.transform.position.y;

                    break;
                }
            }
        }
    }

    void OnMouseDrag()
    {
        if (selectedObject == null) return;  // Ensure that an object is selected

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = dragCamera.ScreenToWorldPoint(curScreenPoint) + offset;
        curPosition.y = fixedY;
        selectedObject.transform.position = curPosition;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LINE"))
        {
            // Log the object currently inside the trigger
            Debug.Log("LINE Currently inside trigger: " + other.gameObject.name);

            // Add the object to the HashSet if it's one of the draggable objects
            if (System.Array.Exists(draggableObjects, obj => obj == selectedObject))
            {
                // Add the selected object to the HashSet
                objectsInTrigger.Add(selectedObject);
                Debug.Log("LINE Added to trigger: " + selectedObject.name);

                // Check if all draggable objects are inside the trigger
                if (objectsInTrigger.Count == draggableObjects.Length)
                {
                    // All draggable objects are in the trigger
                    Debug.Log("LINE All draggable objects are inside the trigger!");
                    toysAligned = true; // Set aligned flag
                }
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("LINE"))
    //    {
    //        // Log the object currently inside the trigger
    //        Debug.Log("LINE Currently inside trigger: " + other.gameObject.name);

    //        // Add the object to the HashSet if it's one of the draggable objects
    //        if (System.Array.Exists(draggableObjects, obj => obj == selectedObject))
    //        {
    //            // Add the selected object to the HashSet
    //            objectsInTrigger.Add(selectedObject);
    //            Debug.Log("LINE Added to trigger: " + selectedObject.name);

    //            // Check if all draggable objects are inside the trigger
    //            if (objectsInTrigger.Count == draggableObjects.Length)
    //            {
    //                // All draggable objects are in the trigger
    //                Debug.Log("LINE All draggable objects are inside the trigger!");
    //                toysAligned = true; // Set aligned flag
    //            }
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        // If an object leaves the trigger, check if it is one of the draggable objects
        if (other.CompareTag("LINE"))
        {
            // Log the object that has exited the trigger
            Debug.Log($"{other.gameObject.name} has exited the trigger.");

            // Remove the selected object from the HashSet if it exists
            if (objectsInTrigger.Remove(selectedObject))
            {
                Debug.Log($"Removed {selectedObject.name} from the trigger.");
            }

            // Check if not all draggable objects are in the trigger
            if (objectsInTrigger.Count < draggableObjects.Length)
            {
                Debug.Log("Not all draggable objects are inside the trigger anymore.");
                toysAligned = false; // Reset aligned flag
            }
        }
    }
}
