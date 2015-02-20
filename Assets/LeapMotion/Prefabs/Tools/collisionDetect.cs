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
	public System.DateTime wat;
	public System.TimeSpan dif;
	public System.DateTime start;
	public System.TimeSpan elapsed;
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
		wat = System.DateTime.Now;

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
		dif = System.DateTime.Now - wat;
		elapsed = System.DateTime.Now - start;
		Debug.Log ("here" + dif.TotalMilliseconds/1000);


		Debug.Log (oldTime - (System.DateTime.UtcNow).Millisecond);
		if (dif.TotalMilliseconds/1000 >= 2.0f) {

	
						if (col.gameObject.name == "Cube1" || col.gameObject.name == "Cube2") {
								Debug.Log ("Bad Bad! try again >:(");
				numberofFaults++;
				//StreamWriter writer = new StreamWriter (@"C:\Users\Omar\Documents\stats.txt");

				string progress = numberofFaults.ToString() + " : " + numberofSuccess.ToString() + " @ time "+ elapsed.Minutes+"\r\n";
				File.AppendAllText(@"C:\Users\Omar\Documents\stats.txt",progress);
//				writer.WriteLine ("\r\n"+numberofFaults.ToString() + " : " + numberofSuccess.ToString() + " @ time "+ elapsed.TotalMinutes+"\r\n");
//				writer.Close ();
			} 

			if (col.gameObject.name == "actualtumor") {
				Debug.Log ("good job");
				numberofSuccess++;
				//StreamWriter writer = new StreamWriter (@"C:\Users\Omar\Documents\stats.txt");
//				writer.WriteLine ( "\r\n "+ numberofFaults.ToString() + " : " + numberofSuccess.ToString()+" @ time"+ elapsed.TotalMinutes+"\r\n");
//				writer.Close ();
				
			} 
			wat = System.DateTime.Now;
			
		}
	}
}
