using UnityEngine;
using System.Collections;

public enum GameState{
	Menu,
	Playing,
	End
}
public class GameController : MonoBehaviour {

	public static GameState gameState=GameState.Menu;
	public GameObject tapToStart;
	public GameObject gameOver;

	void Update(){
		if( gameState==GameState.Menu){
			if(Input.GetMouseButtonDown(0)){
				gameState=GameState.Playing;
				tapToStart.SetActive(false);
			}
		}
		if(gameState==GameState.End){
			gameOver.SetActive(true);

			if(Input.GetMouseButtonDown(0)){
				gameState=GameState.Menu;//因为这是一个静态变量，所以Scene也是公用的，值光写下面的是没用的，要把State重设一下
				Application.LoadLevel (0);

			}

		}
	}

}
