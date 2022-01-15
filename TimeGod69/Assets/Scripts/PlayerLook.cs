using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float sensX = 0.5f;
    [SerializeField] private float sensY = 0.5f;

    [SerializeField] Transform cam;
    [SerializeField] Transform orientaion;

    float mouseX;
    float mouseY;

    float multiplier = 5f;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MyInput();
        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientaion.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
