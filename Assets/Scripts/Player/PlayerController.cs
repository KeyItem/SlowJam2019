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

    private void Awake()
    {
        InitializeEntity();
    }
    
    private void InitializeEntity()
    {
        input = GetComponent<PlayerInput>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        ManageInput();
        ManageInteraction(inputValues);
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
                hitEntity.entityController.SpecialShrink(hitEntity.hitPoint);
            }
        }
        else if (CheckForEnlargeInteractionInput(input))
        {
            HitEntityInfo hitEntity = interaction.SearchForEntity();

            if (hitEntity.entityController != null)
            {
                hitEntity.entityController.SpecialEnlarge(hitEntity.hitPoint);
            }
        }
    }

    private bool CheckForShrinkInteractionInput(InputInfo input)
    {
        if (inputValues.ReturnCurrentButtonState("Interact_Shrink"))
        {
            return true;
        }

        return false;
    }
    
    private bool CheckForEnlargeInteractionInput(InputInfo input)
    {
        if (inputValues.ReturnCurrentButtonState("Interact_Enlarge"))
        {
            return true;
        }

        return false;
    }
}


