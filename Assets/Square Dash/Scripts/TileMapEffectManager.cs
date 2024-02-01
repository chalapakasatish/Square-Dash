using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapEffectManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Renderer[] tilemapRenderers;

    [Header(" Settings ")]
    [SerializeField] private float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (Renderer renderer in tilemapRenderers)
            renderer.material.SetFloat("_Max_Distance", maxDistance);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Renderer renderer in tilemapRenderers)
            renderer.material.SetVector("_Camera_Position", cameraTransform.position);
    }
}
