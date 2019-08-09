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
    private int lastIndex = -1;

    [Space(10)]
    private float timer;
    
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

        timer = 0f;
    }
    private void ManageCamera()
    {
        float t = Mathf.PingPong(Time.time, cameraColorSettings.cameraColorChangeSpeed) / cameraColorSettings.cameraColorChangeSpeed;
        
        Color newColor = Color.Lerp(targetCamera.backgroundColor, nextColor, t);

        targetCamera.backgroundColor = newColor;
        
        if (t == 1)
        {
            t = 0;
            
            int newIndex = ReturnRandomIndex();

            if (CheckForDuplicateColor(newIndex))
            {
                nextColor = ReturnRandomColor(newIndex);
            }
        }
    }

    private bool CheckForDuplicateColor(int newIndex)
    {
        if (newIndex == lastIndex)
        {
            return false;
        }

        return true;
    }
    
    private int ReturnRandomIndex()
    {
       return Random.Range(0, cameraColorSettings.cameraColors.Length);
    }
    
    private Color ReturnRandomColor(int randomIndex)
    {
        return cameraColorSettings.cameraColors[randomIndex];
    }
}

[System.Serializable]
public struct CameraColorSettings
{
    [Header("Camera Color Settings")]
    public Color[] cameraColors;

    [Space(10)]
    public float cameraColorChangeSpeed;
}
