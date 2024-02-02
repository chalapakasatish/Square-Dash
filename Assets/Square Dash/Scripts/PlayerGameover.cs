using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
public class PlayerGameover : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Collider2D gameoverTrigger;
    private PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.IsTouching(gameoverTrigger))
        {
            Explode();
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
}
