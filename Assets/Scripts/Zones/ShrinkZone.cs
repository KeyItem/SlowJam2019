using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkZone : Zone
{
    public override void ManageZone()
    {
        Collider[] hitObjects = ReturnHitObjects(zoneSettings);

        List<EntityController> hitControllers = ReturnHitControllers(hitObjects);

        for (int i = 0; i < hitControllers.Count; i++)
        {
            hitControllers[i].Enlarge();
        }
    }

    private List<EntityController> ReturnHitControllers(Collider[] hitObjects)
    {
        List<EntityController> newHitControllers = new List<EntityController>();

        for (int i = 0; i < hitObjects.Length; i++)
        {
            EntityController newController = hitObjects[i].GetComponent<EntityController>();

            if (newController != null)
            {
                newHitControllers.Add(newController);
            }
        }

        return newHitControllers;
    }
}