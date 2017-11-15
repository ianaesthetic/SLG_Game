using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpot : MonoBehaviour {

	public int ID; 
	public List<int> nextSpot; 
	public List<float> nextSpotDist; 

	// Use this for initialization
	void Start () {
		//nextSpot = new List<int> (); 
		//nextSpotDist = new List<float> (); 
	}
		
	public void SetID(int x) {
		ID = x; 
	}

	public void SetArray(int dest, float dist) {
		nextSpot.Add (dest); 
		nextSpotDist.Add (dist);
	}

	void Update() {
	}
}
