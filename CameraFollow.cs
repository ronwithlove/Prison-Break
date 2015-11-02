using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public float moveSpeed=1;
	private Transform player;
	private Vector3 offset=Vector3.zero;
	void Awake(){
		player=GameObject.FindGameObjectWithTag(Tags.player).transform;//取他的transform组件
		//player=GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Transform>();
		offset=transform.position-player.position;//获得Camera和主角之间的一个距离，以后不管主角怎么动，都保持这个距离
	}

	// Update is called once per frame
	void Update () {
		//transform.position=player.position+offset;// 这当然也可以，但是比较生硬，用插值写Camera比较好
			Vector3 tarPosition=player.position+offset;
			transform.position=Vector3.Lerp (transform.position,tarPosition,Time.deltaTime*moveSpeed);
	}
}
