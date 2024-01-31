using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rig;

    [Header("Elements")]
    public Transform groundDetector;
    public LayerMask groundMask;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float groundDetectionRadius;
    private bool isJumping;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        Vector2 velocity = rig.velocity;

        velocity.x = moveSpeed;
        if(isJumping)
        {
            velocity.y = jumpSpeed;
            isJumping = false;
        }
        
        rig.velocity = velocity;
    }
    public void Jump()
    {
        if(IsGrounded())
        {
            isJumping = true;
        }
    }
    public bool IsGrounded()
    {
        Collider2D ground = Physics2D.OverlapCircle(groundDetector.position, groundDetectionRadius, groundMask);
        return ground != null;
    }
    public bool IsPressing() => Input.GetMouseButton(0);
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundDetector.position, groundDetectionRadius);
    }
}
