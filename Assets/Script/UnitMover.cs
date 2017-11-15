using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour {

	public struct direction {
		float moveX;
		float moveY; 
		float moveZ; 
	};

	public int dest;
	private int curDest; 

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Destroy (); 
	}
			
	void Destroy() {
		if (curDest == dest) {
			Destroy (gameObject); 
		}
	}
}
