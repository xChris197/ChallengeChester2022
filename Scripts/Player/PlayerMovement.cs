using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProblemType{ None, Jumping, Platforms };

public class PlayerMovement : MonoBehaviour
{

    private ProblemType problemType;
    
    private CharacterController controller;
    [SerializeField] private float movementSpeed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    private Vector3 velocity;

    [SerializeField] private Transform groundChecker;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool bIsGrounded;

    [SerializeField] private Vector3 respawnOffset;
    private Vector3 checkpointPos;

    private bool bCanDoubleJump = false;
    private bool bIsActive = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        bIsGrounded = Physics.CheckSphere(groundChecker.position, checkRadius, groundLayer);
        if (bIsGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        if (bIsActive)
        {
            float xMovement = Input.GetAxisRaw("Horizontal");
            float zMovement = Input.GetAxisRaw("Vertical");

            Vector3 movement = transform.right * xMovement + transform.forward * zMovement;
            controller.Move(movement * movementSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space) && bIsGrounded)
            {
                Jump();
                bCanDoubleJump = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !bIsGrounded)
            {
                if (bCanDoubleJump)
                {
                    Jump();
                    bCanDoubleJump = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CustomEvents.Options.OnToggleQuitMenu?.Invoke(true);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    }

    private void SetCheckpoint(Vector3 newPos)
    {
        checkpointPos = newPos;
    }

    private void RespawnPlayer()
    {
        transform.position = checkpointPos + respawnOffset;
    }

    private void DisableMovement(bool state)
    {
        bIsActive = state;
    }

    private Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    private ProblemType GetProblemType()
    {
        return problemType;
    }

    private void SetProblemType(ProblemType type)
    {
        problemType = type;
    }

    private void OnEnable()
    {
        CustomEvents.Player.OnSetCheckpoint += SetCheckpoint;
        CustomEvents.Player.OnRespawnPlayer += RespawnPlayer;
        CustomEvents.Scripts.OnDisableMovement += DisableMovement;
        CustomEvents.Player.OnGetPlayerPos += GetPlayerPosition;
        CustomEvents.Player.OnGetProblemType += GetProblemType;
        CustomEvents.Player.OnSetProblemType += SetProblemType;
    }
    
    private void OnDisable()
    {
        CustomEvents.Player.OnSetCheckpoint -= SetCheckpoint;
        CustomEvents.Player.OnRespawnPlayer -= RespawnPlayer;
        CustomEvents.Scripts.OnDisableMovement -= DisableMovement;
        CustomEvents.Player.OnGetPlayerPos -= GetPlayerPosition;
        CustomEvents.Player.OnGetProblemType -= GetProblemType;
        CustomEvents.Player.OnSetProblemType -= SetProblemType;
    }
}
