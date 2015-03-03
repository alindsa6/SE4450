using UnityEngine;
using System.Collections;
using Leap;
using System.Timers;

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
   private Timer swipeTimer;

   public toolChanger()
   {
      swipeTimer = new Timer(1000); // Can swipe every 1 second
      swipeTimer.Elapsed += swipeTimerElapsed;
      swipeTimer.AutoReset = false;
   }

   private void swipeTimerElapsed(object sender, ElapsedEventArgs e)
   {
      Debug.Log("Timer Elapsed. You can swipe again.");
      canSwipe = true;
   }

   void Start()
   {
      controller = new Controller();
      controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
      controller.EnableGesture(Gesture.GestureType.TYPEINVALID);
      controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
      controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
      controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
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
                  swipeTimer.Start();
                  Debug.Log("Swipe Detected");
                  if (swipDirection.x<0)
                  {
                     transform.RotateAround(camera.position, Vector3.up, -45f);
                  }
                  if (swipDirection.x>0)
                  {
                     transform.RotateAround(camera.position, Vector3.up, 45f);
                  }
                  break;
               }
            default:
               {
                  break;
               }
         }
      }
   }

   void OnDestroy()
   {
      Debug.Log("Destroyed toolChanger.cs!");
      swipeTimer.Dispose();
      swipeTimer = null;
   }   
}
