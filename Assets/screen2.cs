using UnityEngine;
using System.Collections;

public class screen2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnCollisionEnter(Collision collision) {
		Debug.Log ("SCREEN2");
      Application.LoadLevel("Level2"); 
	}
}
