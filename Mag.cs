using UnityEngine;
using System.Collections;

public class Mag : MonoBehaviour {
	private Vector3 vectorA=new Vector3(0,0,0);
	private Vector3 vectorB=new Vector3(3,0,0);
	private Vector3 vectorC=new Vector3(3,0,4);
	void Start() {
		print ("AC长度平方为"+(vectorA-vectorC).sqrMagnitude);
		print ("AB长度平方为"+(vectorA-vectorB).sqrMagnitude);
		print ("BC长度平方为"+(vectorB-vectorC).sqrMagnitude);

		print ("AC长度为"+(vectorA-vectorC).magnitude);
		print ("AB长度为"+(vectorA-vectorB).magnitude);
		print ("BC长度为"+(vectorB-vectorC).magnitude);
	}
}