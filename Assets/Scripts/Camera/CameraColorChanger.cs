using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraColorChanger : MonoBehaviour
{
    [Header("Camera Color Settings")]
    public CameraColorSettings cameraColorSettings;

    [Space(10)]
    private Camera targetCamera;
    
    [Space(10)]
    public Color nextColor;

    [Space(10)]
    public bool isGameStarted = false;

    private void Start()
    {
        InitializeColorChanger();
    }
    private void Update()
    {
        ManageCamera();
    }
    private void InitializeColorChanger()
    {
        targetCamera = Camera.main;;
    }
    private void ManageCamera()
    {
        if (isGameStarted)
        {
            float t = Mathf.PingPong(Time.time, cameraColorSettings.cameraColorChangeSpeed) / cameraColorSettings.cameraColorChangeSpeed;
        
            Color newColor = Color.Lerp(targetCamera.backgroundColor, nextColor, t);

            targetCamera.backgroundColor = newColor;
        }
    }
}

[System.Serializable]
public struct CameraColorSettings
{
    [Header("Camera Color Settings")]
    public float cameraColorChangeSpeed;
}
