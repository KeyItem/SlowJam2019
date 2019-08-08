using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Attributes")]
    public Image playerMoveCooldownMeter;

    private PlayerController playerController;

    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        ManageUI();
    }
    private void InitializeUI()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void ManageUI()
    {
        playerMoveCooldownMeter.fillAmount = playerController.moveEffictiveness / playerController.movementSettings.maxMovementCooldown;
    }
}
