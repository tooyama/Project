using UnityEngine;
using System.Collections;

public class PlayerStateManager : MonoBehaviour {

	int score;
	GameObject[] HP;
	// Use this for initialization
	void Start () {
		score = 0;
		HP = GameObject.FindGameObjectsWithTag ("hpPanel");
		for(int i = 0;i<HP.Length-1;i++){
			for(int j = i+1;j<HP.Length;j++){
				int hi,hj;
				hi = int.Parse(HP[i].name);
				hj = int.Parse(HP[j].name);
				if(hi>hj){
					GameObject tmp = HP[i];
					HP[i] = HP[j];
					HP[j] = tmp;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int getScore(){
		return score;
	}

	public void moveScore(int newScore){
		Debug.Log ("move score start");
		score = newScore;
		Vector3 v3 = gameObject.transform.position;
		v3.z = HP [score].transform.position.z;
		gameObject.transform.position = v3;
	}
}
