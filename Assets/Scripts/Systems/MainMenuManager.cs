using System;
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
            }
        }
    }
    private void StartMenu()
    {
        hasGameStarted = true;
        
        for (int i = 0; i < entityRigidbodies.Length; i++)
        {
            entityRigidbodies[i].isKinematic = false;
        }
    }

    private void StartGame()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}

[System.Serializable]
public struct MainMenuSettings
{
    [Header("Main Menu Settings")]
    public float mainMenuWaitTime;
}
