using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MovingPlatform : MonoBehaviour
{
    private enum WaveMovement { Horizontal, Vertical };

    private float xPos;
    private float yPos;
    private float zPos;
    
    [SerializeField] private float waveStrength;
    [SerializeField] private WaveMovement movementDir;

    private void Start()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        zPos = transform.position.z;
    }

    private void Update()
    {
        switch(movementDir)
        {
            case WaveMovement.Horizontal:
                transform.position = new Vector3(xPos + ((float)Mathf.Sin(Time.time) * waveStrength), yPos, zPos);
                break;
            case WaveMovement.Vertical:
                transform.position = new Vector3(xPos, yPos + ((float)Mathf.Sin(Time.time) * waveStrength), zPos);
                break;
            default:
                Debug.LogWarning("No defined direction of movement");
                break;
        }
    }
    
    private void ChangeWaveStrength(float _amount)
    {
        waveStrength -= _amount;
        CustomEvents.Scripts.OnDisableMovement?.Invoke(true);
        CustomEvents.Scripts.OnDisableCamera?.Invoke(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }

    private void OnEnable()
    {
        CustomEvents.Problems.Platforms.OnChangeWaveStrength += ChangeWaveStrength;
    }
    
    private void OnDisable()
    {
        CustomEvents.Problems.Platforms.OnChangeWaveStrength -= ChangeWaveStrength;
    }
}
