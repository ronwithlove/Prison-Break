using UnityEngine;
using System.Collections;

public enum TouchDir{
	None,
	Left,
	Right,
	Up,
	Down //最后一个是没逗号的，别习惯性打上
}
public class PlayerMove : MonoBehaviour {

	public float moveSpeed=100;
	public float moveDirectionSpeed=1;

	private EnvGenerator envGenerator;
	private TouchDir touchDir=TouchDir.None;	
	private Vector3 lastMouseDown=Vector3.zero;
	public int currentTrack=1; //当前赛道 ,设成 public在PlayerAnimation要作为判断用
	public int targetTrack=1; //目标赛道
	public bool isSliding= false;
	public float slideTime=1.4f;//这里是动画时间，在Animation 里有
	public bool isJumping=false;//是否跳跃
	public float jumpHeight=20;//跳跃高度
	public float jumpSpeed=1;//跳起和下落的速度

	private bool isUp=true;//是否挑起，true挑起，false下落
	private float jumppedHeight;//已经跳起的高度
	private float slideTimer=0;//滑动计时器
	private float moveDistance=0;
	private float[] wayPointOffset=new float[3]{-14,0,14};
	private Transform prisoner;
//	private Forest forest;

	void Awake(){
		envGenerator=Camera.main.GetComponent<EnvGenerator>();
//		forest=GameObject.Find("forest_1").GetComponent<Forest>();
		prisoner=this.transform.FindChild("Prisoner").transform;

	}

	
	// Update is called once per frame
	void Update () {
		if(GameController.gameState==GameState.Playing){
			Vector3 targetPos=envGenerator.forest1.GetNextTargetPoint();
			//Vector3 targetPos2=forest.GetNextTargetPoint();
			//transform.position=Vector3.Lerp (transform.position,targetPos,Time.deltaTime);//因为是插值移动的关系，所以速度不是匀速的换一种方式
			targetPos=new Vector3(targetPos.x+wayPointOffset[targetTrack],targetPos.y,targetPos.z);//获得waypoint之后，因为左右移动的关系，所以要把waypoint在x轴上也对应的移动一下
			Vector3 moveDirection= targetPos-transform.position;
			transform.position+=moveDirection.normalized*moveSpeed*Time.deltaTime; // Vector3.normailezed 把vector3变成方向，长度变成1

			MoveContorl();
		}
	}

	private void MoveContorl(){//位置处理
		TouchDir dir=GetTouchDir();//得到移动方向

		if(targetTrack!=currentTrack){//左右位置处理
			float leftDistance=Mathf.Lerp (0,moveDistance,moveDirectionSpeed*Time.deltaTime);//player位置为0,目标距离为14或者-14
			transform.position=new Vector3(transform.position.x+leftDistance,transform.position.y,transform.position.z);
			moveDistance-=leftDistance;
			if(Mathf.Abs(moveDistance)<0.5f){
				transform.position=new Vector3(transform.position.x+moveDistance,transform.position.y,transform.position.z);
				moveDistance=0;
				//上面两句直接写成leftDistance=0.5；可以不，等会试试
				currentTrack=targetTrack;//移动结束，现在的跑道就是目标跑道；
			}
		}
		if(isSliding){//滑行处理
			slideTimer+=Time.deltaTime;
			if(slideTimer>slideTime){//如果时间到了，停止播放 这里别写等于，因为是float所以很难刚好精确的等于，大于就好了
				slideTimer=0;
				isSliding=false;
			}
		}
		if(isJumping){
			float yMove=jumpSpeed*Time.deltaTime;
			if(isUp){ //跳起				
				prisoner.position=new Vector3(prisoner.position.x,prisoner.position.y+yMove,prisoner.position.z);
				jumppedHeight+=yMove;
				print (prisoner.position.y);
				if(jumpHeight-jumppedHeight<0f){//把绝对值去了，因为小于0之后变负的，但是绝度值一下又正了，于是越跳越高
					isUp=false;//说明最高点了，可以下落了。
				}
			}
			if(!isUp){//下落 这里不应该用else,用else 就在上升最高的地方还要等下一次update更新再下落。我们要马上就下落。
				prisoner.position=new Vector3(prisoner.position.x,prisoner.position.y-yMove,prisoner.position.z);
				jumppedHeight-=yMove;
				if(jumppedHeight<0.1f){//离地高度
					prisoner.position=new Vector3(prisoner.position.x,prisoner.position.y-jumppedHeight,prisoner.position.z);
					isJumping=false;
					jumppedHeight=0;
				}
			}
		}
	}

	TouchDir GetTouchDir(){//方向处理
		if(Input.GetMouseButtonDown (0)){//获取鼠标按下位置
			lastMouseDown=Input.mousePosition;
		}
		if(Input.GetMouseButtonUp(0)){//获取鼠标抬起位置，0代表左键
			Vector3 mouseUp= Input.mousePosition;
			Vector3 touchOffset=mouseUp-lastMouseDown;//得到鼠标拖动之间的差，接下来判断方向, 模拟手指在键盘上的操作，这里先用鼠标

			if(Mathf.Abs(touchOffset.x)>50 || Mathf.Abs(touchOffset.y)>50){//如果X轴和Y轴移动有一个大于50像素的话，再进行判断方向（不然就忽略这个滑动操作）
				if(Mathf.Abs(touchOffset.x)>Mathf.Abs(touchOffset.y)&&touchOffset.x>0){//如果x轴的移动大于Y轴的,而且x轴大于0，是往右
					if(targetTrack<2){
						targetTrack++;
						moveDistance=14;
					}
					return TouchDir.Right;
				}else if(Mathf.Abs(touchOffset.x)>Mathf.Abs(touchOffset.y)&&touchOffset.x<0){ //x小于0么就是左边了。
					if(targetTrack>0){
						targetTrack--;
						moveDistance=-14;
					}
					return TouchDir.Left;
				}else if(Mathf.Abs(touchOffset.x)<Mathf.Abs(touchOffset.y)&&touchOffset.y>0){
					if(isJumping==false){//只有不在跳跃的状态下才能跳跃
						isJumping=true;
						isUp=true;
					}
					return TouchDir.Up;
				}else if(Mathf.Abs(touchOffset.x)<Mathf.Abs(touchOffset.y)&&touchOffset.y<0){ 
					isSliding=true;
					slideTimer=0;
					return TouchDir.Down;
				}//这里也不用写等于的情况，随便划一下X=Y的也是牛了。当然若要写，就等于的时候往上滑好了
			}
		}
		return TouchDir.None;

	}

}
