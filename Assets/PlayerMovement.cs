using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    [Header("Jumping Settings")]
    public float jumpHeight = 2f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    private Vector3 velocity;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;


    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();

        if (jumpAction != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += Jump;
        }
    }

    private void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();

        if (jumpAction != null)
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= Jump;
        }
    }

    private void Update()
    {
        Move();
        ApplyGravity();
        //character controller isGrounded
        //Debug.Log("CharacterController isGrounded: " + controller.isGrounded);

    }

    private void Move()
    {
        if (moveAction == null) return;
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            Debug.Log("Jump Action Triggered!");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            print(velocity.y);
        }
    }

    private void ApplyGravity()
    {
        if (groundCheck == null) return;
        //print(groundCheck.position);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small force to keep grounded properly
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
