using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private bool isLab = false;
    [SerializeField]
    private bool isOpen = false;
    [SerializeField]
    private bool isRunning = false;

    // Rotation for door animations
    public Quaternion openAngle = Quaternion.Euler(0, 90.0f, 0); // Completely open
    public Quaternion closeAngle = Quaternion.Euler(0, 0, 0); // Completely closed

    // Rotational speed of doors
    public float rotationSpeed = 5f;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        isOpen = false;
        if(isLab)
            openAngle = transform.rotation * Quaternion.Euler(0, 0, 90);
        else
            openAngle = transform.rotation * Quaternion.Euler(0, 90, 0);
        closeAngle = transform.rotation;
    }

    void Update()
    {
        if (!isRunning)
            return;

        // Rotation
        if (isOpen)
        {
            Quaternion targetRotation = openAngle;
            if(Quaternion.Angle(transform.rotation, targetRotation)< 1.0f)
                isRunning = false;
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            Quaternion targetRotation = closeAngle;
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1.0f)
                isRunning = false;
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void OnKeyboard()
    {
        if (Managers.Cursor.IsCursorVisible())
            return;

        if (Input.GetKey(KeyCode.E))
        {
            Collider collider = Managers.Raycast.HitCollider("Door");
            if (collider != null && collider.gameObject == transform.gameObject)
            {
                GameObject door = collider.gameObject;
                ToggleDoor(door);
            }
        }
    }

    void ToggleDoor(GameObject door)
    {
        if (isRunning)
            return;

        isOpen = !isOpen;

        if (isOpen)
            OpenDoor(door);
        else
            CloseDoor(door);
    }

    void OpenDoor(GameObject door)
    {
        isRunning = true;
    }

    void CloseDoor(GameObject door)
    {
        isRunning = true;
    }
}
