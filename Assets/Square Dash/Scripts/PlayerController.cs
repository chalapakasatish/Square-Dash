using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerState state;

    private MotionType motionType;

    [Header("Components")]
    private MotionController motionController;
    private SquareMotionController squareMotionController;
    private SpaceshipMotionController spaceshipMotionController;

    [Header("Data")]
    [SerializeField] private SquareMotionData squareMotionData;
    [SerializeField] private SpaceshipMotionData spaceshipMotionData;

    [Header("Elements")]
    public Transform groundDetector;
    public LayerMask groundMask;

    [Header("Settings")]
    [SerializeField] private float groundDetectionRadius;

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
        state = PlayerState.Alive;
        motionType = MotionType.Square;

        squareMotionController = new SquareMotionController(gameObject, squareMotionData);
        spaceshipMotionController = new SpaceshipMotionController(gameObject, spaceshipMotionData);
    }
    private void Start()
    {
        motionController = squareMotionController;
    }
    void Update()
    {
        if (IsDead())
        {
            return;
        }
        motionController.Update(IsGrounded());
    }
    private void FixedUpdate()
    {
        if (IsDead())
        {
            return;
        }
        motionController.FixedUpdate();
    }

    public void Explode()
    {
        state = PlayerState.Dead;
        motionController.Explode();
        explodeParticles.Play();
        LeanTween.delayedCall(2,Revive);
        OnExploded?.Invoke();
    }
    private void Revive()
    {
        state = PlayerState.Alive;
        transform.position = Vector3.up * 0.5f;
        motionController.Revive();
        OnRevived?.Invoke();
        SetSquareMotionType();
    }
    public bool IsDead() => state == PlayerState.Dead;
    public bool IsGrounded()
    {
        Collider2D ground = Physics2D.OverlapCircle(groundDetector.position, groundDetectionRadius, groundMask);
        return ground != null;
    }
    public bool IsPressing() => Input.GetMouseButton(0);
    public bool IsSquareMode() => motionType == MotionType.Square;
    public float GetYVelocity() => motionController.GetYVelocity();

    public MotionType GetMotionType() => motionType;
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
        motionController.SetGravityScale(1);
        motionController = squareMotionController;
        onSquareModeStarted?.Invoke();
    }
    public void SetSpaceshipMotionType()
    {
        if(motionType == MotionType.Spaceship)
        {
            return;
        }
        motionType = MotionType.Spaceship;
        motionController.SetGravityScale(0);
        onSpaceshipModeStarted?.Invoke();
        motionController = spaceshipMotionController;
        Debug.Log("Hit a Spacship Trigger");
    }
}
