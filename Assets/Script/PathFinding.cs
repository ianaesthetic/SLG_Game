using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heap { 
	private List<int> id; 
	private List<int> keyReal;
	private List<int> keyEstimate;
	private List<int> dist; 
	private List<int> l; 
	private List<int> r; 

	int nodeCount; 
	int root; 

	public heap() {
		id = new List<int> (); 
		keyReal = new List<int> (); 
		keyEstimate = new List<int> (); 
		dist = new List<int> (); 
		l = new List<int> (); 
		r = new List<int> ();
		nodeCount = 0; 
		root = -1; 
	}

	public void Clear() {
		id.Clear (); 
		keyReal.Clear ();
		keyEstimate.Clear ();
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
		if (key.Estimate[b] < key[a])
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
		public int preID;

		public node(int _id, int _dist, int _preID) {
			id = _id; 
			dist = _dist;
			preID = _preID;
		}
	};	
		
	List<node> nodeMem;

	public map() {
		nodeMem = new List<node> ();
	}

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
		
	public int GetDist(int id) {
		for (int i = 0; i < nodeMem.Count; ++i) {
			if (nodeMem [i].id == id) {
				return nodeMem[i].dist;
			}
		}
		return PathFinding.MAXDIST;
	}

	public void Insert(int id, int dist, int pre) {
		nodeMem.Add (new node(id, dist, pre));
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

	static int MAXDIST = 0x3f3f3f3f;

	public float lB, rB, uB, dB; 
	public int nB;
	public GameObject hexagon; 

	private float wB, hB, hStart, wStart, scaleFactor, interval;
	private heap unCover; 
	private map covered; 
	private int[] direction; 

	public int GetID(int posH, int posW, bool flag) {
		if (flag == false) {
			if (posH < 0 || posH > nB - 1 || posW < 0 || posW > nB - 1)
				return -1;
			else
				return posH * nB + posW;
		}
		else {
			if (posH < 0 || posH > nB - 2 || posW < 0 || posW > nB - 2)
				return -1;
			else
				return posH * (nB - 1) + posW + nB * nB;
		}
	}
		
	private void SetDirection(int id) {

		int th, tw, nxtID; 
		th = id / nB;
		tw = id % nB;

		if (id < nB * nB) {
			nxtID = GetID (th - 1, tw, false); direction [0] = nxtID == -1 ? nxtID : -1; 
			nxtID = GetID (th + 1, tw, false); direction [1] = nxtID == -1 ? nxtID : -1; 
			nxtID = GetID (th - 1, tw - 1, true); direction [2] = nxtID == -1 ? nxtID : -1; 
			nxtID = GetID (th - 1, tw, true); direction [3] = nxtID == -1 ? nxtID : -1; 
			nxtID = GetID (th, tw - 1, true); direction [4] = nxtID == -1 ? nxtID : -1;
			nxtID = GetID (th, tw, true); direction [5] = nxtID == -1 ? nxtID : -1; 
		}
		else {
			nxtID = GetID (th - 1, tw, true); direction [0] = nxtID == -1 ? nxtID : -1; 
			nxtID = GetID (th + 1, tw, true); direction [1] = nxtID == -1 ? nxtID : -1; 
			nxtID = GetID (th, tw, false); direction [2] = nxtID == -1 ? nxtID : -1; 
			nxtID = GetID (th, tw + 1, false); direction [3] = nxtID == -1 ? nxtID : -1; 
			nxtID = GetID (th + 1, tw, false); direction [4] = nxtID == -1 ? nxtID : -1; 
			nxtID = GetID (th + 1, tw + 1, false); direction [5] = nxtID == -1 ? nxtID : -1; 
		}
	}

	public void GetPos(out float posH, out float posW, int id) {
		if (id < nB * nB) {
			int tmp; 
			tmp = (int)(id / nB); 
			posH = (float)tmp * interval + dB;
			tmp = id % nB; 
			posW = (float)tmp * interval + lB;
		}
		else {
			id -= nB * nB;
			int tmp; 
			tmp = (int)(id / (nB - 1));
			posH = ((float)tmp + 0.5f) * interval + dB; 
			tmp = id % (nB - 1); 
			posW = ((float)tmp + 0.5f) * interval + lB;
		}
	}

	public int GridDistCost(int id) {
		return 1; 
	}

	void Start() {

		float ratio = Mathf.Sqrt (3); 
		scaleFactor = 0.9f;
		direction = new int[6]; 
		unCover = new heap (); 
		covered = new map ();

		wB = rB - lB;
		hB = wB - dB;

		if (hB * ratio < wB) {
			hB *= scaleFactor; 
			wB = hB * ratio; 
		} else {
			wB *= scaleFactor; 
			hB = wB / ratio; 
		}

		interval = hB / (float)nB;
		hStart = dB + (wB - dB - hB) / 2.0f; 
		wStart = lB + (rB - lB - wB) / 2.0f; 
	
		for (int i = 0; i < nB; ++i) {
			for (int j = 0; j < nB; ++j) {
				GameObject tObject = Instantiate (hexagon, new Vector3 (i * interval + hStart, j * interval + wStart, 0), Quaternion.identity) as GameObject;
				tObject.GetComponent<Hexagon> ().SetId (i * nB + j);
			}
		}

		for (int i = 0; i < nB - 1; ++i)
			for (int j = 0; j < nB - 1; ++j) {
				GameObject tObject = Instantiate (
					hexagon, 
					new Vector3 (((float)i + 0.5f) * interval + hStart, ((float)j + 0.5f) * interval + wStart, 0),
					Quaternion.identity) as GameObject;
				tObject.GetComponent<Hexagon> ().SetId (i * (nB - 1) + j + nB * nB); 
			}
	}

	public void FindPath(GameObject unit, int dest) {
		var move = unit.GetComponent<UnitMover> ();
		move.nxtPos.Clear (); 
		unCover.Clear (); 
		covered.Clear (); 
		unCover.Insert (move.curPos, 0);
		int curID = unCover.TopID (); 
		var curIDDist = unCover.TopKey();
		while (curID != dest) {
			covered.Insert (curID, unCover.TopKey (), -1); 

			SetDirection (curID);
			for (int i = 0; i < 6; ++i) {
				if (direction [i] != -1 && covered.GetDist(direction[i]) > GridDistCost(direction[i]) +  curIDDist) {
					
				}
			}
		}
	}
}
