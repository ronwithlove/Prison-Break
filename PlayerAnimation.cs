using UnityEngine;
using System.Collections;

public enum AnimationState{
	Idle,
	Run,
	TurnLeft,
	TurnRight
}

public class PlayerAnimation : MonoBehaviour {

	private Animation playerAnim;
	private AnimationState animState=AnimationState.Idle;
	private PlayerMove playerMove; //要持有一个PlayerMove的引用，定义一个


	void Awake(){
		playerAnim=transform.Find ("Prisoner").GetComponent<Animation>();//如果没有就会返回null
		playerMove=this.GetComponent<PlayerMove>();//赋值，就是要得到这个组价上的PlayerMove
	}

	// Update is called once per frame
	void Update () { //在这里去做一个动画状态的判断
		if(GameController.gameState==GameState.Menu){
			animState=AnimationState.Idle;
		}else if(GameController.gameState==GameState.Playing){
			animState=AnimationState.Run;
			if(playerMove.targetTrack > playerMove.currentTrack){//目标大，当前小就往右
				animState=AnimationState.TurnRight;
			}
			if(playerMove.targetTrack<playerMove.currentTrack){//反之亦然
				animState=AnimationState.TurnLeft;
			}	
		}
	}

	void LateUpdate() { //在这里去做一个动画的播放
		switch(animState){ //其实也是可以直接写在Update后面，没问题，这样写清晰一点
		case AnimationState.Idle:PlayIdle();	break;
		case AnimationState.Run:PlayAnim("run");break;
		case AnimationState.TurnLeft:
			GetComponentInChildren<Animation>()["left"].speed=2;//倍数，2倍速度播放
			PlayAnim("left");break;
		case AnimationState.TurnRight:
			GetComponentInChildren<Animation>()["right"].speed=2;//这里如果3倍的话会循环播放， 因为原本动画时间没结束
			PlayAnim("right");break;
		}

	}

	private void PlayIdle(){ //Idle 有两个动画，所以单独来写，其余的用一个PlayAnim（）就可以了
		if(playerAnim.IsPlaying("Idle_1")==false&&playerAnim.IsPlaying("Idle_2")==false){
			playerAnim.Play ("Idle_1");
			playerAnim.PlayQueued("Idle_2");//播放队列 ,下一行和这行一个效果，是不是说后面省略了就是下面这句话。
			//playerAnim.PlayQueued("Idle_2", QueueMode.CompleteOthers);//当所有其他动画停止播放，这个动画才会开始。
		}
	}

	private void PlayAnim(string animName){
		if(playerAnim.Play(animName)==false){
			playerAnim.Play(animName);
		}
	}

}
