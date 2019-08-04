﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Input")] 
    public PlayerInput input;

    private InputInfo inputValues;
    
    [Header("Player Interaction")] 
    public PlayerInteraction interaction;

    [Header("Player Movement ")]
    public PlayerMovementSettings movementSettings;
    
    [Space(10)]
    public float moveEffictiveness;
    
    [Header("Selected Entity")]
    public EntityController selectedEntity;

    private void Awake()
    {
        InitializeEntity();
    }
    
    private void InitializeEntity()
    {
        input = GetComponent<PlayerInput>();
        interaction = GetComponent<PlayerInteraction>();

        moveEffictiveness = movementSettings.maxMovementCooldown;
    }

    private void Update()
    {
        ManageInput();
        
        ManageMoveEffectiveness();
        
        ManageInteraction(inputValues);
    }

    private void FixedUpdate()
    {
        ManageMovement(inputValues);
    }

    private void ManageInput()
    {
        input.GetInput();
        
        inputValues = input.ReturnInput();
    }

    private void ManageInteraction(InputInfo input)
    {
        if (CheckForShrinkInteractionInput(input))
        {
            HitEntityInfo hitEntity = interaction.SearchForEntity();

            if (hitEntity.entityController != null)
            {
                if (hitEntity.entityController != selectedEntity)
                {
                    selectedEntity = hitEntity.entityController;
                    
                    return;
                }
                else
                {
                    selectedEntity.Shrink();
                }
            }
        }
        else if (CheckForEnlargeInteractionInput(input))
        {
            if (selectedEntity != null)
            {
                selectedEntity.Enlarge();
            }
        }
    }

    private void ManageMovement(InputInfo input)
    {
        if (selectedEntity != null)
        {
            if (CheckForMovementInput(input))
            {
                Vector3 moveDirection = DetermineMovementDirection(input);

                if (selectedEntity.CanMoveInDirection(moveDirection))
                {
                    selectedEntity.MoveEntity(moveDirection, ReturnMoveEffectivenessRatio(moveEffictiveness));

                    moveEffictiveness = 0;
                }
            }
        }
    }

    private void ManageMoveEffectiveness()
    {
        if (moveEffictiveness < movementSettings.maxMovementCooldown)
        {
            moveEffictiveness += Time.deltaTime;

            if (moveEffictiveness > movementSettings.maxMovementCooldown)
            {
                moveEffictiveness = movementSettings.maxMovementCooldown;
            }
        }
    }

    private float ReturnMoveEffectivenessRatio(float currentMovementEffectiveness)
    {
        return currentMovementEffectiveness / movementSettings.maxMovementCooldown;
    }

    private Vector3 DetermineMovementDirection(InputInfo input)
    {
        if (input.ReturnCurrentButtonState("Move_Left"))
        {
            return Vector3.left;
        }
       
        if (input.ReturnCurrentButtonState("Move_Right"))
        {
            return Vector3.right;
        }
        
        if (input.ReturnCurrentButtonState("Move_Up"))
        {
            return Vector3.up;
        }
        
        if (input.ReturnCurrentButtonState("Move_Down"))
        {
            return Vector3.down;
        }

        return Vector3.zero;
    }

    private bool CheckForMovementInput(InputInfo input)
    {
        if (input.ReturnCurrentButtonState("Move_Left") || input.ReturnCurrentButtonState("Move_Right") || input.ReturnCurrentButtonState("Move_Up") || input.ReturnCurrentButtonState("Move_Down"))
        {
            return true;
        }

        return false;
    }
    
    private bool CheckForShrinkInteractionInput(InputInfo input)
    {
        if (input.ReturnCurrentButtonState("Interact_Shrink"))
        {
            return true;
        }

        return false;
    }
    
    private bool CheckForEnlargeInteractionInput(InputInfo input)
    {
        if (input.ReturnCurrentButtonState("Interact_Enlarge"))
        {
            return true;
        }

        return false;
    }
}

[System.Serializable]
public struct PlayerMovementSettings
{
    [Header("Player Movement Settings")]
    public float maxMovementCooldown;
}


