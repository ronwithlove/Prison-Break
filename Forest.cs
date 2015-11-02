using UnityEngine;
using System.Collections;

public class Forest : MonoBehaviour {

	public GameObject[] obstacles;
	public float startLength = 50;//从50米之后开始设成障碍物

	private Transform player;
	private WayPoints wayPoints;
	private int targetWayPointIndex;
	private EnvGenerator envGenerator;

	void Awake(){
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		wayPoints = transform.Find ("waypoints").GetComponent<WayPoints>();
		targetWayPointIndex = wayPoints.points.Length-1;
		envGenerator=Camera.main.GetComponent<EnvGenerator>();
	}

	// Use this for initialization
	void Start () {
		GenerateObstacle ();//生成障碍物

	}
	
	// Update is called once per frame
	void Update () {
		//因为在GetNextTargetPoint()靠玩家距离waypoints的位置创建森林了，所以这里就不用了
//		if(player.position.z>(transform.position.z+100)){
//			Camera.main.SendMessage("GenerateForest");//可以直接调用Main Camera上的z组件EnvGenerator的GenerateForest方法, 创建新的森林
//			GameObject.Destroy(this.gameObject);//主角已经跑远这条森林了，删除自己。
//		}
	}

	void GenerateObstacle(){
		float startZ = transform.position.z-3000;
		float endZ = transform.position.z;
		float z = startZ + startLength;
		while(z < endZ)	 {//这里用while是因为这个方法是写在Start()里的，程序一开始就要把所有路上的障碍都设置好
			z += Random.Range (100, 200);
			Vector3 position=GetWayPosByZ(z);//设成Z之后带入方法GetWayPosByZ得到在waypoints上的一个坐标。
			//创建障碍物
			int obsIndex=Random.Range (0,obstacles.Length);//随机生成一个障碍物
			GameObject go=GameObject.Instantiate(obstacles[obsIndex],position,Quaternion.identity) as GameObject;//设成的是Object 要把它转成 GameObject
			go.transform.parent=this.transform;//设成障碍物放到forest的子目录下
		}
	}

	Vector3 GetWayPosByZ(float z){  //看Vector3.Lerp差值 的笔记
		Transform[] points = wayPoints.points;
		int index = 0;
		for(int i=0;i<points.Length-1;i++){
			if( (z<=points[i].position.z) && (z>=points[i+1].position.z)){
				index=i;
				break;
			}
		}
		return Vector3.Lerp (points[index+1].position, points[index].position,(z-points[index+1].position.z)/(points[index].position.z- points[index+1].position.z));
	}

	public Vector3 GetNextTargetPoint(){  //PlayerMove 会叫这个方法
		while(true){
			if(wayPoints.points[targetWayPointIndex].position.z-player.position.z<10){//如果前一个点和玩家距离小于10米就移到下个点
				//这里原本是这句wayPoints.points[targetWayPointIndex].position-player.position).sqrMagnitude<100
				//但是当player左右移动之后就不会继续前进了，原来的意思是如果玩家距离前一个点在10米就找下个点，这样写是因为这个时候只有一条赛道
				//假如玩家从中间赛道滑到右边赛道，那么玩家右边赛道和中间赛道的waypoint至少也有14米，所以无法往下个点移动，这里改成比较Z轴距离就没问题了。
				targetWayPointIndex--;
				if(targetWayPointIndex<0){
					envGenerator.GenerateForest();//当前面障碍没了，就要创建新的森林了
					//Camera.main.SendMessage("GenerateForest"); //这句作用和上一句一样，但是怎么区分分别什么时候用那？
					Destroy(this.gameObject,1);//1秒之后销毁当前森林
					return envGenerator.forest1.GetNextTargetPoint();//再叫自己继续循环
				}
			}else{
				return wayPoints.points[targetWayPointIndex].position;//如果还不到10米，继续返回这个点的位置。
			}

		}
	}


}
