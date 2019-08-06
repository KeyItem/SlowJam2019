﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [Header("Entity Attributes")] 
    public EntityInfo entityInfo;
    
    [Space(10)] 
    private CameraFollow cameraFollow;
    
    [HideInInspector]
    public Rigidbody entityRigidbody;

    [Header("Entity Collision Attributes")]
    public EntityCollisionInfo collisionInfo;

    private void Awake()
    {
        InitializeEntity();
    }

    private void Update()
    {
        ManageCollision();
    }

    private void InitializeEntity()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        
        entityRigidbody = GetComponent<Rigidbody>();

        entityInfo.entityStartingPosition = transform.position;
        entityInfo.resizingSettings.entityBaseScale = transform.localScale;
    }

    public virtual void ManageCollision()
    {
        if (Physics.Raycast(transform.position, Vector3.down, entityInfo.collisionSettings.groundCheckRayLength, entityInfo.collisionSettings.groundCollisionMask))
        {   
            collisionInfo = new EntityCollisionInfo(true);
        }
        else
        {
            collisionInfo = new EntityCollisionInfo(false);
        }
    }
    public virtual void MoveEntity(Vector3 moveDirection, float effectiveness)
    {
        entityRigidbody.AddForce((entityInfo.movementSettings.entityMovementMultiplier * effectiveness) * Time.deltaTime * moveDirection, ForceMode.Impulse);
    }
    
    /*
    public virtual void SpecialShrink(Vector3 hitPoint)
    {
        Mesh myMesh = entityMesh.mesh;

        Vector3[] modifiedVertices = myMesh.vertices;

        Vector3 meshCenter = transform.position;
   
        Debug.Log("Hit Object :: " + gameObject.name);
        
        for (int i = 0; i < modifiedVertices.Length; i++)
        {  
            Vector3 worldMeshPoint = transform.TransformPoint(modifiedVertices[i]);
            Vector3 interceptDirection = (meshCenter - worldMeshPoint).normalized;

            float distanceBetweenPoint = Vector3.Distance(hitPoint, worldMeshPoint);

            if (distanceBetweenPoint < entityInfo.resizingSettings.modifyThreshold)
            {
                float distanceRatio = (entityInfo.resizingSettings.modifyThreshold - distanceBetweenPoint) / entityInfo.resizingSettings.modifyThreshold;
                
                modifiedVertices[i] += distanceRatio * entityInfo.resizingSettings.shrinkMultiplier * Time.deltaTime * interceptDirection;
            }
        }

        myMesh.vertices = modifiedVertices;
        
        myMesh.RecalculateNormals();

        entityMeshCollider.sharedMesh = myMesh;
    }

    
    public virtual void SpecialEnlarge(Vector3 hitPoint)
    {
        Mesh myMesh = entityMesh.mesh;

        Vector3[] modifiedVertices = myMesh.vertices;

        Vector3 meshCenter = transform.position;
   
        Debug.Log("Hit Object :: " + gameObject.name);
        
        for (int i = 0; i < modifiedVertices.Length; i++)
        {  
            Vector3 worldMeshPoint = transform.TransformPoint(modifiedVertices[i]);
            Vector3 interceptDirection = (worldMeshPoint - meshCenter).normalized;

            float distanceBetweenPoint = Vector3.Distance(hitPoint, worldMeshPoint);

            if (distanceBetweenPoint < entityInfo.resizingSettings.modifyThreshold)
            {
                float distanceRatio = (entityInfo.resizingSettings.modifyThreshold - distanceBetweenPoint) / entityInfo.resizingSettings.modifyThreshold;
                
                modifiedVertices[i] += distanceRatio * entityInfo.resizingSettings.shrinkMultiplier * Time.deltaTime * interceptDirection;
            }
        }

        myMesh.vertices = modifiedVertices;
        
        myMesh.RecalculateNormals();

        entityMeshCollider.sharedMesh = myMesh;
    }
    
    */
    
    public virtual void Shrink(float multiplier = 1f)
    {
        if (entityInfo.resizingSettings.canBeShrinked)
        {
            Vector3 newSize = ReturnNewSize(SIZE_DIRECTION.SHRINK, multiplier, transform.localScale);
                                   
            if (newSize.sqrMagnitude < entityInfo.resizingSettings.minSize)
            {
                if (entityInfo.resizingSettings.isKilledAfterMinSize)
                {
                    cameraFollow.RemoveTarget(transform);
                
                    Destroy(gameObject);
                
                    return;
                }
            }
        
            transform.localScale = newSize;
        }
    }

    public virtual void ShrinkByPercentage(float newPercentage)
    {
        if (entityInfo.resizingSettings.canBeShrinked)
        {
            Vector3 newSize = ReturnNewSizeByPercent(SIZE_DIRECTION.SHRINK, newPercentage, transform.localScale);
                                   
            if (newSize.sqrMagnitude < entityInfo.resizingSettings.minSize)
            {
                if (entityInfo.resizingSettings.isKilledAfterMinSize)
                {
                    cameraFollow.RemoveTarget(transform);
                
                    Destroy(gameObject);
                
                    return;
                }
            }
        
            transform.localScale = newSize;
        }
    }

    public virtual void Enlarge(float multiplier = 1f)
    {
        if (entityInfo.resizingSettings.canBeEnlarged)
        {
            Vector3 newSize = ReturnNewSize(SIZE_DIRECTION.ENLARGE, multiplier, transform.localScale);
                            
            if (newSize.sqrMagnitude > entityInfo.resizingSettings.maxSize)
            {
                if (entityInfo.resizingSettings.isKilledAfterMaxSize)
                {
                    cameraFollow.RemoveTarget(transform);
                
                    Destroy(gameObject);
                
                    return;
                }
            }
        
            transform.localScale = newSize;
        }
    }

    public virtual void EnlargeByPercentage(float newPercentage)
    {
        if (entityInfo.resizingSettings.canBeShrinked)
        {
            Vector3 newSize = ReturnNewSizeByPercent(SIZE_DIRECTION.ENLARGE, newPercentage, transform.localScale);
                                   
            if (newSize.sqrMagnitude < entityInfo.resizingSettings.minSize)
            {
                if (entityInfo.resizingSettings.isKilledAfterMinSize)
                {
                    cameraFollow.RemoveTarget(transform);
                
                    Destroy(gameObject);
                
                    return;
                }
            }
        
            transform.localScale = newSize;
        }
    }

    public virtual void ResetEntity()
    {
        transform.position = entityInfo.entityStartingPosition;
        
        transform.localScale = entityInfo.resizingSettings.entityBaseScale;
    }

    public virtual void SelectEntity()
    {
        
    }

    public virtual void UnSelectEntity()
    {
        
    }
    
    private Vector3 ReturnNewSize(SIZE_DIRECTION sizeDirection, float overallMultiplier, Vector3 baseSize)
    {
        Vector3 newSize = baseSize;
        float sizeMultiplier = 0;

        if (sizeDirection == SIZE_DIRECTION.SHRINK)
        {
            sizeMultiplier = entityInfo.resizingSettings.shrinkMultiplier;

            if (entityInfo.resizingSettings.canXBeModified)
            {
                newSize.x -= (1 * sizeMultiplier * Time.deltaTime) * overallMultiplier;
            }

            if (entityInfo.resizingSettings.canYBeModified)
            {
                newSize.y -= (1 * sizeMultiplier * Time.deltaTime) * overallMultiplier;
            }

            if (entityInfo.resizingSettings.canZBeModified)
            {
                newSize.z -= (1 * sizeMultiplier * Time.deltaTime) * overallMultiplier;
            }
        }
        else
        {
            sizeMultiplier = entityInfo.resizingSettings.enlargeMultiplier;
            
            if (entityInfo.resizingSettings.canXBeModified)
            {
                newSize.x += (1 * sizeMultiplier * Time.deltaTime) * overallMultiplier;
            }

            if (entityInfo.resizingSettings.canYBeModified)
            {
                newSize.y += (1 * sizeMultiplier * Time.deltaTime) * overallMultiplier;
            }

            if (entityInfo.resizingSettings.canZBeModified)
            {
                newSize.z += (1 * sizeMultiplier * Time.deltaTime) * overallMultiplier;
            }
        }

        return newSize;
    }

    private Vector3 ReturnNewSizeByPercent(SIZE_DIRECTION sizeDirection, float sizePercent, Vector3 baseSize)
    {
        Vector3 newSize = baseSize;

        if (sizeDirection == SIZE_DIRECTION.SHRINK)
        {
            if (entityInfo.resizingSettings.canXBeModified)
            {
                newSize.x /= sizePercent;
            }

            if (entityInfo.resizingSettings.canYBeModified)
            {
                newSize.y /= sizePercent;
            }

            if (entityInfo.resizingSettings.canZBeModified)
            {
                newSize.z /= sizePercent;
            }
        }
        else
        {
            if (entityInfo.resizingSettings.canXBeModified)
            {
                newSize.x *= sizePercent;
            }

            if (entityInfo.resizingSettings.canYBeModified)
            {
                newSize.y *= sizePercent;
            }

            if (entityInfo.resizingSettings.canZBeModified)
            {
                newSize.z *= sizePercent;
            }
        }

        return newSize;
    }

    public virtual bool CanMoveInDirection(Vector3 direction)
    {
        if (collisionInfo.isGrounded)
        {
            if (direction == Vector3.left)
            {
                if (entityInfo.movementSettings.canMoveLeft)
                {
                    return true;
                }
            }
            else if (direction == Vector3.right)
            {
                if (entityInfo.movementSettings.canMoveRight)
                {
                    return true;
                }
            }
            else if (direction == Vector3.up)
            {
                if (entityInfo.movementSettings.canMoveUp)
                {
                    return true;
                }
            }
            else if (direction == Vector3.down)
            {
                if (entityInfo.movementSettings.canMoveDown)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        
        Gizmos.DrawRay(transform.position, Vector3.down * entityInfo.collisionSettings.groundCheckRayLength);
    }
}

[System.Serializable]
public struct EntityInfo
{
    public string entityName;

    [Space(10)]
    public Vector3 entityStartingPosition;
    
    [Space(10)]
    public EntityMovementSettings movementSettings;
    
    [Space(10)]
    public EntityResizingSettings resizingSettings;
    
    [Space(10)]
    public EntityCollisionSettings collisionSettings;
}

[System.Serializable]
public struct EntityMovementSettings
{
    [Header("Entity Movement Settings")]
    public float entityMovementMultiplier;

    [Space(10)]
    public bool canMoveUp;
    public bool canMoveDown;
    public bool canMoveLeft;
    public bool canMoveRight;
}

[System.Serializable]
public struct EntityResizingSettings
{
    [Header("Entity Size Attributes")] 
    public Vector3 entityBaseScale;

    [Space(10)] 
    public float modifyThreshold;
    
    [Space(10)]
    public bool canXBeModified;
    public bool canYBeModified;
    public bool canZBeModified;
    
    [Space(10)]
    public float shrinkMultiplier;
    public float enlargeMultiplier;
    
    [Space(10)]
    public float minSize;
    public float maxSize;

    [Space(10)] 
    public bool canBeShrinked;
    public bool canBeEnlarged;
    
    [Space(10)] 
    public bool isKilledAfterMinSize;
    public bool isKilledAfterMaxSize;
}

[System.Serializable]
public struct EntityCollisionSettings
{
    [Header("Entity Collision Settings")]
    public float groundCheckRayLength;
    
    [Space(10)]
    public LayerMask groundCollisionMask;
}

[System.Serializable]
public struct EntityCollisionInfo
{
    [Header("Entity Collision Info")]
    public bool isGrounded;

    public EntityCollisionInfo(bool newIsGrounded)
    {
        this.isGrounded = newIsGrounded;
    }
}

public enum SIZE_DIRECTION
{
    SHRINK,
    ENLARGE
}
