using UnityEngine;
using System.Collections;
using System.IO;

public class collisionDetect : MonoBehaviour {
	public Transform target;
	public Transform errorPoint;
	public Transform errorPoint2;
	public Transform tip;
	public float dist;
	public float errorDist1;
	public float errorDist2;
	public int numberofFaults;
	public int numberofSuccess;
	public float oldTime; 
	public float oldTimeSuc;
	public bool mistake;
	public float totalTime;
	public StreamWriter writer;
	// Use this for initialization
	void Start () {

		oldTime = 0.0f;
		totalTime = Time.realtimeSinceStartup;
		numberofFaults = 0;
		numberofSuccess = 0;
		mistake = false;
		target = GameObject.Find ("tumor").transform;
		errorPoint=GameObject.Find ("Cube1").transform;
		errorPoint2 = GameObject.Find ("Cube2").transform;
	}
	

	// Update is called once per frame
	void Update () {

		dist = Vector3.Distance (transform.position, target.transform.position);
		errorDist1 = Vector3.Distance (transform.position, errorPoint.transform.position);
		errorDist2 = Vector3.Distance (transform.position, errorPoint2.transform.position);
//		if (dist <= 1.0f) {
//			if (oldTimeSuc - (System.DateTime.UtcNow).Millisecond < 80){
//				oldTimeSuc = (System.DateTime.UtcNow).Millisecond;
//				Debug.Log("You hit the tumor, good job!");
//			mistake = !mistake;
//				numberofSuccess++;
//
//			}
//		}

//		if (errorDist1 <= 1.0f) {
//			mistake = true;
//			Debug.Log ("Bad Bad! try again >:(");
//
//		}
	}



	void OnCollisionEnter (Collision col)
	{

		Debug.Log (oldTime - (System.DateTime.UtcNow).Millisecond);
		if (oldTime - (System.DateTime.UtcNow).Millisecond < 350) {

	
						if (col.gameObject.name == "Cube1" || col.gameObject.name == "Cube2") {
				oldTime = (System.DateTime.UtcNow).Millisecond;
								numberofFaults++;
								Debug.Log ("Bad Bad! try again >:(");
				StreamWriter writer = new StreamWriter (@"C:\Users\Omar\Documents\stats.txt");
				writer.Write (numberofFaults.ToString() + " : " + numberofSuccess.ToString());
				writer.Close ();
		
			} 

			if (col.gameObject.name == "actualtumor") {
				oldTime = (System.DateTime.UtcNow).Millisecond;
				numberofSuccess++;
				Debug.Log ("good job");
				StreamWriter writer = new StreamWriter (@"C:\Users\Omar\Documents\stats.txt");
				writer.Write (numberofFaults.ToString() + " : " + numberofSuccess.ToString());
				writer.Close ();
				
			} 
			
		}
	}
}
