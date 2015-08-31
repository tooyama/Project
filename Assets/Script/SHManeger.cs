using UnityEngine;
using System.Collections;

public class SHManeger : MonoBehaviour {

	public int gameStatus;

	public GameObject d4,d6,diceButton,PlayerPanels,Monster,EffectPanels,Enemy,buttonPanel;

	int d4Value,d6Value,greenCount,whiteCount,blackCount;

	int[] stages = new int[6];
	int[] greenCards = new int[16];
	int[] whiteCards = new int[15];
	int[] blackCards = new int[16];

	GameObject[] greens,whites,blacks,effects,attackButtons,drawButtons,stageButtons;

	Transform[] panelPositions;

	
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

		attackButtons = GameObject.FindGameObjectsWithTag ("attackButton");
		drawButtons = GameObject.FindGameObjectsWithTag ("drawButton");
		stageButtons = GameObject.FindGameObjectsWithTag ("stageButton");

		foreach (GameObject obj in attackButtons) {
			obj.SetActive(false);
		}
		foreach (GameObject obj in drawButtons) {
			obj.SetActive(false);
		}
		foreach (GameObject obj in stageButtons) {
			obj.SetActive(false);
		}


		Transform[] tmp = PlayerPanels.GetComponentsInChildren<Transform>();
		panelPositions = new Transform[tmp.Length - 1];
		for (int i = 0; i<panelPositions.Length; i++) {
			panelPositions[i] = tmp[i+1];
		}
		for (int i = 0; i<panelPositions.Length-1; i++) {
			for(int j = i+1;j<panelPositions.Length;j++){
				int pi,pj;
				pi = int.Parse(panelPositions[i].name.Substring(6));
				pj = int.Parse(panelPositions[j].name.Substring(6));
				if(pi>pj){
					Transform temp = panelPositions[i];
					panelPositions[i] = panelPositions[j];
					panelPositions[j] = temp;
				}
			}
		}

		for (int i  =0; i<panelPositions.Length; i++) {
			Debug.Log(panelPositions[i].name);
		}

		effects = GameObject.FindGameObjectsWithTag("effectPanel");

		for (int i = 0; i<effects.Length-1; i++) {
			for(int j = i+1;j<effects.Length;j++){
				int ei,ej;
				ei = int.Parse(effects[i].name.Substring(5));
				ej = int.Parse(effects[j].name.Substring(5));
				if(ei>ej){
					GameObject temp = effects[i];
					effects[i] = effects[j];
					effects[j] = temp;
				}
			}
		}
		for (int i  =0; i<effects.Length; i++) {
			Debug.Log (effects[i]);
		}

		for (int i = 0; i<stages.Length; i++) {
			Vector3 v3 = panelPositions[stages[i]].position;
			v3.y += 0.2f;
			effects[i].transform.position = v3;
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
			Debug.Log (moveStatus);

			moveStatus = 7;

			switch (moveStatus){
			case 2:
			case 3:
				moveStage(0);
				break;
			case 4:
			case 5:
				moveStage (1);
				break;
			case 6:
				moveStage (2);
				break;
			case 7:
				Debug.Log ("7 start");
				buttonPanel.SetActive(true);
				foreach(GameObject obj in stageButtons){
					obj.SetActive(true);
				}
				break;
			case 8:
				moveStage (3);
				break;
			case 9:
				moveStage(4);
				break;
			case 10:
				moveStage (5);
				break;
			}

			break;

		case 3:
			/* 攻撃の処理 */
			/* 自エリアと隣エリアに居るキャラを抽出 */
			/* 攻撃対象キャラを選択(攻撃しないも可)*/
			/* 攻撃(dice roll) */
			/* ダメージ処理 */

			break;
		}
	}

	public void hideStageButton(){
		foreach (GameObject obj in stageButtons) {
			obj.SetActive(false);
		}
		buttonPanel.SetActive (false);
	}

	public void moveStage(int stageIndex){
		hideStageButton ();
		Monster.transform.position = panelPositions[stages[stageIndex]].position;
		switch (stageIndex) {
		case 0:
			drawGreenCard();
			break;
		case 1:
			drawFreeCard ();
			break;
		case 2:
			drawWhiteCard();
			break;
		case 3:
			drawBlackCard();
			break;
		case 4:
			hopeAndDespair ();
			break;
		case 5:
			altar ();
			break;
		}
	}

	public void drawGreenCard(){
		greens [greenCards [greenCount]].SetActive (true);
		/* オババカードを送る相手を選択*/
		/* オババカードの各効果 */
		switch (greenCards [greenCount]) {
		}
		greenCount++;
		if (greenCount == greenCards.Length)
			randomize (greenCards);
		ChangeGameStatus(3);
	}
	public void drawWhiteCard(){
		whites [whiteCards [whiteCount]].SetActive (true);
		/* 白カードの各効果 */
		switch (whiteCards [whiteCount]) {
		}
		whiteCount++;
		if (whiteCount == whiteCards.Length)
			randomize (whiteCards);
		ChangeGameStatus(3);
	}
	public void drawBlackCard(){
		blacks [blackCards [blackCount]].SetActive (true);
		/* 白カードの各効果 */
		switch (blackCards [blackCount]) {
		}
		blackCount++;
		if (blackCount == blackCards.Length)
			randomize (blackCards);
		ChangeGameStatus(3);
	}
	void drawFreeCard(){
		buttonPanel.SetActive (true);
		foreach (GameObject obj in drawButtons) {
			obj.SetActive(true);
		}
	}
	void hopeAndDespair(){
		ChangeGameStatus(3);
	}
	void altar(){
		ChangeGameStatus(3);
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
		dicesActivate (false);
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
		buttonPanel.SetActive (flag);
		diceButton.SetActive(flag);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
