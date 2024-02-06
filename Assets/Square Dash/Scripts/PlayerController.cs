using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    enum State { Alive, Dead }
    private State state;

    enum MotionType { Square,Spaceship}
    private MotionType motionType;
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

    [Header("Spaceship Settings")]
    [SerializeField] private float spaceshipAcceleration;

    [Header("Actions")]
    public Action OnExploded;
    public Action OnRevived;
    public static Action onSpaceshipModeStarted;
    public static Action onSquareModeStarted;

    [Header("Effects")]
    [SerializeField] private ParticleSystem explodeParticles;
    private void Awake()
    {
        state = State.Alive;
        motionType = MotionType.Square;
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsDead())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        if (IsDead())
        {
            return;
        }
        switch (motionType)
        {
            case MotionType.Square:
                SquareFixedUpdate();
                break;
            case MotionType.Spaceship:
                SpaceshipFixedUpdate();
                break;
            default:
                break;
        }
    }
    public void SquareFixedUpdate()
    {
        Vector2 velocity = rig.velocity;

        velocity.x = moveSpeed;
        if (isJumping)
        {
            velocity.y = jumpSpeed;
            isJumping = false;
        }

        rig.velocity = velocity;
    }
    public void SpaceshipFixedUpdate()
    {
        Vector2 velocity = rig.velocity;

        velocity.x = moveSpeed;
        if (IsPressing())
        {
            velocity.y += spaceshipAcceleration;
        }
        else
        {
            velocity.y -= spaceshipAcceleration;
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
    public void Explode()
    {
        state = State.Dead;
        rig.isKinematic = true;
        rig.velocity = Vector2.zero;
        explodeParticles.Play();
        LeanTween.delayedCall(2,Revive);
        OnExploded?.Invoke();
    }
    private void Revive()
    {
        state = State.Alive;
        transform.position = Vector3.up * 0.5f;
        rig.isKinematic = false;
        OnRevived?.Invoke();
        SetSquareMotionType();
    }
    public bool IsDead() => state == State.Dead;
    public bool IsGrounded()
    {
        Collider2D ground = Physics2D.OverlapCircle(groundDetector.position, groundDetectionRadius, groundMask);
        return ground != null;
    }
    public bool IsPressing() => Input.GetMouseButton(0);
    public bool IsSquareMode() => motionType == MotionType.Square;
    public float GetYVelocity() => rig.velocity.y;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundDetector.position, groundDetectionRadius);
    }
    public void SetSquareMotionType()
    {
        if (motionType == MotionType.Square)
        {
            return;
        }
        motionType = MotionType.Square;
        rig.gravityScale = 1;
        onSquareModeStarted?.Invoke();
    }
    public void SetSpaceshipMotionType()
    {
        if(motionType == MotionType.Spaceship)
        {
            return;
        }
        motionType = MotionType.Spaceship;
        rig.gravityScale = 0;
        onSpaceshipModeStarted?.Invoke();
        Debug.Log("Hit a Spacship Trigger");
    }
}
