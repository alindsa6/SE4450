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
   private GameObject tvLookPosition;

   private GameObject current;


   private Action moveCamera;
   private GameObject zoomedCamera;
   private Vector3 targetPosition;
   private Vector3 targetLookPosition;

   private CameraPosition currentPosition;
   enum CameraPosition { brainView, trayView, tvView }

   private Timer brainBeatTimer = new Timer(1050);
   private Timer brainBeatUpTimer = new Timer(300);
   private volatile bool pulseUp = false;
   private volatile bool pulseDown = false;

   public toolChanger()
   {
      brainBeatTimer.Elapsed += brainBeatTimer_Elapsed;
      brainBeatTimer.Start();
      brainBeatUpTimer.AutoReset = false;
      brainBeatUpTimer.Elapsed += brainBeatUpTimer_Elapsed;
   }


   private void brainBeatTimer_Elapsed(object sender, ElapsedEventArgs e)
   {
      pulseUp = true;
   }
   private void brainBeatUpTimer_Elapsed(object sender, ElapsedEventArgs e)
   {
      pulseDown = true;
   }

   void OnDestroy()
   {
      brainBeatTimer.Dispose();
      brainBeatTimer = null;
      brainBeatUpTimer.Dispose();
      brainBeatUpTimer = null;
      Debug.Log("Destroyed");
   }

   void Start()
   {
      zoomedCamera = GameObject.Find("Camera");
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
      currentPosition = CameraPosition.brainView;
   }

   public void OnConnect(Controller controller)
   {
      controller.EnableGesture(Gesture.GestureType.TYPECIRCLE, true);
   }
   // Update is called once per frame
   void Update()
   {
      // Hide the zoomed camera if we're looking at the tv
      zoomedCamera.SetActive(currentPosition != CameraPosition.tvView);

      // Give the brain a pulsing heart beat
      if (pulseUp)
      {
         Vector3 target = operatingLookPosition.transform.position - new Vector3(0f, 0f, 0.2f);
         operatingLookPosition.transform.position = Vector3.MoveTowards(operatingLookPosition.transform.position, target, 0.005f);
         brainBeatUpTimer.Start();
         pulseUp = false;
      }
      if (pulseDown)
      {
         Vector3 target = operatingLookPosition.transform.position + new Vector3(0f, 0f, 0.2f);
         operatingLookPosition.transform.position = Vector3.MoveTowards(operatingLookPosition.transform.position, target, 0.005f);
         pulseDown = false;
      }

      Frame frame = controller.Frame();
      foreach (Gesture gesture in frame.Gestures())
      {
         switch (gesture.Type)
         {
            case (Gesture.GestureType.TYPECIRCLE):
               {
                  // CIRCLE gesture keeps getting triggered even without an actual circle gesture.
                  // Don't use this gesture for now.
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

                  bool swipedLeft = swipDirection.x < 0;
                  bool swipedRight = !swipedLeft;

                  if ((currentPosition == CameraPosition.brainView && swipedLeft) // At brain and swiped left
                     || (currentPosition == CameraPosition.tvView && swipedRight))// At tv and swiped right
                  {
                     targetPosition = toolsTrayViewPosition.transform.position;
                     targetLookPosition = toolsTrayLookPosition.transform.position;
                     currentPosition = CameraPosition.trayView;
                  }
                  else if (currentPosition == CameraPosition.trayView)
                  {
                     if (swipedLeft)
                     {
                        targetPosition = tvViewPosition.transform.position;
                        targetLookPosition = tvLookPosition.transform.position;
                        currentPosition = CameraPosition.tvView;
                     }
                     else if (swipedRight)
                     {
                        targetPosition = operatingViewPosition.transform.position;
                        targetLookPosition = operatingLookPosition.transform.position;
                        currentPosition = CameraPosition.brainView;
                     }
                  }
                  else
                  {
                     // invalid swipe at current location
                     cameraIsMoving = false;
                     return;
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
