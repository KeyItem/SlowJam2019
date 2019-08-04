using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [Header("Entity Size Settings")] 
    public EntityInfo entityInfo;
    
    [Space(10)] 
    private CameraFollow cameraFollow;

    private MeshCollider entityMeshCollider;
    private MeshFilter entityMesh;

    [HideInInspector]
    public Rigidbody entityRigidbody;
    
    private void Awake()
    {
        InitializeEntity();
    }

    private void InitializeEntity()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        entityMesh = GetComponent<MeshFilter>();

        entityRigidbody = GetComponent<Rigidbody>();

        entityMeshCollider = GetComponent<MeshCollider>();

        entityInfo.resizingSettings.entityBaseScale = transform.localScale;
    }
    public virtual void MoveEntity(Vector3 moveDirection, float effectiveness)
    {
        entityRigidbody.AddForce((entityInfo.movementSettings.entityMovementMultiplier * effectiveness) * Time.deltaTime * moveDirection, ForceMode.Impulse);
    }
    
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
    
    public virtual void Shrink()
    {
        if (entityInfo.resizingSettings.canBeShrinked)
        {
            Vector3 newSize = ReturnNewSize(SIZE_DIRECTION.SHRINK, transform.localScale);
            newSize -= entityInfo.resizingSettings.shrinkMultiplier * Time.deltaTime * Vector3.one;
                                   
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

    public virtual void Enlarge()
    {
        if (entityInfo.resizingSettings.canBeEnlarged)
        {
            Vector3 newSize = ReturnNewSize(SIZE_DIRECTION.ENLARGE, transform.localScale);
                            
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

    public virtual void ResetEntitySize()
    {
        transform.localScale = entityInfo.resizingSettings.entityBaseScale;
    }

    private Vector3 ReturnNewSize(SIZE_DIRECTION sizeDirection, Vector3 baseSize)
    {
        Vector3 newSize = baseSize;
        float sizeMultiplier = 0;

        if (sizeDirection == SIZE_DIRECTION.SHRINK)
        {
            sizeMultiplier = entityInfo.resizingSettings.shrinkMultiplier;

            if (entityInfo.resizingSettings.canXBeModified)
            {
                newSize.x -= 1f * sizeMultiplier * Time.deltaTime;
            }

            if (entityInfo.resizingSettings.canYBeModified)
            {
                newSize.y -= 1f * sizeMultiplier * Time.deltaTime;
            }

            if (entityInfo.resizingSettings.canZBeModified)
            {
                newSize.z -= 1f * sizeMultiplier * Time.deltaTime;
            }
        }
        else
        {
            sizeMultiplier = entityInfo.resizingSettings.enlargeMultiplier;
            
            if (entityInfo.resizingSettings.canXBeModified)
            {
                newSize.x += 1 * sizeMultiplier * Time.deltaTime;
            }

            if (entityInfo.resizingSettings.canYBeModified)
            {
                newSize.y += 1 * sizeMultiplier * Time.deltaTime;
            }

            if (entityInfo.resizingSettings.canZBeModified)
            {
                newSize.z += 1 * sizeMultiplier * Time.deltaTime;
            }
        }

        return newSize;
    }

    public virtual bool CanMoveInDirection(Vector3 direction)
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

        return false;
    }
}

[System.Serializable]
public struct EntityInfo
{
    public string entityName;
    
    [Space(10)]
    public EntityMovementSettings movementSettings;
    
    [Space(10)]
    public EntityResizingSettings resizingSettings;
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

public enum SIZE_DIRECTION
{
    SHRINK,
    ENLARGE
}
