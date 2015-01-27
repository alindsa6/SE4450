using UnityEngine;
using System.Collections;

public class collisionDetect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name == "default_MeshPart4" || col.gameObject.name == "default_MeshPart5") {
						Debug.Log ("you hit the brain, dumb ass keep trying");
		} else if (col.gameObject.name == "Sphere") {
			Debug.Log ("good job! you hit the tumor");

				}
	}
}
