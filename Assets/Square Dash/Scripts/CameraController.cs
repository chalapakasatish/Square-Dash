using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform target;
    [Header("Settings")]
    [SerializeField] private float yFollowThreshold;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float backToRestLerp;
    [SerializeField] private float yDistanceFelloePower;
    [SerializeField] private float baseYLerp;

    private bool isSpaceshipMode;
    private void Awake()
    {
        isSpaceshipMode = false;
        PlayerController.onSpaceshipModeStarted += SpaceshipModeStartedCallback;
        PlayerController.onSquareModeStarted += SquareModeStartedCallback;
    }

    private void OnDestroy()
    {
        PlayerController.onSpaceshipModeStarted -= SpaceshipModeStartedCallback;
        PlayerController.onSpaceshipModeStarted -= SquareModeStartedCallback;
    }
    private void SpaceshipModeStartedCallback()
    {
        isSpaceshipMode = true;
    }
    private void SquareModeStartedCallback()
    {
        isSpaceshipMode = false;
    }
    private void LateUpdate()
    {
        float targetX = target.position.x + xOffset;
        float targetY = GetTargetY(isSpaceshipMode);
        
        Vector3 targetPosition = new Vector3(targetX, targetY, -10);
        transform.position = targetPosition;
    }
    private float GetTargetY(bool isSpaceshipMode)
    {
        if (isSpaceshipMode)
        {
            return yOffset;
        }
        float targetY = target.position.y;

        //player below the line
        if (target.position.y < yFollowThreshold)
        {
            targetY = Mathf.Lerp(transform.position.y, yOffset, Time.deltaTime * backToRestLerp);
        }
        else
        {
            float lerpFactor = Mathf.Abs(transform.position.y - target.position.y);
            float lerpMultiplier = Mathf.Pow(lerpFactor, yDistanceFelloePower);
            targetY = Mathf.Lerp(transform.position.y, target.position.y + yOffset, Time.deltaTime * lerpMultiplier * baseYLerp);
        }
        return targetY;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 from = Vector3.up * yFollowThreshold;
        from += Vector3.left * 50;
        Vector3 to = from + Vector3.right * 100;
        Gizmos.DrawLine(from,to);
        //Gizmos.DrawLine(Vector3.up * yFollowThreshold, Vector3.up * yFollowThreshold + Vector3.right * 10);
    }
}
