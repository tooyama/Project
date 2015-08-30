using UnityEngine;
using System.Collections;
//テスト
public class DiceRoll : MonoBehaviour 
{
    public Vector3 move = new Vector3(0,0,0);

	public SHManeger shm;

	void Start () 
    {
        Rigidbody[] rigidArray = gameObject.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigid in rigidArray)
        {
            rigid.Sleep();
        }
        //gameObject.GetComponent<Rigidbody>().Sleep();
	}
    
	void diceRoll()
    {
        Rigidbody[] rigidArray = gameObject.GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rigid in rigidArray){
            rigid.WakeUp();
			Vector3 tempMove = move;
			tempMove.x += Random.Range (0, 20);
			tempMove.y += Random.Range (50, 150);
			tempMove.z += Random.Range (-10, 100);
			rigid.AddForce(tempMove);
        }
		Invoke ("changeSHGameStatus", .1f);
        //gameObject.GetComponent<Rigidbody>().WakeUp();
    }

	void changeSHGameStatus(){
		Debug.Log ("css start");
		int gameStatus = shm.gameStatus + 1;
		Debug.Log (gameStatus);
		if(gameStatus == 1) shm.ChangeGameStatus (gameStatus);
	}

	void FixedUpdate () 
    {
	
	}
}
