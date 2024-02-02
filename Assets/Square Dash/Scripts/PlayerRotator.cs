using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerController))]
public class PlayerRotator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private new Renderer renderer;
    private PlayerController playerController;
    [Header("Settings")]
    [SerializeField] private float angleIncrement;
    [SerializeField] private float leanAngleTime;
    private bool isTweening;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnExploded += Explode;
        playerController.OnRevived += Revive;
    }
    private void OnDestroy()
    {
        playerController.OnExploded -= Explode;
        playerController.OnRevived -= Revive;
    }
    void Update()
    {
        if (playerController.IsDead())
            return;
        ManagePlayerRotation();
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
            LeanTween.cancel(renderer.gameObject);
            isTweening = false;
        }
        renderer.transform.localEulerAngles += Vector3.forward * Time.deltaTime * angleIncrement;
    }
    private void PlayerGroundedState()
    {
        if(playerController.IsPressing())
        {
            if(isTweening) 
            {
                isTweening = false;
                LeanTween.cancel(renderer.gameObject);            
            }
            return;
        }
        if(isTweening)
        {
            return;
        }
        float closestAngle = Mathf.FloorToInt(renderer.transform.localEulerAngles.z / 90) * 90;
        float angleDifference = renderer.transform.localEulerAngles.z - closestAngle;

        if(angleDifference > 45) 
        {
            angleDifference = (90 - angleDifference) * -1; 
        }

        LeanTween.cancel(renderer.gameObject);
        LeanTween.rotateAroundLocal(renderer.gameObject, Vector3.forward, -angleDifference, leanAngleTime);

        isTweening = true;
    }
    private void Explode()
    {
        renderer.enabled = false;
    }
    private void Revive()
    {
        renderer.enabled = true;
        renderer.transform.rotation = Quaternion.identity;
    }
}
