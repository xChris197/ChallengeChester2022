using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RespawnPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CustomEvents.Player.OnRespawnPlayer?.Invoke();
            switch (CustomEvents.Player.OnGetProblemType?.Invoke())
            {
                case ProblemType.None:
                    break;
                case ProblemType.Jumping:
                    CustomEvents.Problems.Jumping.OnAddJumpFail?.Invoke();
                    break;
                case ProblemType.Platforms:
                    CustomEvents.Problems.Platforms.OnAddPlatformFail?.Invoke();
                    break;
                default:
                    Debug.LogWarning("No defined problem type");
                    break;
            }
        }
    }
}
