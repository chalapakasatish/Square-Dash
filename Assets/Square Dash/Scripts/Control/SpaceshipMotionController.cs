using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMotionController : MotionController
{
    [Header("Data")]
    private SpaceshipMotionData data;
    [Header("Settings")]
    private bool isFlying;

    public SpaceshipMotionController(GameObject player, SpaceshipMotionData data)
    {
        Awake(player);
        this.data = data;
    }
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Fly();
        }
    }

    public override void FixedUpdate()
    {
        Vector2 velocity = rig.velocity;

        velocity.x = data.MoveSpeed;
        if (isFlying)
        {
            velocity.y += data.SpaceshipAcceleration;
            isFlying = false;
        }
        else
        {
            velocity.y -= data.SpaceshipAcceleration;
        }

        rig.velocity = velocity;
    }
    public void Fly()
    {
        isFlying = true;
    }
}
