using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;


public class collisionDetect : MonoBehaviour
{
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
   public string ratio;
   public string tims;
   public string ra;
   public string ti;
   public int counter;
   // Use this for initialization

   void Start()
   {


      ratio = "0";
      tims = "0";
      oldTime = 0.0f;
      totalTime = Time.realtimeSinceStartup;
      numberofFaults = 1;
      numberofSuccess = 1;
      mistake = false;
      target = GameObject.Find("tumor").transform;
      errorPoint=GameObject.Find("Cube1").transform;
      errorPoint2 = GameObject.Find("Cube2").transform;
      wat = System.DateTime.Now;
      counter = 0;


   }


   // Update is called once per frame
   void Update()
   {
      dist = Vector3.Distance(transform.position, target.transform.position);
      errorDist1 = Vector3.Distance(transform.position, errorPoint.transform.position);
      errorDist2 = Vector3.Distance(transform.position, errorPoint2.transform.position);
   }

   void sendData()
   {
      this.ra = ratio.ToString();
      this.ti = tims.ToString();
      using (WebClient client = new WebClient())
      {
         Debug.Log("last time " + this.ratio.ToString() + "\nSending data to server");
         byte[] response =
            client.UploadValues("http://localhost:8080/leapsurg/login", new NameValueCollection()
                                {
                                   { "rats", this.ratio.ToString() },
                                   { "tims", this.tims.ToString()}
                                });

         string result = System.Text.Encoding.UTF8.GetString(response);
         Debug.Log(result.ToString());
         counter = 0;
      }
   }


   void OnCollisionEnter(Collision col)
   {
      dif = System.DateTime.Now - wat;
      elapsed = System.DateTime.Now - start;

      if (dif.TotalMilliseconds/1000 >= 2.0f)
      {
         numberofFaults++;
         counter++;
         int numCollisionsBeforeSend = 3;

         this.ratio = (this.ratio.ToString()+ ", "+(this.numberofFaults/this.numberofSuccess).ToString());
         this.tims = (this.tims.ToString()+", "+this.elapsed.Seconds.ToString());
         if (col.gameObject.name == "Cube1" || col.gameObject.name == "Cube2")
         {

            this.ratio = (ratio.ToString()+ ", "+(numberofFaults/numberofSuccess).ToString());
            Debug.Log(ratio);
            this.tims = (tims.ToString()+", "+elapsed.Seconds.ToString());
            Debug.Log(tims);
            string progress = (numberofFaults/numberofSuccess).ToString() +  " @ time "+ elapsed.Minutes + " " + ra+"\r\n";

            if (counter == numCollisionsBeforeSend)
            {
               Thread sendThread = new Thread(sendData);
               sendThread.Start();
            }
         }

         if (col.gameObject.name == "actualtumor")
         {
            numberofSuccess++;
            counter++;
            this.ratio = (this.ratio.ToString()+ ", "+(this.numberofFaults/this.numberofSuccess).ToString());
            Debug.Log(ratio);
            this.tims = (this.tims.ToString()+", "+this.elapsed.Seconds.ToString());
            Debug.Log(tims);
            if (counter == numCollisionsBeforeSend)
            {
               Thread sendThread = new Thread(sendData);
               sendThread.Start();
            }
         }
         wat = System.DateTime.Now;

      }
   }
}
