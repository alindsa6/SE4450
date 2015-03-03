using UnityEngine;
using System.Collections;
using Leap;
public class toolChanger : MonoBehaviour {
	Leap.Controller controller;	// Use this for initialization
	public HandController hc;
	public ToolModel tool1;
	public ToolModel tool2;
	public ToolList tools = new ToolList();
	public Transform camera;
	public float smooth = 2.0F;
	public float tiltAngle = 30.0F;
	void Start () 
	{
		controller = new Controller();
		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
		controller.EnableGesture(Gesture.GestureType.TYPEINVALID);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
	}
	public  void OnConnect(Controller controller){
		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE,true);
	}
	// Update is called once per frame
	void Update () 
	{
		Frame frame = controller.Frame();
		foreach (Gesture gesture in frame.Gestures())
		{
			switch(gesture.Type)
			{
			case(Gesture.GestureType.TYPECIRCLE):
			{
				Debug.Log("Circle gesture recognized.");

				hc.toolModel = tool1;
				break;
			}
			case(Gesture.GestureType.TYPEINVALID):
			{
				Debug.Log("Invalid gesture recognized.");
				break;
			}
			case(Gesture.GestureType.TYPEKEYTAP):
			{
				Debug.Log("Key Tap gesture recognized.");
				hc.toolModel = tool2;

				break;
			}
			case(Gesture.GestureType.TYPESCREENTAP):
			{
				Debug.Log("Screen tap gesture recognized.");
				hc.toolModel = tool2;

				break;
			}
			case(Gesture.GestureType.TYPESWIPE):
			{

//				SwipeGesture swipe = new SwipeGesture(gesture);
//				Vector  swipDirection = swipe.Direction;
//				if(swipDirection.x<0 )
//				{
//				
//					//some shit here I found on the internet to rotate a unity object
//					Quaternion newRotation = new Quaternion(camera.transform.rotation.x,camera.transform.rotation.y,camera.transform.rotation.z,camera.transform.rotation.w);;
//					
//					//next time we swip left we need to know where to go from our last rotation
//					newRotation *= Quaternion.Euler(0, -100, 0); // this add a -90 degrees Y rotation
//					
//					//2*time.delta says to rotate 2 degrees every second
//					transform.rotation= Quaternion.Slerp(camera.transform.rotation, newRotation,  Time.deltaTime); 
//
//					}
//				if(swipDirection.x>0 )
//				{
//					//some shit here I found on the internet to rotate a unity object
//					Quaternion newRotation = new Quaternion(camera.transform.rotation.x,camera.transform.rotation.y,camera.transform.rotation.z,camera.transform.rotation.w);;
//					
//					//next time we swip left we need to know where to go from our last rotation
//					newRotation *= Quaternion.Euler(0, 100, 0); // this add a -90 degrees Y rotation
//					
//					//2*time.delta says to rotate 2 degrees every second
//					transform.rotation= Quaternion.Slerp(camera.transform.rotation, newRotation, 8* Time.deltaTime); 
//
//
//					}
				
				//				Debug.Log("Swipe gesture recognized.");
				//				hc.toolModel = tool2;
				//
							break;
			}

			default:
			{
				break;
			}
			}
		}
	}

//	public  void OnFrame(Controller controller)
//	{
//		Frame frame = controller.Frame();
//		GestureList gestures = frame.Gestures();
//		for (int i = 0; i < gestures.Count; i++)
//		{
//			Gesture gesture = gestures[0];
//			switch(gesture.Type){
//			case Gesture.GestureType.TYPECIRCLE:
//				Debug.Log("Circle Detected");
//				break;
//			default:
//				Debug.Log("Bad gesture type");
//				break;
//			}
//		}
//	}
}
