using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heap { 
	private List<int> id; 
	private List<int> key; 
	private List<int> dist; 
	private List<int> l; 
	private List<int> r; 

	int nodeCount; 
	int root; 

	heap() {
		id = new List<int> (); 
		key = new List<int> (); 
		dist = new List<int> (); 
		l = new List<int> (); 
		r = new List<int> ();
		nodeCount = 0; 
		root = -1; 
	}
		
	public void Swap(ref int a, ref int b) {
		int tmp = b; 
		b = a; 
		a = b; 
	}

	int merge(int a, int b) {	
		if (a < 0 || b < 0)
			return a < 0 ? b : a;
		if (key[b] < key[a])
			Swap (ref a, ref b); 
		r [a] = merge (r [a], b);  
		if (l[a] < 0 || dist [l [a]] < dist [r [a]]) {
			int tmp = r [a]; 
			r [a] = l [a]; 
			l [a] = tmp; 
		}
		if (r [a] >= 0)
			dist [a] = dist [r [a]] + 1;
		else
			dist [a] = 0; 
		return a; 
	}

	void Insert(int _id, int _key) {
		id.Add (_id); 
		key.Add (_key);
		l.Add (-1); 
		r.Add (-1); 
		nodeCount++; 
		merge (root, nodeCount); 
	}
		
	void Pop() {
		root = merge (l[root], r[root]); 
	}

	int top_id() {
		return root < 0 ? -1 : id [root]; 
	}

	int top_key() {
		return root < 0 ? -1 : key [root]; 
	}
};

public class PathFinding : MonoBehaviour {

	void Start() {
	
	}
}
