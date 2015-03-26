using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;

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
	public string    ratio;
	public string   tims;
	public string ra;
	public string ti;
	public GameObject blood;
	public int counter;
	public Vector3 head;
	public string heading;
	public string y;
	public string z;

	// Use this for initialization

	void Start () {

		heading = "0";
		y = "0";
		z = "0";
		ratio = "0";
		tims = "0";
		oldTime = 0.0f;
		totalTime = Time.realtimeSinceStartup;
		numberofFaults = 1;
		numberofSuccess = 1;
		mistake = false;
		target = GameObject.Find ("tumor").transform;
		errorPoint=GameObject.Find ("Cube1").transform;
		errorPoint2 = GameObject.Find ("Cube2").transform;
		wat = System.DateTime.Now;
		counter = 0;

		
	}
	

	// Update is called once per frame
	void Update () {



		//InvokeRepeating("sendData", 40, 0);

		head = target.position - this.transform.position;
		head.z = 0;
		heading = heading + ", " + head.sqrMagnitude;

		y = y + ", " + (target.position.z - this.transform.position.z);


		z = z + ", " + Time.time;

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

	void sendData()
	{
		this.ra = ratio.ToString ();
		this.ti = tims.ToString ();

	



		using (WebClient client = new WebClient())
		{
			Debug.Log("last time " + this.ratio.ToString());

			
			byte[] response =
				client.UploadValues("http://localhost:8080/leapsurg/login", new NameValueCollection()
				                    {
					{ "rats", this.ratio.ToString() },
					{ "tims", this.tims.ToString()},
					{ "heading", this.heading.ToString() },
					{ "y", this.y.ToString() },
					{ "z", this.z.ToString() }
				});
			
			string result = System.Text.Encoding.UTF8.GetString(response);
			Debug.Log (result.ToString());

			counter = 0 ; 
		}
		
	}
	
	
	void OnCollisionEnter (Collision col)
	{

	
		dif = System.DateTime.Now - wat;
		elapsed = System.DateTime.Now - start;
		if (dif.TotalMilliseconds/1000 >= 2.0f) {
			numberofFaults++;
			counter++;
			foreach (ContactPoint contact in col.contacts) {
				Instantiate (blood.transform, contact.point, Quaternion.LookRotation(contact.normal));
			}
			this.ratio = (this.ratio.ToString()+ ", "+(this.numberofFaults/this.numberofSuccess).ToString());
			this.tims = (this.tims.ToString()+", "+this.elapsed.Seconds.ToString());
						if (col.gameObject.name == "Cube1" || col.gameObject.name == "Cube2") {
				
				this.ratio = (ratio.ToString()+ ", "+(numberofFaults/numberofSuccess).ToString());
				Debug.Log(ratio);
				this.tims = (tims.ToString()+", "+elapsed.Seconds.ToString());
				Debug.Log(tims);
				//StreamWriter writer = new StreamWriter (@"C:\Users\Omar\Documents\stats.txt");

				string progress = (numberofFaults/numberofSuccess).ToString() +  " @ time "+ elapsed.Minutes + " " + ra+"\r\n";
				File.AppendAllText(@"C:\Users\Omar\Documents\stats.txt",progress);
//				writer.WriteLine ("\r\n"+numberofFaults.ToString() + " : " + numberofSuccess.ToString() + " @ time "+ elapsed.TotalMinutes+"\r\n");
//				writer.Close ();
				if(counter == 5)
				{
					Thread sendThread = new Thread(sendData);
					sendThread.Start();
				}
			} 

			if (col.gameObject.name == "actualtumor") {
				numberofSuccess++;
				counter++;
				Debug.Log ("SCUCCES");
				target.renderer.material.color =  Color.Lerp(Color.yellow, Color.red, Time.time);

				this.ratio = (this.ratio.ToString()+ ", "+(this.numberofFaults/this.numberofSuccess).ToString());
				Debug.Log(ratio);
				this.tims = (this.tims.ToString()+", "+this.elapsed.Seconds.ToString());
				Debug.Log(tims);

				//ratio.Append(numberofFaults/numberofSuccess+", ");
				//tims.Append (elapsed.Seconds+", ");
				//StreamWriter writer = new StreamWriter (@"C:\Users\Omar\Documents\stats.txt");
//				writer.WriteLine ( "\r\n "+ numberofFaults.ToString() + " : " + numberofSuccess.ToString()+" @ time"+ elapsed.TotalMinutes+"\r\n");
//				writer.Close ();
				if(counter == 5)
				{
					sendData();
				}
			} 
			wat = System.DateTime.Now;
			
		}
	}
}
