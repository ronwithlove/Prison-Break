using UnityEngine;
using System.Collections;

public class PlayerSmallCollider : MonoBehaviour {
	
	private PlayerAnimation playerAnimation;
	
	void Awake(){
		playerAnimation=GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAnimation>();//获取Player下面的PlayerAnimation
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag==Tags.obstacles && GameController.gameState==GameState.Playing&&playerAnimation.animState==AnimationState.Slide){
			GameController.gameState=GameState.End;
		}
	}
}
