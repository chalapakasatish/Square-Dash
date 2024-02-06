using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRendererManager : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private GameObject squareRenderer;
    [SerializeField] private GameObject spaceshipRenderer;

    private void Awake()
    {
        PlayerController.onSpaceshipModeStarted += SpaceshipModeStartedCallback;
        PlayerController.onSquareModeStarted += SquareModeStartedCallback;
    }
    private void OnDestroy()
    {
        PlayerController.onSpaceshipModeStarted -= SpaceshipModeStartedCallback;
        PlayerController.onSquareModeStarted -= SquareModeStartedCallback;
    }
    private void SpaceshipModeStartedCallback()
    {
        squareRenderer.gameObject.SetActive(false);
        spaceshipRenderer.gameObject.SetActive(true);
    }
    private void SquareModeStartedCallback()
    {
        squareRenderer.gameObject.SetActive(true);
        spaceshipRenderer.gameObject.SetActive(false);
    }
}
