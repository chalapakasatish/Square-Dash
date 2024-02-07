using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MotionController
{
    [Header("Components")]
    protected Rigidbody2D rig;

    protected void Awake(GameObject player)
    {
        rig = player.GetComponent<Rigidbody2D>();
    }
    
    public void SetGravityScale(float gravityScale)
    {
        rig.gravityScale = gravityScale;
    }
    public void Explode()
    {
        rig.isKinematic = true;
        rig.velocity = Vector2.zero;
    }
    public void Revive()
    {
        rig.isKinematic = false;
    }
    public float GetYVelocity() => rig.velocity.y;
    public abstract void FixedUpdate();
    public virtual void Update(bool isGrounded)
    {
        Update();
    }
    public virtual void Update()
    {

    }
}
