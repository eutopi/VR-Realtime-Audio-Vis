using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateMesh : MonoBehaviour
{
    // Start is called before the first frame update
    private bool triggerDownController = false;
    private bool triggerDownMouse = false;
    Vector3 PrevPosController = Vector3.zero;
    Vector3 PosDeltaController = Vector3.zero;
    Vector3 PrevPosMouse = Vector3.zero;
    Vector3 PosDeltaMouse = Vector3.zero;
    public InputActionReference dragReferenceController = null;
    public InputActionReference dragReferenceMouse = null;

    // Update is called once per frame
    void Update()
    {
        Vector2 rotationMouse = dragReferenceMouse.action.ReadValue<Vector2>();
        Vector3 currentPosMouse = new Vector3((float)rotationMouse.x/10, (float)rotationMouse.y/10, 0f);

        Quaternion rotationController = dragReferenceController.action.ReadValue<Quaternion>();
        Vector3 currentPosController = new Vector3(-rotationController.y*100, rotationController.x*100, 0f);

        if (triggerDownMouse)
        {
            Vector3 delta = currentPosMouse - PrevPosMouse;
            UpdateTransform(delta);
        }
        else if (triggerDownController)
        {
            Vector3 delta = currentPosController - PrevPosController;
            UpdateTransform(delta);
        }

        PrevPosMouse = currentPosMouse;
        PrevPosController = currentPosController;
    }

    private void UpdateTransform(Vector3 delta) 
    {
        if (Vector3.Dot(transform.up, Vector3.up) >= 0)
        {
            transform.Rotate(transform.up, -Vector3.Dot(delta, Camera.main.transform.right), Space.World);
        }
        else 
        {
            transform.Rotate(transform.up, Vector3.Dot(delta, Camera.main.transform.right), Space.World);
        }
        transform.Rotate(Camera.main.transform.right, Vector3.Dot(delta, Camera.main.transform.up),  Space.World);
        
    }

    public void ToggleTriggerMouse()  {
        triggerDownMouse = !triggerDownMouse;
    }

    public void ToggleTriggerController() {
        triggerDownController = !triggerDownController;
    }

}
