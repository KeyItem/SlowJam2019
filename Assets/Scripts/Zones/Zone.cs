using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [Header("Force Zone Settings")] 
    public ZoneSettings zoneSettings;

    [Space(10)] 
    public bool isZoneActive = true;

    private void Start()
    {
        InitializeZone();
    }

    private void FixedUpdate()
    {
        ManageZone();
    }

    public virtual void InitializeZone()
    {
        
    }
    public virtual void ManageZone()
    {
        Collider[] hitObjects = ReturnHitObjects(zoneSettings);
    }

    public virtual Collider[] ReturnHitObjects(ZoneSettings zoneSettings)
    {
        if (zoneSettings.zoneShape == ZONE_SHAPE.BOX)
        {
            Collider[] hitObjects = Physics.OverlapBox(transform.position, zoneSettings.zoneSize / 2, Quaternion.Euler(zoneSettings.zoneRotation), zoneSettings.zoneCollisionMask);

            return hitObjects;
        }
        else
        {
            Collider[] hitObjects = Physics.OverlapSphere(transform.position, zoneSettings.zoneRadius, zoneSettings.zoneCollisionMask);

            return hitObjects;
        }
    }

    private void OnDrawGizmos()
    {
        if (zoneSettings.zoneShape == ZONE_SHAPE.BOX)
        {
            Gizmos.color = Color.yellow;
            
            Matrix4x4 cubeMatrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(zoneSettings.zoneRotation), zoneSettings.zoneSize);
            Matrix4x4 oldMatrix = Gizmos.matrix;

            Gizmos.matrix *= cubeMatrix;
            
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            Gizmos.matrix = oldMatrix;
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, zoneSettings.zoneRadius);
        }
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, zoneSettings.zoneForceDirection);       
    }
}

[System.Serializable]
public struct ZoneSettings
{
    [Header("Force Zone Settings")] 
    public ZONE_SHAPE zoneShape;

    [Space(10)] 
    public Vector3 zoneSize;
    public Vector3 zoneRotation;
    
    [Space(10)]
    public float zoneRadius;

    [Space(10)] 
    public Vector3 zoneForceDirection;

    public float zoneForceStrength;

    [Space(10)] 
    public LayerMask zoneCollisionMask;
}

public enum ZONE_SHAPE
{
    BOX,
    SPHERE
}