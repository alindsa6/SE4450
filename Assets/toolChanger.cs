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
   private volatile bool canSwipe = true;

   private GameObject toolsTray;
   private GameObject operatingView;
   private GameObject current;

   private Action moveCamera;
   private Vector3 targetPosition;

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
      toolsTray = GameObject.Find("ToolsTrayPosition");
      operatingView = GameObject.Find("OperatingPosition");
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
                  Debug.Log("Circle gesture recognized.");

                  hc.toolModel = tool1;
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
                  hc.toolModel = tool2;

                  break;
               }
            case (Gesture.GestureType.TYPESCREENTAP):
               {
                  Debug.Log("Screen tap gesture recognized.");
                  hc.toolModel = tool2;

                  break;
               }
            case (Gesture.GestureType.TYPESWIPE):
               {
                  Vector swipDirection = new SwipeGesture(gesture).Direction;

                  if (!canSwipe)
                  {
                     return;
                  }

                  canSwipe = false;
                  Debug.Log("Swipe detected.");

                  if (swipDirection.x<0)
                  {
                     targetPosition = toolsTray.transform.position;
                     moveCamera = () =>
                        {
                           current.transform.position = Vector3.MoveTowards(current.transform.position, targetPosition, 0.1f);
                        };
                     transform.RotateAround(camera.position, Vector3.right, 20f);
                  }
                  if (swipDirection.x>0)
                  {
                     targetPosition = operatingView.transform.position;
                     moveCamera = () =>
                     {
                        current.transform.position = Vector3.MoveTowards(current.transform.position, targetPosition, 0.1f);
                     };
                     transform.RotateAround(camera.position, Vector3.right, -20f);
                  }
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
         if (current.transform.position == targetPosition)
         {
            canSwipe = true;
            moveCamera = null;
         }
      }
   }
}
