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

	public void Clear() {
		id.Clear (); 
		key.Clear (); 
		dist.Clear (); 
		l.Clear (); 
		r.Clear (); 
		nodeCount = 0; 
		root = -1; 
	}
		
	public void Swap(ref int a, ref int b) {
		int tmp = b; 
		b = a; 
		a = b; 
	}

	private int merge(int a, int b) {	
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

	public void Insert(int _id, int _key) {
		id.Add (_id); 
		key.Add (_key);
		l.Add (-1); 
		r.Add (-1); 
		nodeCount++; 
		merge (root, nodeCount); 
	}
		
	public void Pop() {
		root = merge (l[root], r[root]); 
	}

	public int TopID() {
		return root < 0 ? -1 : id [root]; 
	}

	public int TopKey() {
		return root < 0 ? -1 : key [root]; 
	}
};
	
public class map {
	public struct node {
		public int id;	
		public int dist; 

		public node(int _id, int _dist) {
			id = _id; 
			dist = _dist;
		}
	};	
		
	List<node> nodeMem;

	public void Clear() {
		nodeMem.Clear (); 
	}

	public bool Find(int id) {
		for (int i = 0; i < nodeMem.Count; ++i) {
			if (nodeMem [i].id == id)
				return true; 
		}
		return false; 
	}
		
	public void Insert(int id, int dist) {
		nodeMem.Add (new node(id, dist));
	}
		
	public void Delete(int id) { 
		for (int i = 0; i < nodeMem.Count; ++i)
			if (nodeMem [i].id == id) {
				nodeMem.RemoveAt (i); 
				break;
			}
	}

};

public class PathFinding : MonoBehaviour {

	private struct pair{
		public float h; 
		public float w; 

		pair(float _h, float _w) {
			h = _h; 
			w = _w; 
		}
	};

	public float lB, rB, uB, dB; 
	public int nB;
	private float wB, hB, hStart, wStart, scaleFactor, interval;
	public GameObject hexagon; 

	private heap unCover; 
	private map covered; 
	private pair[] direction; 

	public void GetPos(out float posH, out float posW, int id) {
		int tmp; 
		tmp = (int)(id / nB); 
		posH = (float)tmp * interval + dB;
		tmp = id % nB; 
		posW = (float)tmp * interval + lB;
	}

	public void GetID(float posH, float posW, out int id) {
		if (posH < hStart || posH > hStart + hB || posW < wStart || posW > wStart + wB) {
			id = -1; 
			return; 
		}

	}

	void Start() {

		float ratio = Mathf.Sqrt (3); 
		scaleFactor = 0.9f;


		wB = rB - lB;
		hB = wB - dB;

		if (hB * ratio < wB) {
			hB *= scaleFactor; 
			wB = hB * ratio; 
		}
		else {
			wB *= scaleFactor; 
			hB = wB / ratio; 
		}

		interval = hB / (float)nB;
		hStart = dB + (wB - dB - hB) / 2.0f; 
		wStart = lB + (rB - lB - wB) / 2.0f; 
	
		for (int i = 0; i < nB; ++i)
			for (int j = 0; j < nB; ++j) {
				GameObject tObject = Instantiate (hexagon, new Vector3 (i * interval + hStart, j * interval + wStart), Quaternion.identity) as GameObject;
				tObject.GetComponent<Hexagon> ().SetId (i * nB + j);
			}

		float tInterval_0 = interval / 2.0; 
		float tInterval_1 = tInterval_0 * ratio; 
	
		direction = new int[6] (); 
		direction [0] = new pair (tInterval_0, tInterval_1);
		direction [1] = new pair (interval, 0); 
		direction [2] = new pair (tInterval_0, -tInterval_1); 
		direction [3] = new pair (-tInterval_0, tInterval_1); 
		direction [4] = new pair (-interval, 0); 
		direction [5] = new pair (-tInterval_0, tInterval_1);
	}

	public void FindPath(GameObject unit, int dest) {
		var move = unit.GetComponent<UnitMover> ();
		move.nxtPos.Clear (); 
		unCover.Clear (); 
		covered.Clear (); 
		unCover.Insert (move.curPos, 0);
		int curID = unCover.TopID (); 
		while (curID != dest) {
			covered.Insert (curID, unCover.TopKey ()); 
			unCover.Pop (); 
		}
	}
}
