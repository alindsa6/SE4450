using UnityEngine;
using System.Collections;
using Leap;
using System.Timers;
using System;
//using System.Threading;
public class toolChanger : MonoBehaviour
{
   Leap.Controller controller;	// Use this for initialization
   public HandController hc;
   public ToolModel tool1;
   public ToolModel tool2;
   public ToolList tools = new ToolList();
   public Transform camera;
   public float smooth = 2.0F;
   public float tiltAngle = 30.0F;
   private volatile bool cameraIsMoving = false;

   private GameObject toolsTrayViewPosition;
   private GameObject toolsTrayLookPosition;
   private GameObject operatingViewPosition;
   private GameObject operatingLookPosition;
   private GameObject tvViewPosition;
   private GameObject current;

   private GameObject tvLookPosition;

   private Action moveCamera;
   private Vector3 targetPosition;
   private Vector3 targetLookPosition;
   private Vector3 targetAngle;

   public toolChanger()
   {
   }

   void Start()
   {
      controller = new Controller();
      controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
      controller.EnableGesture(Gesture.GestureType.TYPEINVALID);
      controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
      controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
      controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
      toolsTrayViewPosition = GameObject.Find("ToolsTrayPosition");
      toolsTrayLookPosition = GameObject.Find("ToolsTrayLookPosition");
      operatingViewPosition = GameObject.Find("OperatingPosition");
      operatingLookPosition = GameObject.Find("Brain_Model");
      tvViewPosition = GameObject.Find("TvPosition");
      tvLookPosition = GameObject.Find("TvLookPosition");
      current = GameObject.Find("Main Camera");

   }
   public void OnConnect(Controller controller)
   {
      controller.EnableGesture(Gesture.GestureType.TYPECIRCLE, true);
   }
   // Update is called once per frame
   void Update()
   {
      Frame frame = controller.Frame();
      foreach (Gesture gesture in frame.Gestures())
      {
         switch (gesture.Type)
         {
            case (Gesture.GestureType.TYPECIRCLE):
               {
                  if (cameraIsMoving)
                  {
                     return;
                  }
                  cameraIsMoving = true;

                  targetPosition = tvViewPosition.transform.position;
                  targetLookPosition = tvLookPosition.transform.position;
                  moveCamera = () =>
                  {
                     current.transform.position = Vector3.MoveTowards(current.transform.position, targetPosition, 0.1f);
                     
                     Vector3 targetDir = tvLookPosition.transform.position - current.transform.position;
                     Vector3 newDir = Vector3.RotateTowards(current.transform.forward, targetDir, 0.02f, 1f);
                     current.transform.rotation = Quaternion.LookRotation(newDir);
                  };
                  break;
               }
            case (Gesture.GestureType.TYPEINVALID):
               {
                  Debug.Log("Invalid gesture recognized.");
                  break;
               }
            case (Gesture.GestureType.TYPEKEYTAP):
               {
                  Debug.Log("Key Tap gesture recognized.");
                  break;
               }
            case (Gesture.GestureType.TYPESCREENTAP):
               {
                  Debug.Log("Screen tap gesture recognized.");
                  break;
               }
            case (Gesture.GestureType.TYPESWIPE):
               {
                  Vector swipDirection = new SwipeGesture(gesture).Direction;

                  if (cameraIsMoving)
                  {
                     return;
                  }

                  cameraIsMoving = true;
                  Debug.Log("Swipe detected.");

                  if (swipDirection.x<0)
                  {
                     targetPosition = toolsTrayViewPosition.transform.position;
                     //targetAngle = new Vector3(30f, 0f, 0f); // optimum angle for viewing the tools tray
                     targetLookPosition = toolsTrayLookPosition.transform.position;
                  }
                  else if (swipDirection.x>0)
                  {
                     targetPosition = operatingViewPosition.transform.position;
                     //targetAngle = new Vector3(10f, 0f, 0f); // optimum angle for viewing the brain
                     targetLookPosition = operatingLookPosition.transform.position;
                  }

                  moveCamera = () =>
                  {
                     current.transform.position = Vector3.MoveTowards(current.transform.position, targetPosition, 0.1f);

                     Vector3 targetDir = targetLookPosition - current.transform.position;
                     Vector3 newDir = Vector3.RotateTowards(current.transform.forward, targetDir, 0.02f, 1f);
                     current.transform.rotation = Quaternion.LookRotation(newDir);
                  };
                  break;
               }

            default:
               {
                  break;
               }
         }
      }

      if (moveCamera != null)
      {
         moveCamera.Invoke();

         // Vectors representing the look direction
         Vector3 forward = current.transform.forward;
         Vector3 target = targetLookPosition - current.transform.position;

         // Calculate if the camera is done moving
         bool positionIsDone = current.transform.position == targetPosition;
         bool lookIsDone = Math.Abs(Vector3.Angle(forward, target)) < Mathf.Epsilon;
         if (positionIsDone && lookIsDone)
         {
            Debug.Log("End of camera movement");
            // The camera has reached its final position and angle.
            // It is no longer moving
            cameraIsMoving = false;
            moveCamera = null;
         }
      }
   }
}
