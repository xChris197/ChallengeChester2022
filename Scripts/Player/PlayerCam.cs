using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float lookSensitivity;

    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    private bool bIsActive = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (bIsActive)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * lookSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * lookSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    private void DisableCam(bool state)
    {
        bIsActive = state;
    }

    private void OnEnable()
    {
        CustomEvents.Scripts.OnDisableCamera += DisableCam;
    }
    
    private void OnDisable()
    {
        CustomEvents.Scripts.OnDisableCamera -= DisableCam;
    }
}
