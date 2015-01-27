using UnityEngine;
using System.Collections;
using Leap;

public class NewBehaviourScript : MonoBehaviour {

	//declaring the camera to rotate and controller and some other boolean 
	public Transform camera;
	Controller controller;
	public bool turningRight;
	public bool turningLeft;

	public HandController hc;

		// Initializing here
	void Start () {
		//access the actual controller to get inputs
		//accessing controller class
		hc = new HandController ();
		controller = new Controller (); // creating controller object
		controller.EnableGesture (Gesture.GestureType.TYPESWIPE); // accessing gesture class
		controller.EnableGesture (Gesture.GestureType.TYPECIRCLE); // accessing gesture class
		controller.Config.SetFloat("Gesture.Swipe.MinLength",200.0f); //initiallizing gesture min val
		controller.Config.SetFloat("Gesture.Swipe.MinVelocity",750f); //initiallizing gesture max val
		controller.Config.Save (); //save it
		turningRight = false; //initiallize the turning methods(might delete those in the future)
		turningLeft = false; //ya..this one too lol
	}
	
	// Update is called once per frame
	void Update () {
		//capture the frames of the controller
		Frame frame = controller.Frame ();

		GestureList gestures = frame.Gestures (); //get their gestures

		//go through the list and get the swiping one
		for(int i=0; i<gestures.Count;i++)
		{
			Gesture gesture = gestures[i];
			//if you found it go in this if statement
			if(gesture.Type ==  Gesture.GestureType.TYPESWIPE)
			{
				//get the gesture and assign it to an object to analyze its values
				SwipeGesture swipe = new SwipeGesture(gesture);
				Vector  swipDirection = swipe.Direction;

//				if(swipDirection.x<0 && !rotating) rotating = true;
//				
//				if(rotating && camera.transform.eulerAngles.y <= angle)
//				{
//					Debug.Log ("I am here");
//					camera.transform.Rotate(Vector3.up * (speed * Time.deltaTime));
//				}
//				else if (rotating) rotating = false;

				//If its x value is less than 0 then we must be swiping left
				if(swipDirection.x<0 )
				{
					//rotate the camera
					//hc.toolModel = tool1;
					rotate();
				}
				if(swipDirection.x>0 )
				{
					//rotate the camera
					//hc.toolModel = tool1;
					rotateOtherWay();
				}

//				if(turningLeft && camera.transform.rotation.y>=90)
//				{
//					transform.rotation =  Quaternion.Euler(0,90,0);
//					turningLeft = false;
//				}
//	
//				else if(swipDirection.x>0)
//				{
//					camera.transform.Rotate(Time.deltaTime*speed, -90, 0);
//
//				}
			}



		}


		
	}

	void rotateOtherWay()
	{
		//some shit here I found on the internet to rotate a unity object
		Quaternion newRotation = new Quaternion(camera.transform.rotation.x,camera.transform.rotation.y,camera.transform.rotation.z,camera.transform.rotation.w);;
		
		//next time we swip left we need to know where to go from our last rotation
		newRotation *= Quaternion.Euler(0, 90, 0); // this add a -90 degrees Y rotation
		
		//2*time.delta says to rotate 2 degrees every second
		transform.rotation= Quaternion.Slerp(camera.transform.rotation, newRotation, 8* Time.deltaTime);      
	}

	void rotate()
	{
		//some shit here I found on the internet to rotate a unity object
		Quaternion newRotation = new Quaternion(camera.transform.rotation.x,camera.transform.rotation.y,camera.transform.rotation.z,camera.transform.rotation.w);;

		//next time we swip left we need to know where to go from our last rotation
		newRotation *= Quaternion.Euler(0, -90, 0); // this add a -90 degrees Y rotation

		//2*time.delta says to rotate 2 degrees every second
		transform.rotation= Quaternion.Slerp(camera.transform.rotation, newRotation,  Time.deltaTime);      
	}
}