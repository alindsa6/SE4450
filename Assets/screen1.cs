using UnityEngine;
using System.Collections;

public class screen1 : MonoBehaviour {
	public GameObject x;
	// Use this for initialization
	void Start () {
		x.SetActive (false);

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnCollisionEnter(Collision collision) {
		Debug.Log ("SCREEN1");
		Application.LoadLevel ("example_scene"); 
	}
}
