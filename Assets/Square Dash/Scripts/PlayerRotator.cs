using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerController))]
public class PlayerRotator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform rotatorParent;
    private PlayerController playerController;
    [Header("Settings")]
    [SerializeField] private float angleIncrement;
    [SerializeField] private float leanAngleTime;
    private bool isTweening;
    [Header("Spaceship Settings")]
    [SerializeField] private float spaceshipRotationMultiplier;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnExploded += Explode;
        playerController.OnRevived += Revive;
        PlayerController.onSpaceshipModeStarted += SpaceshipModeStarted;
    }

    
    private void OnDestroy()
    {
        playerController.OnExploded -= Explode;
        playerController.OnRevived -= Revive;
        PlayerController.onSpaceshipModeStarted -= SpaceshipModeStarted;
    }
    void Update()
    {
        if (playerController.IsDead())
            return;
        switch(playerController.GetMotionType())
        {
            case MotionType.Square:
                ManagePlayerRotation();
                break;
            case MotionType.Spaceship:
                ManageSpaceshipRotation();
                break;
        }
    }
    public void ManageSpaceshipRotation()
    {
        float yVelocity = playerController.GetYVelocity();
        yVelocity *= spaceshipRotationMultiplier;
        rotatorParent.right = Quaternion.Euler(Vector3.forward * yVelocity) * Vector3.right;
    }
    private void SpaceshipModeStarted()
    {
        LeanTween.cancel(rotatorParent.gameObject);
        rotatorParent.rotation = Quaternion.identity;
    }
    private void ManagePlayerRotation()
    {
        if(!playerController.IsGrounded())
        {
            PlayerNotGroundedState();
        }else
        {
            PlayerGroundedState();
        }
    }
    private void PlayerNotGroundedState()
    {
        if(isTweening)
        {
            LeanTween.cancel(rotatorParent.gameObject);
            isTweening = false;
        }
        rotatorParent.transform.localEulerAngles += Vector3.forward * Time.deltaTime * angleIncrement;
    }
    private void PlayerGroundedState()
    {
        if(playerController.IsPressing())
        {
            if(isTweening) 
            {
                isTweening = false;
                LeanTween.cancel(rotatorParent.gameObject);            
            }
            return;
        }
        if(isTweening)
        {
            return;
        }
        float closestAngle = Mathf.FloorToInt(rotatorParent.transform.localEulerAngles.z / 90) * 90;
        float angleDifference = rotatorParent.transform.localEulerAngles.z - closestAngle;

        if(angleDifference > 45) 
        {
            angleDifference = (90 - angleDifference) * -1; 
        }

        LeanTween.cancel(rotatorParent.gameObject);
        LeanTween.rotateAroundLocal(rotatorParent.gameObject, Vector3.forward, -angleDifference, leanAngleTime);

        isTweening = true;
    }
    

    private void Explode()
    {
        rotatorParent.gameObject.SetActive(false);
    }
    private void Revive()
    {
        rotatorParent.gameObject.SetActive(true);
        rotatorParent.transform.rotation = Quaternion.identity;
    }
}
