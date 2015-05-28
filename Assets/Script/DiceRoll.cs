using UnityEngine;
using System.Collections;

public class DiceRoll : MonoBehaviour 
{
    public Vector3 move = new Vector3(-10,0,0);

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

        foreach (Rigidbody rigid in rigidArray)
        {
            rigid.WakeUp();

            rigid.AddForce(move);
        }
        //gameObject.GetComponent<Rigidbody>().WakeUp();
    }

	void FixedUpdate () 
    {
	
	}
}
