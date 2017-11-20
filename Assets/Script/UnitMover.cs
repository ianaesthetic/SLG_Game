using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour {
	public int curPos;
	public List<int> nxtPos;
	private bool isGoing; 

	void Start() {
		List<int> nxtPos = new List<int> (); 	
		isGoing = false; 
	}
}
