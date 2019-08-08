using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : Zone
{
    [Header("Win Zone Attributes")]
    public WinZoneSettings winSettings;
    
    [Space(10)]
    public List<Collider> detectedColliders = new List<Collider>();

    public override void InitializeZone()
    {
        detectedColliders = new List<Collider>();
    }

    public override void ManageZone()
    {
        Collider[] hitObjects = ReturnHitObjects(zoneSettings);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (!detectedColliders.Contains(hitObjects[i]))
            {
                detectedColliders.Add(hitObjects[i]);

                if (CheckForWinCondition())
                {
                    ActivateWinZone();
                }
            }
        }
    }
    
    private void ActivateWinZone()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    public bool CheckForWinCondition()
    {
        if (detectedColliders.Count != 0)
        {
            if (detectedColliders.Count >= winSettings.entitiesNeededToWin)
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public struct WinZoneSettings
{
    [Header("Win Zone Settings")]
    public int entitiesNeededToWin;
}
