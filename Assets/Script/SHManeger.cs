using UnityEngine;
using System.Collections;

public class SHManeger : MonoBehaviour {

	public int gameStatus;

	public GameObject d4,d6,dicePanel,PlayerPanels,Monster;

	int d4Value,d6Value,greenCount,whiteCount,blackCount;

	int[] stages = new int[6];
	int[] greenCards = new int[16];
	int[] whiteCards = new int[15];
	int[] blackCards = new int[16];

	GameObject[] greens,whites,blacks;

	
	// Use this for initialization
	void Start () {
		setStage ();
		ChangeGameStatus (0);
	}

	void setStage(){
		randomize (stages);
		randomize (greenCards);
		randomize (whiteCards);
		randomize (blackCards);
		greenCount = 0;
		whiteCount = 0;
		blackCount = 0;
		greens = GameObject.FindGameObjectsWithTag("greenCard");
		foreach (GameObject card in greens) {
			card.SetActive(false);
		}
		whites = GameObject.FindGameObjectsWithTag("whiteCard");
		foreach (GameObject card in whites) {
			card.SetActive(false);
		}
		blacks = GameObject.FindGameObjectsWithTag("blackCard");
		foreach (GameObject card in blacks) {
			card.SetActive(false);
		}
	}

	void randomize(int[] array){
		for (int i = 0; i<array.Length; i++) {
			array[i] = i;
		}
		for (int i = 0; i<array.Length; i++) {
			int temp = array[i];
			int randomIndex = Random.Range (0,array.Length);
			array[i] = array[randomIndex];
			array[randomIndex] = temp;
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
				drawGreenCard();
			}else if(moveStatus <= 5){
				Monster.transform.position = panelPositions[2].position;
				drawFreeCard();
			}else if(moveStatus <= 6){
				Monster.transform.position = panelPositions[3].position;
				drawWhiteCard();
			}else if(moveStatus <= 7){
//				Monster.transform.Translate(panelPositions[0].localPosition);
				Debug.Log (moveStatus);
			}else if(moveStatus <= 8){
				Monster.transform.position = panelPositions[4].position;
				drawBlackCard();
			}else if(moveStatus <= 9){
				Monster.transform.position = panelPositions[5].position;
				hopeAndDespair();
			}else if(moveStatus <= 10){
				Monster.transform.position = panelPositions[6].position;
				altar();
			}
			Debug.Log ("move finished");
			ChangeGameStatus(3);
			break;

		case 3:

			break;
		}
	}

	void drawGreenCard(){
		greens [greenCards [greenCount]].SetActive (true);
		/* オババカードの各効果 */
		switch (greenCards [greenCount]) {
		}
		greenCount++;
		if (greenCount == greenCards.Length)
			randomize (greenCards);
	}
	void drawWhiteCard(){
		whites [whiteCards [whiteCount]].SetActive (true);
		/* 白カードの各効果 */
		switch (whiteCards [whiteCount]) {
		}
		whiteCount++;
		if (whiteCount == whiteCards.Length)
			randomize (whiteCards);
	}
	void drawBlackCard(){
		blacks [blackCards [blackCount]].SetActive (true);
		/* 白カードの各効果 */
		switch (blackCards [blackCount]) {
		}
		blackCount++;
		if (blackCount == blackCards.Length)
			randomize (blackCards);
	}
	void drawFreeCard(){
	}
	void hopeAndDespair(){
	}
	void altar(){
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

	IEnumerator waitingClick(){
		while (true) {
			if(Input.GetMouseButton(0)) break;
			yield return null;
		}
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
