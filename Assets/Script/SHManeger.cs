﻿using UnityEngine;
using System.Collections;

public class SHManeger : MonoBehaviour {

	public int gameStatus;

	public GameObject d4,d6,diceButton,PlayerPanels,Monster,EffectPanels,Enemy,buttonPanel;

	int d4Value,d6Value,greenCount,whiteCount,blackCount;

	int[] stages = new int[6];
	int[] greenCards = new int[16];
	int[] whiteCards = new int[15];
	int[] blackCards = new int[16];

	GameObject[] greens,whites,blacks,effects,attackButtons,playerStates,monsters;

	GameObject stageButtons,drawButtons;

	Transform[] panelPositions;

	int[] playersPositions = new int[2];

	int playerId,attackTarget;
	
	// Use this for initialization
	void Start () {
		setStage ();

		playerId = 0;

		for (int i = 0; i<playersPositions.Length; i++) {
			playersPositions[i] = -2;
		}

		ChangeGameStatus (0);
	}

	void setStage(){
		/* カードのシャッフル */
		randomize (stages);
		randomize (greenCards);
		randomize (whiteCards);
		randomize (blackCards);
		greenCount = 0;
		whiteCount = 0;
		blackCount = 0;

		/* カード類を非表示に */
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

		/* ボタン類のセット */
		attackButtons = GameObject.FindGameObjectsWithTag ("attackButton");
		drawButtons = GameObject.FindGameObjectWithTag ("drawButton");
		stageButtons = GameObject.FindGameObjectWithTag ("stageButton");

		/* プレイヤーセット */
		monsters = GameObject.FindGameObjectsWithTag ("Monster");

		/* attackButtonの並び替え */
		for (int i = 0; i<attackButtons.Length-1; i++) {
			for(int j = i+1;j<attackButtons.Length;j++){
				int ai,aj;
				ai = int.Parse(attackButtons[i].name.Substring(12));
				aj = int.Parse(attackButtons[j].name.Substring(12));
				if(ai>aj){
					GameObject tmp = attackButtons[i];
					attackButtons[i] = attackButtons[j];
					attackButtons[j] = tmp;
				}
			}
		}


		/* ボタン類を非表示に */
		foreach (GameObject obj in attackButtons) {
			obj.SetActive(false);
		}
		drawButtons.SetActive (false);
		stageButtons.SetActive (false);

		/* イベントパネルの位置情報のセット */
		Transform[] tmps = PlayerPanels.GetComponentsInChildren<Transform>();
		panelPositions = new Transform[tmps.Length - 1];
		for (int i = 0; i<panelPositions.Length; i++) {
			panelPositions[i] = tmps[i+1];
		}
		/* 名前順に並び替え */
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

		/* イベントパネルカード情報のセット */
		effects = GameObject.FindGameObjectsWithTag("effectPanel");

		/* 並べ替え */
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

		/* パネルを設定した位置に移動 */
		for (int i = 0; i<stages.Length; i++) {
			Vector3 v3 = panelPositions[stages[i]].position;
			v3.y += 0.2f;
			effects[i].transform.position = v3;
		}

		/*
		 * panelPositions -> 老婆の庵等のカードの座標情報
		 * stages -> stages[0]が3なら老婆の庵をpanelPositions[3]の座標に置く
		 * 
		 * */

		/* playerState(HP表示)のセット */
		playerStates = GameObject.FindGameObjectsWithTag("playerState");
		for(int i = 0;i<playerStates.Length-1;i++){
			for(int j = i+1;j<playerStates.Length;j++){
				int pi,pj;
				pi = int.Parse (playerStates[i].name.Substring(6));
				pj = int.Parse (playerStates[j].name.Substring(6));
				if(pi>pj){
					GameObject tmp = playerStates[i];
					playerStates[i] = playerStates[j];
					playerStates[j] = tmp;
				}
			}
		}
	}

	/* 配列のシャッフル */
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
			/* ダイスのアクティブ化 */
			dicesActivate (true);
			break;
		case 1:
			/* ダイスの値取得 */
			StartCoroutine(getDiceValue());
			break;
		case 2:
			/* コマ移動 */
			getStageIndex(d6Value+d4Value);
			break;
		case 3:
			/* 攻撃の処理 */
			bool[] playerExists = new bool[2];
			for(int i = 0;i<playerExists.Length;i++){
				playerExists[i] = false;
			}
			for(int i = 0;i<playersPositions.Length;i++){
				if(i != playerId && playersPositions[i]/2 == playersPositions[playerId]/2){
//					playerExist = true;
					playerExists[i] = true;
				}
			}
			buttonPanel.SetActive(true);
			attackButtons[0].SetActive(true);

//			playerExist = true;
			for(int i = 0;i<playerExists.Length;i++){
				if(playerExists[i]){
					attackButtons[i+1].SetActive(true);
				}
			}
/*			if(playerExist){
				for(int i = 1;i<attackButtons.Length;i++){
					attackButtons[i].SetActive(true);
				}
			}*/
			break;
		case 4:
			Debug.Log ("finished");
			playerId++;
			playerId = playerId%2;
			ChangeGameStatus(0);
			break;
		}
	}

	/* 出目から移動先を決定 */
	public void getStageIndex(int moveStatus){
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
			buttonPanel.SetActive(true);
			stageButtons.SetActive(true);
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

	}

	/* ステージ移動 */
	public void moveStage(int stageIndex){
		stageButtons.SetActive (false);;
		monsters[playerId].transform.position = panelPositions[stages[stageIndex]].position;
		playersPositions [playerId] = stages [stageIndex];
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

	/* ********************** */
	/* ********************** */
	/* ****移動効果ここから**** */
	/* ********************** */
	/* ********************** */

	/* 老婆の庵 */
	public void drawGreenCard(){
		/* 時空の扉からのドロー時の処理 */
		drawButtons.SetActive (false);

		greens [greenCards [greenCount]].SetActive (true);
		/* オババカードを送る相手を選択*/
		/* オババカードの各効果 */
		switch (greenCards [greenCount]) {
		}
		greens [greenCards [greenCount]].SetActive (false);
		greenCount++;
		if (greenCount == greenCards.Length) {
			randomize (greenCards);
		}
		Debug.Log ("finish dgc");
		ChangeGameStatus(3);
	}
	/* 教会 */
	public void drawWhiteCard(){
		/* 時空の扉からのドロー時の処理 */
		drawButtons.SetActive (false);
		
		whites [whiteCards [whiteCount]].SetActive (true);
		/* 白カードの各効果 */
		switch (whiteCards [whiteCount]) {
		}
		whites [whiteCards [whiteCount]].SetActive (false);
		whiteCount++;
		if (whiteCount == whiteCards.Length)
			randomize (whiteCards);
		Debug.Log ("finish dwc");
		ChangeGameStatus(3);
	}

	/* 共同墓地 */
	public void drawBlackCard(){
		/* 時空の扉からのドロー時の処理 */
		drawButtons.SetActive (false);
		
		blacks [blackCards [blackCount]].SetActive (true);
		/* 白カードの各効果 */
		switch (blackCards [blackCount]) {
		}
		blacks [blackCards [blackCount]].SetActive (false);
		blackCount++;
		if (blackCount == blackCards.Length)
			randomize (blackCards);
		Debug.Log ("finish dbc");
		ChangeGameStatus(3);
	}

	/* 時空の扉 */
	void drawFreeCard(){
		buttonPanel.SetActive (true);
		drawButtons.SetActive (true);
	}

	/* 希望と絶望の森 */
	void hopeAndDespair(){
		ChangeGameStatus(3);
	}

	/* いにしえの祭壇 */
	void altar(){
		ChangeGameStatus(3);
	}

	/* ********************** */
	/* ********************** */
	/* ****移動効果ここまで**** */
	/* ********************** */
	/* ********************** */

	public void attackEnemyReady(int id){
		attackTarget = id;
		dicesActivate (true);
	}
	
	public void activateAttackButtons(bool flag){
		foreach(GameObject obj in attackButtons){
			obj.SetActive(flag);
		}
	}
	
	public void getAttackValue(){
		StartCoroutine(getDiceValue());
	}
	
	public void attackEnemy(){
		int tmpScore = playerStates [attackTarget].GetComponent <PlayerStateManager>().getScore ();
		tmpScore += Mathf.Abs (d6Value - d4Value);
		playerStates [attackTarget].GetComponent<PlayerStateManager>().moveScore (tmpScore);
		ChangeGameStatus (4);
	}
	

	IEnumerator getDiceValue(){
		d6Value = 0;
		d4Value = 0;
		while (d6.GetComponent<Die>().rolling || d4.GetComponent<Die>().rolling) {
			yield return new WaitForSeconds (2.0f);
		}
		d6Value = d6.GetComponent<Die_d6>().value;
		d4Value = d4.GetComponent<Die_d6>().value;
		Debug.Log ("d6:" + d6Value + " d4:" + d4Value);
		d4Value = d4Value % 4 + 1;
		dicesActivate (false);
		Debug.Log (gameStatus);
		if (gameStatus == 1) {
			ChangeGameStatus (2);
		} else if (gameStatus == 3) {
			attackEnemy();
		}
	}

	IEnumerator waitingClick(){
		while (true) {
			if(Input.GetMouseButton(0)) break;
			yield return null;
		}
	}
	
	void dicesActivate(bool flag){
		d4.transform.position = new Vector3(5,20,-2);
		d6.transform.position = new Vector3(-2,14,0);
		d4.transform.rotation = Quaternion.Euler (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
		d6.transform.rotation = Quaternion.Euler (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
		d4.SetActive(flag);
		d6.SetActive(flag);
		d4.GetComponent<Rigidbody>().Sleep();
		d6.GetComponent<Rigidbody>().Sleep();

		buttonPanel.SetActive (flag);
		diceButton.SetActive(flag);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
