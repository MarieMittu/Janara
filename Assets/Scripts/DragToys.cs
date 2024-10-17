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
}
