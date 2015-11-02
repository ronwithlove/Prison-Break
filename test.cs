using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	private Vector3 vA;
	private Vector3 vB;
	private Vector3 vC;
	// Use this for initialization
	void Start () {
		vA = new Vector3 (0,0,0);
		vB = new Vector3 (3,0,4);
		vC = Vector3.Lerp (vA,vB,(3.0f/4.0f));
		print (vC);
	}
}
