using UnityEngine;
using System.Collections;

public class EnvGenerator : MonoBehaviour {

	public Forest forest1;//这两个目前是用不上的
	public Forest forest2;
	public int forestCount = 2;

	public GameObject[] forest; //要把perfabs 里的forest1,23 拉倒组件上

	public void GenerateForest(){
		forestCount++;
		int type = Random.Range (0, 3);//包含0，不好含3
		GameObject newForest=GameObject.Instantiate(forest[type],new Vector3(0,0,forestCount*3000),Quaternion.identity) as GameObject;  //as GameObject 这是强转
		//不强转会出现错误：Assets/EnvGenerator.cs(15,28): error CS0266: Cannot implicitly convert type `UnityEngine.Object' to `UnityEngine.GameObject'. An explicit conversion exists (are you missing a cast?)
		forest1 = forest2; //这两句目前是用不上的，没任何意义呀？
		forest2 = newForest.GetComponent<Forest> ();
	}
}
