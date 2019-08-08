using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{    
   [Header("Camera Attributes")]
   public List<Transform> cameraActiveTargets = new List<Transform>();

   [Space(10)] public CameraSettings cameraSettings;
   
   private Camera targetCamera;

   private Vector3 cameraMovementSmoothingVelocity;
   private float cameraZoomSmoothingVelocity;

   private void Awake()
   {
       InitializeCamera();
   }

   private void LateUpdate()
   {
       ManageCamera();
   }

   private void InitializeCamera()
   {
       targetCamera = Camera.main;
   }

   private void ManageCamera()
   {
       ManageCameraMovement();
       ManageCameraRotation();
       ManageCameraZoom();
   }

   private void ManageCameraMovement()
   {
       Vector3 newCameraCenterPoint = ReturnMedianPositionBetweenTargets(cameraActiveTargets);
       Vector3 newCameraTargetPosition = newCameraCenterPoint + cameraSettings.offset;
       Vector3 newCameraPosition = Vector3.SmoothDamp(transform.position, newCameraTargetPosition, ref cameraMovementSmoothingVelocity, cameraSettings.moveSmoothing);
       
       MoveCamera(newCameraPosition);
   }

   private void ManageCameraRotation()
   {
       
   }

   private void ManageCameraZoom()
   {
       float zoomCoefficent = ReturnZoomCoefficent(cameraActiveTargets);
       float newCameraZoom = Mathf.SmoothDamp(targetCamera.fieldOfView, zoomCoefficent, ref cameraZoomSmoothingVelocity, cameraSettings.zoomSmoothing);
       
       ZoomCamera(zoomCoefficent);
   }

   private void MoveCamera(Vector3 newCameraPosition)
   {
       transform.position = newCameraPosition;
   }

   private void RotateCamera(Quaternion newCameraRotation)
   {
       transform.rotation = newCameraRotation;
   }

   private void ZoomCamera(float newCameraZoom)
   {
       targetCamera.fieldOfView = newCameraZoom;
   }

   private Vector3 ReturnMedianPositionBetweenTargets(List<Transform> activeTargets)
   {
       if (activeTargets.Count > 0)
       {
           if (activeTargets.Count == 1)
           {
               return activeTargets[0].position;
           }
           
           Bounds centerBoundingPoint = new Bounds(activeTargets[0].position, Vector3.zero);

           for (int i = 0; i < activeTargets.Count; i++)
           {
               centerBoundingPoint.Encapsulate(activeTargets[i].position);
           }

           return centerBoundingPoint.center;
       }
       else
       {
           return cameraSettings.defaultPosition;
       }
   }

   private float ReturnGreatestDistanceBetweenTargets(List<Transform> activeTargets)
   {
       float maxDistance = 0f;
       
       if (activeTargets.Count == 0)
       {
           return 0f;
       }
       
       Bounds maxDistanceBounds = new Bounds(activeTargets[0].position, Vector3.zero);

       for (int i = 0; i < activeTargets.Count; i++)
       {
           maxDistanceBounds.Encapsulate(activeTargets[i].position);
       }

       maxDistance = maxDistanceBounds.size.x;

       return maxDistance;
   }

   private float ReturnZoomCoefficent(List<Transform> activeTargets)
   {
       float maxDistance = ReturnGreatestDistanceBetweenTargets(activeTargets);

       if (maxDistance < cameraSettings.minZoom)
       {
           return cameraSettings.minZoom;
       }
       else if (maxDistance > cameraSettings.maxZoom)
       {
           return cameraSettings.maxZoom;
       }
       else
       {
           return maxDistance;
       }
   }
   
   private List<Transform> ReturnAllActiveTargets()
   {
       List<Transform> newActiveTargets = new List<Transform>();
       
       GameObject[] foundTargets = GameObject.FindGameObjectsWithTag("Entity");

       for (int i = 0; i < foundTargets.Length; i++)
       {
           newActiveTargets.Add(foundTargets[i].transform);
       }

       return newActiveTargets;
   }

   public void AddTarget(Transform targetToAdd)
   {
       if (cameraActiveTargets.Contains(targetToAdd))
       {
           return;
       }
       
       cameraActiveTargets.Add(targetToAdd);
   }
   public void RemoveTarget(Transform targetToRemove)
   {
       if (cameraActiveTargets.Contains(targetToRemove))
       {
           cameraActiveTargets.Remove(targetToRemove);
       }
   }
   
   private void ClearActiveTargets()
   {
       cameraActiveTargets.Clear();
   }
}

[System.Serializable]
public struct CameraSettings
{
    [Header("Camera Follow Movement Settings")] 
    [Range(0, 10)]
    public float moveSmoothing;

    [Space(10)] [Range(0, 10)] 
    public float rotationalSmoothing;

    [Space(10)]
    public Vector3 defaultPosition;
    
    [Space(10)] 
    public Vector3 offset;
    
    [Header("Camera Follow Zoom Settings")] 
    public float minZoom;
    public float maxZoom;

    [Range(0, 10)]
    [Space(10)] 
    public float zoomSmoothing;
}
