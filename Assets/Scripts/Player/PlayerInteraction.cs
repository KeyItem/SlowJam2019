using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Entity Interaction Settings")]
    public EntityInteractionSettings interactionSettings;

    [Space(10)]
    public HitEntityInfo hitEntityInfo;
    
    public EntityController selectedEntity;
    public EntityController lastSelectedEntity;
    
    public virtual void ManageInteraction()
    {
        hitEntityInfo = SearchForEntity();
        
        ManageHover(hitEntityInfo);
    }
    
    public virtual void ManageHover(HitEntityInfo newHitEntityInfo)
    {
        if (selectedEntity == null)
        {
            if (newHitEntityInfo.entityController != null)
            {
                if (newHitEntityInfo.entityController == lastSelectedEntity)
                {
                    return;
                }
                else
                {
                    if (lastSelectedEntity != null)
                    {
                        lastSelectedEntity.StopHover();
                        
                        newHitEntityInfo.entityController.StartHover();

                        lastSelectedEntity = newHitEntityInfo.entityController;
                    }
                    else
                    {
                        newHitEntityInfo.entityController.StartHover();
                        
                        lastSelectedEntity = newHitEntityInfo.entityController;
                    }
                }
            }
            else
            {
                if (lastSelectedEntity != null)
                {
                    lastSelectedEntity.StopHover();

                    lastSelectedEntity = null;
                }
            }
        }
    }

    public virtual void ShrinkSelectedEntity()
    {
        if (selectedEntity != null)
        {
            if (hitEntityInfo.entityController != selectedEntity)
            {
                selectedEntity.UnSelectEntity();
                hitEntityInfo.entityController.SelectEntity();

                selectedEntity = hitEntityInfo.entityController;
            }
            
            selectedEntity.Shrink();
        }
        else
        {
            if (hitEntityInfo.entityController != null)
            {
                hitEntityInfo.entityController.SelectEntity();
                
                selectedEntity = hitEntityInfo.entityController;
                
                selectedEntity.Shrink();
            }
        }
    }

    public virtual void EnlargeSelectedEntity()
    {
        if (selectedEntity != null)
        {
            if (hitEntityInfo.entityController != selectedEntity)
            {
                selectedEntity.UnSelectEntity();
                hitEntityInfo.entityController.SelectEntity();

                selectedEntity = hitEntityInfo.entityController;
            }
            
            selectedEntity.Enlarge();
        }
        else
        {
            if (hitEntityInfo.entityController != null)
            {
                hitEntityInfo.entityController.SelectEntity();
                
                selectedEntity = hitEntityInfo.entityController;
                
                selectedEntity.Enlarge();
            }
        }
    }
    
    public virtual HitEntityInfo SearchForEntity()
    {
        Ray checkRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit checkRayInfo = new RaycastHit();

        if (Physics.Raycast(checkRay, out checkRayInfo, interactionSettings.interactionMask))
        {
            return new HitEntityInfo(checkRayInfo.transform.GetComponent<EntityController>(), checkRayInfo.point);
        }

        return new HitEntityInfo();
    }
}

[System.Serializable]
public struct EntityInteractionSettings
{
    [Header("Entity Interaction Settings")]
    public float interactionCheckSize;
    
    [Space(10)]
    public LayerMask interactionMask;
}

[System.Serializable]
public struct HitEntityInfo
{
    public EntityController entityController;
    public Vector3 hitPoint;

    public HitEntityInfo(EntityController newEntityController, Vector3 newHitPoint)
    {
        this.entityController = newEntityController;
        this.hitPoint = newHitPoint;
    }
}
