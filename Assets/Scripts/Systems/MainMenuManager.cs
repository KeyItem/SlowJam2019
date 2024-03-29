﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Attributes")]
    public MainMenuSettings menuSettings;

    [Space(10)]
    public Rigidbody[] entityRigidbodies;
    
    [Space(10)]
    public bool hasGameStarted = false;
    
    private float menuTimer;

    private CameraColorChanger cameraColor;

    private WipeTransition transition;
    
    private void Start()
    {
       InitializeMainMenu();
    }
    private void Update()
    {
       ManageMainMenu();
    }
    private void InitializeMainMenu()
    {
        menuTimer = menuSettings.mainMenuWaitTime;

        cameraColor = Camera.main.GetComponent<CameraColorChanger>();

        transition = GameObject.FindObjectOfType<WipeTransition>();
    }

    private void ManageMainMenu()
    {
        if (!hasGameStarted)
        {
            CheckInput();
        }
        else
        {
            CheckTimer();
        }
    }
    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartMenu();
        }
    }

    private void CheckTimer()
    {
        if (hasGameStarted)
        {
            menuTimer -= Time.deltaTime;

            if (menuTimer <= 0f)
            {
                StartGame();

                hasGameStarted = false;
            }
        }
    }
    private void StartMenu()
    {
        hasGameStarted = true;

        cameraColor.isGameStarted = true;
        
        for (int i = 0; i < entityRigidbodies.Length; i++)
        {
            entityRigidbodies[i].isKinematic = false;
        }
    }

    private void StartGame()
    {
        StartTransition();

        LevelManager.Instance.LoadNextLevelAfterDelay(2f);
    }

    private void StartTransition()
    {
        transition.levelFinished = true;
    }
}

[System.Serializable]
public struct MainMenuSettings
{
    [Header("Main Menu Settings")]
    public float mainMenuWaitTime;
}
