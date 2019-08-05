using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Attributes")]
    public ItemSettings itemSettings;

    [Space(10)]
    public int itemUseCount = 0;

    private void Start()
    {
        InitializeItem();
    }

    private void Update()
    {
        ManageItem();
    }

    public virtual void InitializeItem()
    {
       
    }
    public virtual void ManageItem()
    {
        Collider[] hitColliders = ReturnHitObjects();
        
        if (hitColliders.Length > 0)
        {
            ApplyEffect(hitColliders);
        }
    }

    public virtual void ApplyEffect(Collider[] hitColliders)
    {
        itemUseCount++;

        if (itemUseCount > itemSettings.itemMaxUseCount)
        {
            Destroy(gameObject);
        }
    }

    public virtual Collider[] ReturnHitObjects()
    {
        return Physics.OverlapSphere(transform.position, itemSettings.itemCollisionRadius, itemSettings.itemCollisionMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, itemSettings.itemCollisionRadius);
    }
}

[System.Serializable]
public struct ItemSettings
{
    [Header("Item Settings")]
    public float itemCollisionRadius;

    [Space(10)]
    public int itemMaxUseCount;

    [Space(10)]
    public LayerMask itemCollisionMask;
}