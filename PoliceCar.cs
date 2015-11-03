using UnityEngine;
using System.Collections;

public class PoliceCar : MonoBehaviour {
	private bool isPlay=false;

	void Update () {
		if(GameController.gameState==GameState.End){
			PlayTireSquealSound();
		}
	}

	private void PlayTireSquealSound(){
		if(isPlay==false&&GetComponent<AudioSource>().isPlaying==false){
			GetComponent<AudioSource>().Play();
			isPlay=true;
		}
	}

}
