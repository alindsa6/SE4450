using UnityEngine;
using System.Collections;

public class collisionDetect : MonoBehaviour {
	public Transform target;
	public Transform errorPoint;
	public Transform errorPoint2;
	public float dist;
	public float errorDist1;
	public float errorDist2;

	public bool mistake;
	// Use this for initialization
	void Start () {
		target = GameObject.Find ("tumor").transform;
		errorPoint=GameObject.Find ("default_MeshPart4").transform;
		errorPoint2 = GameObject.Find ("default_MeshPart5").transform;
	}
	
	
	// Update is called once per frame
	void Update () {
		dist = Vector3.Distance (transform.position, target.transform.position);
		errorDist1 = Vector3.Distance (transform.position, errorPoint.transform.position);
		errorDist2 = Vector3.Distance (transform.position, errorPoint2.transform.position);
		if (dist <= 1.0f) {
			Debug.Log("You hit the tumor, good job!");
		}


	}

	void success()
	{
		Debug.Log("Success");
	}
	void OnCollisionEnter (Collision col)
	{

	 dist = Vector3.Distance(transform.position, target.transform.position);

		if (col.gameObject.name == "default_MeshPart4" || col.gameObject.name == "default_MeshPart5") {
						Debug.Log ("you hit the brain, dumb ass keep trying");
		} 
	}
}
