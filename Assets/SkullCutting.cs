using UnityEngine;
using System.Collections;

public class SkullCutting : MonoBehaviour {

   public GameObject topLeft;
   public GameObject topRight;
   public GameObject lowerLeft;
   public GameObject LowerRight;
   public GameObject skull;

   private bool didStart = false;

   private bool hit1 = false;
   private bool hit2 = false;
   private bool hit3 = false;
   private bool hit4 = false;


	// Use this for initialization
	void Start () 
   {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   //void OnTriggerEnter(Collider other)
   void OnCollisionEnter(Collision col)
   {
      if (col.gameObject == topRight)
      {
         Debug.Log("Top right");
         hit1 = true;
         didStart = true;
			if(!audio.isPlaying)
			audio.Play();
      }
      if (col.gameObject == topLeft)
      {
         Debug.Log("Top LEft");
         hit2 = true;
         hit1 = false;
			if(!audio.isPlaying)
				audio.Play();
      }
      if (col.gameObject == lowerLeft)
      {
         Debug.Log("lower LEft");
         hit3 = true;
			if(!audio.isPlaying)
				audio.Play();      }
      if (col.gameObject == LowerRight)
      {
         Debug.Log("lower right");
         hit4 = true;
			if(!audio.isPlaying)
				audio.Play();    
		}

      if (hit1 && hit2 && hit3 && hit4 && didStart)
      {
			this.collider.enabled = false;
			skull.collider.enabled = true;
			skull.rigidbody.constraints = RigidbodyConstraints.None;
         Debug.Log("Done");
         skull.rigidbody.useGravity = true;
      }
   }
}
