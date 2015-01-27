using UnityEngine;
using System.Collections;
using Leap;
public class toolChanger : MonoBehaviour {
	Leap.Controller controller;	// Use this for initialization
	public HandController hc;
	public ToolModel tool1;
	public ToolModel tool2;
	public ToolList tools = new ToolList();
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
				Debug.Log("Swipe gesture recognized.");
				hc.toolModel = tool2;

				break;
			}
			default:
			{
				break;
			}
			}
		}
	}

	public  void OnFrame(Controller controller)
	{
		Frame frame = controller.Frame();
		GestureList gestures = frame.Gestures();
		for (int i = 0; i < gestures.Count; i++)
		{
			Gesture gesture = gestures[0];
			switch(gesture.Type){
			case Gesture.GestureType.TYPECIRCLE:
				Debug.Log("Circle Detected");
				break;
			default:
				Debug.Log("Bad gesture type");
				break;
			}
		}
	}
}
