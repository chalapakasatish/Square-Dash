using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameover : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(LayerMask.LayerToName(collision.collider.gameObject.layer) == "Obstacle")
        {
            transform.position = Vector3.up;
        }
    }
}
