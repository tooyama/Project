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
		Debug.Log ("diceroll start");

        Rigidbody[] rigidArray = gameObject.GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rigid in rigidArray){
            rigid.WakeUp();
			Vector3 tempMove = move;
			tempMove.x += Random.Range (90, 120);
			tempMove.y += Random.Range (80, 180);
			tempMove.z += Random.Range (-50, 150);
			rigid.AddForce(tempMove);
        }

		Debug.Log ("force added");

		Invoke ("changeSHGameStatus", .2f);

		Debug.Log ("css invoke");
        //gameObject.GetComponent<Rigidbody>().WakeUp();
    }

	void changeSHGameStatus(){
		Debug.Log ("css start");
		int gameStatus = shm.gameStatus + 1;
		Debug.Log (gameStatus);
		if(gameStatus == 1) shm.ChangeGameStatus (gameStatus);
		if(gameStatus == 4) shm.getAttackValue ();
	}

	void FixedUpdate () 
    {
	
	}
}
