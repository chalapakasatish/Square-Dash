using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
public class PlayerDetection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask gameoverMask;

    [Header("Components")]
    [SerializeField] private Collider2D gameoverTrigger;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.IsTouching(gameoverTrigger) && IsInLayerMask(collider.gameObject.layer,gameoverMask))
        {
            Explode();
        }
        if(collider.TryGetComponent(out SpaceshipTrigger spaceshipTrigger))
        {
            playerController.SetSpaceshipMotionType();
        }
        if (collider.TryGetComponent(out SquareTrigger squareTrigger))
        {
            playerController.SetSquareMotionType();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(LayerMask.LayerToName(collision.collider.gameObject.layer) == "Obstacle")
        {
            Explode();
        }
    }
    private void Explode() => playerController.Explode();
    private bool IsInLayerMask(int layer,LayerMask layerMask)
    {
        // 1000000
        // Ground -> 6 ->       01000000
        // Obstacles -> 7 ->    10000000
        // LayerMask            11000000
        return (layerMask.value & (1  << layer)) != 0;
    }
}
