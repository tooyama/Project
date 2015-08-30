using UnityEngine;
using System.Collections;

public class SHManeger : MonoBehaviour {

	public int gameStatus;

	public GameObject d4,d6,dicePanel,PlayerPanels,Monster;

	int d4Value,d6Value;

	int[] stages = new int[6];


	// Use this for initialization
	void Start () {
		ChangeGameStatus (0);
	}

	void setStage(){
		for (int i = 0; i<6; i++) {
			stages [i] = i;
		}
		for (int i = 0; i<stages.Length; i++) {
			int temp = stages[i];
			int randomIndex = Random.Range(0,stages.Length);
			stages[i] = stages[randomIndex];
			stages[randomIndex] = temp;
		}

	}

	public void ChangeGameStatus(int status){
		gameStatus = status;

		switch (status) {
		case 0:
			dicesActivate (true);
			break;
		case 1:

			Debug.Log ("case 1 start");
			StartCoroutine(getDiceValue());
			break;
		case 2:
			Debug.Log ("case 2 start");
			int moveStatus = d6Value + d4Value;
			Debug.Log (d6Value + " | " + d4Value);

			Transform[] panelPositions = PlayerPanels.GetComponentsInChildren<Transform>();
			Transform panelRoot = PlayerPanels.GetComponent<Transform>();

			if(moveStatus <= 3){
				Monster.transform.position = panelPositions[1].position;
			}else if(moveStatus <= 5){
				Monster.transform.position = panelPositions[2].position;
			}else if(moveStatus <= 6){
				Monster.transform.position = panelPositions[3].position;
			}else if(moveStatus <= 7){
//				Monster.transform.Translate(panelPositions[0].localPosition);
				Debug.Log (moveStatus);
			}else if(moveStatus <= 8){
				Monster.transform.position = panelPositions[4].position;
			}else if(moveStatus <= 9){
				Monster.transform.position = panelPositions[5].position;
			}else if(moveStatus <= 10){
				Monster.transform.position = panelPositions[6].position;
			}
			Debug.Log ("move finished");
			ChangeGameStatus(3);
			break;

		case 3:

			break;
		}
	}

	IEnumerator getDiceValue(){
		Debug.Log ("get dice value start");
		d6Value = 0;
		d4Value = 0;
		while(d6.GetComponent<Die>().rolling || d4.GetComponent<Die>().rolling){
			Debug.Log (d6.GetComponent<Die>().rolling + " | " + d4.GetComponent<Die>().rolling);
			yield return new WaitForSeconds(1.0f);
		}
		d6Value = d6.GetComponent<Die_d6>().value;
		d4Value = d4.GetComponent<Die_d6>().value;
		Debug.Log ("d6:" + d6Value + " d4:" + d4Value);
		d4Value = d4Value % 4 + 1;
		dicePanel.SetActive(false);
		Debug.Log ("case1 finish");
		ChangeGameStatus (2);
	}
	
	void dicesActivate(bool flag){
		d4.SetActive(flag);
		d6.SetActive(flag);
		dicePanel.SetActive(flag);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
