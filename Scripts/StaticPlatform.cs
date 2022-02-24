using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlatform : MonoBehaviour
{
    [SerializeField] private GameObject promptsUI;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CustomEvents.Problems.OnGetPrompt?.Invoke() == true)
            {
                promptsUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CustomEvents.Problems.OnGetPrompt?.Invoke() == true)
        {
            promptsUI.SetActive(false);
        }
    }
}
