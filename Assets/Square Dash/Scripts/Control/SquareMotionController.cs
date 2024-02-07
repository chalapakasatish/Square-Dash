using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SquareMotionController : MotionController
{
    [Header("Data")]
    private SquareMotionData data;
    [Header("Settings")]
    private bool isJumping;
    private bool isGrounded;

    public SquareMotionController(GameObject player, SquareMotionData data)
    {
        Awake(player);
        this.data = data;   
    }
    public override void Update(bool isGrounded)
    {
        this.isGrounded = isGrounded;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Jump();
        }
    }

    public override void FixedUpdate()
    {
        Vector2 velocity = rig.velocity;

        velocity.x = data.MoveSpeed;
        if (isJumping)
        {
            velocity.y = data.JumpSpeed;
            isJumping = false;
        }

        rig.velocity = velocity;
    }
    public void Jump()
    {
        if (isGrounded)
        {
            isJumping = true;
        }
    }
}
