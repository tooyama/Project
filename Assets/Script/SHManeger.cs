using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class SHManeger : MonoBehaviour {

	public int gameStatus;

	public static int playerNum = 5;

    public GameObject fazeEffect;

	public GameObject d4,d6,diceButton,PlayerPanels,Monster,EffectPanels,Enemy,buttonPanel,character,buttonPrefab;

    public Camera mainCamera,subCamera;

    public DiceRoll diceroll;

	int d4Value,d6Value,greenCount,whiteCount,blackCount;

	int[] stages = new int[6];
	int[] greenCards = new int[16];
	int[] whiteCards = new int[15];
	int[] blackCards = new int[16];

	GameObject[] greens,whites,blacks,effects,attackButtons,selectButtons,playerStates,monsters;

	CharacterState[] characters;

    GameObject stageButtons, drawButtons, choiseButtons, firstButton, secondButton, openButton1, openButton2,machineAttackButton1, machineAttackButton2;

	Transform[] panelPositions;

    PlayerStateManager[] playerStatesM;

	int[] playersPositions = new int[playerNum];
	bool[] playerExists = new bool[playerNum];

	int playerId,attackTarget,angelId = -1,compass_first = -1,compass_second,wightCounter = 0;

    bool masamune,handgun,wisdom,machineGun;

    bool mainPlayer = true;

    List<int> attackTargetList,attackedTarget;

//<<<<<<< HEAD
	string selectingAction = "",diceAction = "";
//=======
    //ここから追加
    /*フェード中の透明度*/
    private float fadeAlpha = 0;
    /*フェード中かどうか*/
    private bool isFading = false;
    /*フェード色*/
    public Color fadeColor = Color.black;
//>>>>>>> origin/master

	// Use this for initialization
	void Start () {
		setStage ();

		playerId = 0;

		for (int i = 0; i<playersPositions.Length; i++) {
			playersPositions[i] = -2;
		}

		ChangeGameStatus (-1);
	}

    /*                                */
    /* 開始時のステージ初期化ここから */
    /*                                */

    void setStage()
    {
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
        foreach (GameObject card in blacks)
        {
            card.SetActive(false);
        }
        /* 緑カードの並び替え */
        for (int i = 0; i < greens.Length - 1; i++)
        {
            for (int j = i + 1; j < greens.Length; j++)
            {
                int ai, aj;
                ai = int.Parse(greens[i].name.Substring(4));
                aj = int.Parse(greens[j].name.Substring(4));
                if (ai > aj)
                {
                    GameObject tmp = greens[i];
                    greens[i] = greens[j];
                    greens[j] = tmp;
                }
            }
        }
        /* 黒カードの並び替え */
		for (int i = 0; i<blacks.Length-1; i++) {
			for(int j = i+1;j<blacks.Length;j++){
				int ai,aj;
				ai = int.Parse(blacks[i].name.Substring(4));
				aj = int.Parse(blacks[j].name.Substring(4));
				if(ai>aj){
					GameObject tmp = blacks[i];
					blacks[i] = blacks[j];
					blacks[j] = tmp;
				}
			}
		}
        /* 白カードの並び替え */
        for (int i = 0; i < whites.Length - 1; i++)
        {
            for (int j = i + 1; j < whites.Length; j++)
            {
                int ai, aj;
                ai = int.Parse(whites[i].name.Substring(4));
                aj = int.Parse(whites[j].name.Substring(4));
                if (ai > aj)
                {
                    GameObject tmp = whites[i];
                    whites[i] = whites[j];
                    whites[j] = tmp;
                }
            }
        }

		/* ボタン類のセット */
		attackButtons = GameObject.FindGameObjectsWithTag ("attackButton");
		selectButtons = GameObject.FindGameObjectsWithTag ("selectButton");
		drawButtons = GameObject.FindGameObjectWithTag ("drawButton");
        stageButtons = GameObject.FindGameObjectWithTag("stageButton");
        choiseButtons = GameObject.FindGameObjectWithTag("choiseButton");

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
		/* selectButtonの並び替え */
		for (int i = 0; i<selectButtons.Length-1; i++) {
			for(int j = i+1;j<selectButtons.Length;j++){
				int ai,aj;
				ai = int.Parse(selectButtons[i].name.Substring(12));
				aj = int.Parse(selectButtons[j].name.Substring(12));
				if(ai>aj){
					GameObject tmp = selectButtons[i];
					selectButtons[i] = selectButtons[j];
					selectButtons[j] = tmp;
				}
			}
		}


		/* ボタン類を非表示に */
		foreach (GameObject obj in attackButtons) {
			obj.SetActive(false);
		}
		foreach (GameObject obj in selectButtons) {
			obj.SetActive(false);
		}
		drawButtons.SetActive (false);
		stageButtons.SetActive (false);
        choiseButtons.SetActive(false);
        /* 神秘のコンパス用のボタン(prefabから生成) */
        firstButton = Instantiate(buttonPrefab, new Vector3(580,160,0), Quaternion.identity) as GameObject;
        secondButton = Instantiate(buttonPrefab, new Vector3(580, 60, 0), Quaternion.identity) as GameObject;
        firstButton.transform.SetParent(buttonPanel.GetComponent<Transform>());
        secondButton.transform.SetParent(buttonPanel.GetComponent<Transform>());
        firstButton.GetComponent<Button>().onClick.AddListener(() => selectCompass(1));
        secondButton.GetComponent<Button>().onClick.AddListener(() => selectCompass(2));
        firstButton.SetActive(false);
        secondButton.SetActive(false);

        /* 正体公開用ボタン */
        openButton1 = Instantiate(buttonPrefab, new Vector3(580, 160, 0), Quaternion.identity) as GameObject;
        openButton2 = Instantiate(buttonPrefab, new Vector3(580, 60, 0), Quaternion.identity) as GameObject;
        openButton1.transform.SetParent(buttonPanel.GetComponent<Transform>());
        openButton2.transform.SetParent(buttonPanel.GetComponent<Transform>());
        openButton1.GetComponent<Button>().onClick.AddListener(() => activateOpenButtons(false));
        openButton2.GetComponent<Button>().onClick.AddListener(() => activateOpenButtons(false));
        openButton1.GetComponent<Button>().onClick.AddListener(() => revealIdentity());
        openButton2.GetComponent<Button>().onClick.AddListener(() => ChangeGameStatus(0));
        openButton1.transform.FindChild("Text").GetComponent<Text>().text = "正体を公開する";
        openButton2.transform.FindChild("Text").GetComponent<Text>().text = "正体を公開しない";
        openButton1.SetActive(false);
        openButton2.SetActive(false);


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
        playerStatesM = new PlayerStateManager[playerStates.Length];
        for (int i = 0; i < playerStates.Length; i++)
        {
            playerStatesM[i] = playerStates[i].GetComponent<PlayerStateManager>();
        }
        

		/* Character情報のセット */
		characters = character.GetComponentsInChildren<CharacterState> ();

		for (int i = 0; i<characters.Length; i++) {
			int randomIndex = Random.Range (0,characters.Length);
			CharacterState temp = characters[i];
			characters[i] = characters[randomIndex];
			characters[randomIndex] = temp;
		}
		for(int i = 0;i<playerNum;i++){
            Debug.Log(i + " HP:" + characters[i].maxHp + "(" + characters[i].fullname + ")");
			playerStatesM [i].setMaxHp(characters[i].maxHp);
		}
        dicesActivate(false);
	}



	/* 配列のシャッフル(補助関数) */
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

    /*                                */
    /* 開始時のステージ初期化ここまで */
    /*                                */

    /*                                */
    /*　 ゲームステート管理ここから　 */
    /*                                */

    bool compass;
    
    public void ChangeGameStatus(int status){
        Debug.Log("ChangeGameStatus:" + status + " playerId:" + playerId);
		gameStatus = status;
		switch (status) {
            case -1:
                if (playerStatesM[playerId].dead)
                {
                    ChangeGameStatus(4);
                }
                if (!characters[playerId].reveal)
                {
                    activateOpenButtons(true);
                }
                else
                {
                    ChangeGameStatus(0);
                }
                break;
		    case 0:
                /* キャサリンの特殊能力 */
                if (characters[playerId].reveal && characters[playerId].fullname == CharacterState.CharacterFullName.Catherine)
                {
                    playerStatesM[playerId].getDamage(-1);
                }

                /* 守護天使の効果切れ */
                    angelId = -1;
                /*ダイスロール時のカメラに切り替え*/
                StartCoroutine(TransCamera(1.0f));
                compass = characters[playerId].findEquipment("Compass");

                if (!mainPlayer)
                {
				    d4Value = Random.Range(1,4);
				    d6Value = Random.Range(1,6);
				    ChangeGameStatus(2);
			    }else{
				    /* ダイスのアクティブ化 */
				    dicesActivate (true);
			    }
			    break;
		    case 1:
			    /* ダイスの値取得 */
    //            Debug.Log(compass);
    //			StartCoroutine(getDiceValue());
			    break;
		    case 2:
                /*通常のカメラに切り替え*/
                StartCoroutine(TransCamera(1.0f));
			    /* コマ移動 */
			    getStageIndex(d6Value+d4Value);
			    break;
		    case 3:
			    Debug.Log ("phase 3 playerId = " + playerId);
                masamune = false;
                handgun = false;
                /* マサムネの所持確認 */
                if(characters[playerId].findEquipment("Masamune")){
                    masamune = true;
                }
                /* 拳銃の所持確認 */
                if(characters[playerId].findEquipment("HandGun")){
                    handgun = true;
                }
			    /* 攻撃の処理 */
			    checkPlayerExist(); //攻撃範囲のプレイヤー存在確認
                if (!mainPlayer)
                {
				    int existCount = 0;
				    for(int i = 0;i < playerExists.Length;i++){
					    if(playerExists[i]) existCount++;
				    }
				    int attackSelect = Random.Range(0,existCount);
				    if(attackSelect == 0){
					    ChangeGameStatus(4);
				    }else{
					    for(int i = 0;i < playerExists.Length;i++){
						    if(playerExists[i]) attackSelect--;
						    if(attackSelect == 0){
							    attackTarget = i;
							    d4Value = Random.Range(1,4);
							    d6Value = Random.Range(1,6);
							    attackEnemy();
							    break;
						    }
					    }
				    }
			    }else{
                    bool attackPossibility = false;
                    if (characters[playerId].findEquipment("MachineGun"))
                    {
                        attackTargetList = new List<int>();
                        for (int i = 0; i < playerExists.Length; i++)
                        {
                            if (playerExists[i])
                            {
                                attackTargetList.Add(i);
                                attackPossibility = true;
                            }
                        }
                        machineAttackButton1 = Instantiate(buttonPrefab, new Vector3(580, 160, 0), Quaternion.identity) as GameObject;
                        machineAttackButton2 = Instantiate(buttonPrefab, new Vector3(580, 60, 0), Quaternion.identity) as GameObject;
                        machineAttackButton1.transform.SetParent(buttonPanel.GetComponent<Transform>());
                        machineAttackButton2.transform.SetParent(buttonPanel.GetComponent<Transform>());
                        machineAttackButton1.GetComponent<Button>().onClick.AddListener(() => activateMachineAttackButtons(false));
                        machineAttackButton2.GetComponent<Button>().onClick.AddListener(() => activateMachineAttackButtons(false));
                        machineAttackButton1.GetComponent<Button>().onClick.AddListener(() => startMachineGun());
                        machineAttackButton2.GetComponent<Button>().onClick.AddListener(() => ChangeGameStatus(4));
                        machineAttackButton1.transform.FindChild("Text").GetComponent<Text>().text = "攻撃する";
                        machineAttackButton2.transform.FindChild("Text").GetComponent<Text>().text = "攻撃しない";
                        buttonPanel.SetActive(true);
                        machineAttackButton1.SetActive(attackPossibility);
                        machineAttackButton2.SetActive(true);
                    }
                    else
                    {
                        buttonPanel.SetActive(true);
                        attackButtons[0].SetActive(true);
                        for (int i = 0; i < playerExists.Length; i++)
                        {
                            if (playerExists[i])
                            {
                                attackButtons[i + 1].SetActive(true);
                                attackPossibility = true;
                            }
                        }
                        if (masamune && attackPossibility)
                        {
                            attackButtons[0].SetActive(false);
                        }
                    }
                }
			    break;
		    case 4:
			    Debug.Log ("finished");
                masamune = false;
                handgun = false;
                if (!wisdom)
                {
                    if (characters[playerId].fullname == CharacterState.CharacterFullName.Wight && wightCounter > 0)
                    {
                        wightCounter--;
                    }
                    else
                    {
                        playerId++;
                        playerId = playerId % playerNum;
                    }
                }
                wisdom = false;
			    ChangeGameStatus(-1);
			    break;
		    case 5:
			    Debug.Log ("game finished");
			    break;
		}
	}

	/* 出目から移動先を決定 */
	public void getStageIndex(int moveStatus){
        Debug.Log("moveStatus:" + moveStatus);
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
			if(!mainPlayer){
				moveStage(Random.Range(0,5));
			}else{
				buttonPanel.SetActive(true);
				stageButtons.SetActive(true);
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
		default:
			if(!mainPlayer){
				moveStage(Random.Range(0,5));
			}else{
				buttonPanel.SetActive(true);
				stageButtons.SetActive(true);
			}
			break;
		}

	}

	/* ステージ移動 */
	public void moveStage(int stageIndex){
        Debug.Log("stageIndex:" + stageIndex);
        stageButtons.SetActive(false);
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

    bool selecting;
    int tmppid;

	/* 老婆の庵 */
	public void drawGreenCard(){
		/* 時空の扉からのドロー時の処理 */
		drawButtons.SetActive (false);

		greens [greenCards [greenCount]].SetActive (true);
		/* オババカードを送る相手を選択*/
		buttonPanel.SetActive(true);
		activateSelectButtons(true);
		selectButtons[playerId].SetActive(false);
		selectingAction = "obaba";
		selecting = true;
	}

	public void greenCardAction(int pid){
        tmppid = pid;
		/* オババカードの各効果 */
		switch (greenCards [greenCount]+1) {
            case 1: case 14: //NかHなら装備を渡すor1ダメージ
                if (characters[pid].isHunter())
                {
                    choiseButtons.SetActive(true);
                }
                else
                {
                    greenCardAfter();
                }
                break;
            case 2: case 3: //Hなら1ダメージ
    			if(characters[pid].isHunter()){
	    			int tmpScore = playerStatesM [pid].getScore ();
		    		tmpScore++;
			    	playerStatesM[pid].moveScore (tmpScore);
			    }
                greenCardAfter();
    			break;
    		case 4: //HP>=12なら2ダメージ
			    if(characters[pid].isOver(12)){
				    int tmpScore = playerStatesM [pid].getScore ();
				    tmpScore += 2;
				    playerStatesM[pid].moveScore (tmpScore);
			    }
                greenCardAfter();
			    break;
            case 5: case 7: //HかSなら装備を渡すor1ダメージ
                if (characters[pid].isHunter() || characters[pid].isShadow())
                {
                    choiseButtons.SetActive(true);
                }
                else
                {
                    greenCardAfter();
                }
                break;
            case 6: //Sなら1ダメージ
                if (characters[pid].isShadow())
                {
				    int tmpScore = playerStatesM [pid].getScore ();
				    tmpScore++;
				    playerStatesM[pid].moveScore (tmpScore);
			    }
                greenCardAfter();
			    break;
            case 8: case 12: //NかSなら装備を渡すor1ダメージ
                if (characters[pid].isNeutral() || characters[pid].isShadow())
                {
                    choiseButtons.SetActive(true);
                }
                else
                {
                    greenCardAfter();
                }

                break;
            case 9: //Hなら1回復(それ以外なら1ダメージ)
                if (characters[pid].isHunter())
                {
                    int tmpScore = playerStatesM[pid].getScore();
                    if (tmpScore > 0) tmpScore--;
                    playerStatesM[pid].moveScore(tmpScore);
                }
                else
                {
                    int tmpScore = playerStatesM[pid].getScore();
                    tmpScore++;
                    playerStatesM[pid].moveScore(tmpScore);
                }
                greenCardAfter();
    			break;
            case 10: //Nなら1回復(それ以外なら1ダメージ)
                if (characters[pid].isNeutral())
                {
                    int tmpScore = playerStatesM[pid].getScore();
                    if (tmpScore > 0) tmpScore--;
                    playerStatesM[pid].moveScore(tmpScore);
                }
                else
                {
                    int tmpScore = playerStatesM[pid].getScore();
                    tmpScore++;
                    playerStatesM[pid].moveScore(tmpScore);
                }
                greenCardAfter();
    			break;
    		case 11: //HP<=11なら1ダメージ
			    if(characters[pid].isUnder(11)){
				    int tmpScore = playerStatesM [pid].getScore ();
				    tmpScore++;
				    playerStatesM[pid].moveScore (tmpScore);
			    }
                greenCardAfter();
			    break;
            case 13: //Sなら2ダメージ
                if (characters[pid].isShadow())
                {
				    int tmpScore = playerStatesM [pid].getScore ();
				    tmpScore += 2;
				    playerStatesM[pid].moveScore (tmpScore);
			    }
                greenCardAfter();
			    break;
            case 15: //相手にカードを見せる(実装する？)
                greenCardAfter();
                break;
            case 16: //Sなら1回復(それ以外なら1ダメージ)
                if (characters[pid].isShadow())
                {
				    int tmpScore = playerStatesM [pid].getScore ();
				    if(tmpScore > 0) tmpScore--;
				    else tmpScore++;
				    playerStatesM[pid].moveScore (tmpScore);
			    }
                greenCardAfter();
			    break;
            default:
                greenCardAfter();
            break;
		}
	}

    /* オババ終了処理 */
    public void greenCardAfter()
    {
        greens[greenCards[greenCount]].SetActive(false);
        greenCount++;
        if (greenCount == greenCards.Length)
        {
            randomize(greenCards);
            greenCount = 0;
        }
        Debug.Log("finish dgc");
        ChangeGameStatus(3);
    }


    /* ダメージを受けるか装備を渡す */

        /* 装備を渡す（未実装） */
        public void handOffEquip()
        {
            choiseButtons.SetActive(false);
            ChangeGameStatus(3);
        }
    
        /* ダメージを受ける */
        public void getDamage()
        {
            choiseButtons.SetActive(false);
            int tmpScore = playerStatesM[tmppid].getScore();
            tmpScore++;
            playerStatesM[tmppid].moveScore(tmpScore);
            ChangeGameStatus(3);
        }
	
	/* 教会 */
	public void drawWhiteCard(){
		/* 時空の扉からのドロー時の処理 */
		drawButtons.SetActive (false);
		
		whites [whiteCards [whiteCount]].SetActive (true);
		/* 白カードの各効果 */
//		switch (whiteCards [whiteCount]+1) {
        int whiteDebug = 1;
        switch (whiteDebug)
        {
            case 1:
            /* 神秘のコンパス */
                characters[playerId].addEquipment("Compass");
                drawWhiteCardAfter();
                break;
            case 2:
            /* 封印の知恵(もう一度手番を行う) */
                wisdom = true;
                drawWhiteCardAfter();
                break;
            case 3:
                /* 癒やしの聖水 */
				int tmpScore = playerStatesM [playerId].getScore ();
                if (tmpScore > 2) tmpScore -= 2;
                else tmpScore = 0;
				playerStatesM[playerId].moveScore (tmpScore);
                drawWhiteCardAfter();
                break;
            case 4:
                /* 魔除けのお守り */
                characters[playerId].addEquipment("Amulet");
                break;
            case 5:
                /* 恩恵 */
                if(mainPlayer)
                {
                    buttonPanel.SetActive(true);
                    activateSelectButtons(true);
                    selectButtons[playerId].SetActive(false);
                    selectingAction = "benefit";
                    selecting = true;
                }
                break;
            case 6:
                /* 聖者のローブ */
                characters[playerId].addEquipment("Robe");
                drawWhiteCardAfter();
                break;
            case 7:
                /* チョコレート */
                //該当キャラクターなし
                drawWhiteCardAfter();
                break;
            case 8:
                /* 銀のロザリオ */
                characters[playerId].addEquipment("Rosary");
                drawWhiteCardAfter();
                break;
            case 9:
                /* ロンギヌスの槍 */
                /* 実装検討 */
                characters[playerId].addEquipment("Longinus");
                drawWhiteCardAfter();
                break;
            case 10:
                /* 守護天使 */
                angelId = playerId; //攻撃対象のIdがangelIdならノーダメ
                drawWhiteCardAfter();
                break;
            case 11:
                /* 裁きの閃光 */
                for (int i = 0; i < playerStatesM.Length; i++)
                {
                    if (i != playerId)
                    {
                        tmpScore = playerStatesM[i].getScore();
                        tmpScore += 2;
                        playerStatesM[i].moveScore(tmpScore);
                    }
                }
                drawWhiteCardAfter();
                break;
            case 12:
                /* 応急手当 */
                selectingAction = "Aid";
                activateSelectButtons(true);
                break;
            case 13:
                /* 神秘のブローチ */
                characters[playerId].addEquipment("Brooch");
                drawWhiteCardAfter();
                break;
            case 14:
                /* 闇を祓う鏡 */
                if (characters[playerId].fullname == CharacterState.CharacterFullName.Werewolf) revealIdentity();
                drawWhiteCardAfter();
                break;
            case 15:
                /* 光臨 */
                //実装検討
                drawWhiteCardAfter();
                break;
        }
	}

    public void drawWhiteCardAfter()
    {
        whites[whiteCards[whiteCount]].SetActive(false);
        whiteCount++;
        if (whiteCount == whiteCards.Length)
        {
            randomize(whiteCards);
            whiteCount = 0;
        }
        Debug.Log("finish dwc");
        ChangeGameStatus(3);

    }


	/* 共同墓地 */
	public void drawBlackCard(){
		/* 時空の扉からのドロー時の処理 */
		drawButtons.SetActive (false);
		
		blacks [blackCards [blackCount]].SetActive (true);


		/* 黒カードの各効果 */
//		switch(blackCards[blackCount]+1){
        int blackfordebug = 15;
        switch (blackfordebug)
        {
            case 1:
                /* 妖刀マサムネ */
                characters[playerId].addEquipment("Masamune");
                drawBlackCardAfter();
                break;
            case 2:
                /* 血に飢えた大蜘蛛 */
                if (mainPlayer)
                {
                    buttonPanel.SetActive(true);
                    activateSelectButtons(true);
                    selectButtons[playerId].SetActive(false);
                    selectingAction = "bigspider";
                    selecting = true;
                }
                else
                {
                    drawBlackCardAfter();
                }
                break;
            case 3:
                /* ダイナマイト */
                diceAction = "dynamite";
                dicesActivate(true);
                break;
            case 4:
                /* 拳銃 */
                characters[playerId].addEquipment("HandGun");
                drawBlackCardAfter();
                break;
            case 5:
                /* 錆びついた大斧 */
                characters[playerId].addEquipment("bigAxe");
                drawBlackCardAfter();
                break;
            case 9:
                /* 肉切り包丁 */
                characters[playerId].addEquipment("knife");
                drawBlackCardAfter();
                break;
            case 10:
            case 12:
            case 14:
                /* 吸血コウモリ */
                if (mainPlayer)
                {
                    buttonPanel.SetActive(true);
                    activateSelectButtons(true);
                    selectButtons[playerId].SetActive(false);
                    selectingAction = "bat";
                    selecting = true;
                }
                else
                {
                    drawBlackCardAfter();
                }
                break;
            case 13:
                /* チェーンソー */
                characters[playerId].addEquipment("ChainSaw");
                drawBlackCardAfter();
                break;
            case 15:
                /* 機関銃 */
                characters[playerId].addEquipment("MachineGun");
                drawBlackCardAfter();
                break;
            case 16:
                selectingAction = "doll";
                activateSelectButtons(true);
                selectButtons[playerId].SetActive(false);
                break;
            default:
                drawBlackCardAfter();
                break;
        }
	}

	public void drawBlackCardAfter(){
		blacks [blackCards [blackCount]].SetActive (false);
		blackCount++;
		if (blackCount == blackCards.Length) {
			randomize (blackCards);
			blackCount = 0;
		}
		Debug.Log ("finish dbc");
		ChangeGameStatus(3);
	}


	/* 時空の扉 */
	void drawFreeCard(){
		if (!mainPlayer) {
			switch(Random.Range (0,2)){
			case 0:
				drawBlackCard();
				break;
			case 1:
				drawGreenCard();
				break;
			case 2:
				drawWhiteCard();
				break;
			}
		} else {
			buttonPanel.SetActive (true);
			drawButtons.SetActive (true);
		}
	}

	/* 希望と絶望の森 */
	void hopeAndDespair(){
        selectingAction = "hopeAndDespair";
        activateSelectButtons(true);
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

    public void attackEnemy()
    {
        int robeEffect = 0;
        if (attackTarget != angelId)
        {
            if (characters[playerId].findEquipment("Robe") || characters[attackTarget].findEquipment("Robe"))
            {
                robeEffect = 1;
            }
            Debug.Log("Damage:" + (Mathf.Abs(d6Value - d4Value) - robeEffect + characters[playerId].attackPower) + " d6:" + d6Value + " d4:" + d4Value);
            if (playerStatesM[attackTarget].getDamage(Mathf.Abs(d6Value - d4Value) - robeEffect + characters[playerId].attackPower)) //攻撃で死んだ場合
            {
                if (characters[playerId].findEquipment("Rosary")) //銀のロザリオの特殊効果
                {
                    foreach (string equip in characters[attackTarget].getEquipList())
                    {
                        characters[playerId].addEquipment(equip);
                    }

                }
                ChangeGameStatus(4);
            }
            else if (characters[attackTarget].fullname == CharacterState.CharacterFullName.Werewolf && characters[attackTarget].reveal)
            {
                revengeReady(attackTarget, playerId);
            }
            else
            {
                ChangeGameStatus(4);
            }
        }
        else
        {
            ChangeGameStatus(4);
        }
    }
    
    public void machineGunAttack()
    {
        Debug.Log("Machine Gun Start!");
        Debug.Log(attackTargetList);
        bool revengeOccur = false;
        foreach (int target in attackTargetList)
        {
            int robeEffect = 0;
            if (target != angelId)
            {
                if (characters[playerId].findEquipment("Robe") || characters[target].findEquipment("Robe"))
                {
                    robeEffect = 1;
                }
                Debug.Log("Damage:" + (Mathf.Abs(d6Value - d4Value) - robeEffect + characters[playerId].attackPower) + " d6:" + d6Value + " d4:" + d4Value);
                if (playerStatesM[target].getDamage(Mathf.Abs(d6Value - d4Value) - robeEffect + characters[playerId].attackPower)) //攻撃で死んだ場合
                {
                    if (characters[playerId].findEquipment("Rosary")) //銀のロザリオの特殊効果
                    {
                        foreach (string equip in characters[target].getEquipList())
                        {
                            characters[playerId].addEquipment(equip);
                        }

                    }
                }
                else if (characters[target].fullname == CharacterState.CharacterFullName.Werewolf && characters[target].reveal)
                {
                    revengeOccur = true;
                }
            }
        }
        machineGun = false;
        if (revengeOccur)
        {
            revengeReady(attackTarget, playerId);
        }
        else
        {
            ChangeGameStatus(4);
        }
    }

	public void activateAttackButtons(bool flag){
		foreach(GameObject obj in attackButtons){
			obj.SetActive(flag);
		}
	}
	public void activateSelectButtons(bool flag){
        for (int i = 0; i < selectButtons.Length; i++)
        {
            if (!playerStatesM[i].dead) selectButtons[i].SetActive(flag);
        }
        buttonPanel.SetActive(flag);
	}

    public void activateMachineAttackButtons(bool flag)
    {
        machineAttackButton1.SetActive(flag);
        machineAttackButton2.SetActive(flag);
        buttonPanel.SetActive(flag);
    }
    public void startMachineGun()
    {
        machineGun = true;
        diceAction = "MachineGun";
        dicesActivate(true);
    }

	public void selectCharacter(int id){
        Debug.Log("id:" + id + " selectingAction:" + selectingAction);
		switch (selectingAction) {
		    case "bigspider":
			    buttonPanel.SetActive (false);
                playerStatesM[id].getDamage(2);
                playerStatesM[playerId].getDamage(2);
			    drawBlackCardAfter ();
			    break;
		    case "bat":
			    buttonPanel.SetActive (false);
                playerStatesM[id].getDamage(2);
                playerStatesM[playerId].getDamage(-1);
			    drawBlackCardAfter ();
			    break;
		    case "obaba":
			    buttonPanel.SetActive (false);
			    greenCardAction (id);
			    break;
            case "benefit":
                tmppid = id;
                diceAction = "benefit";
			    dicesActivate (true);
                break;
            case "George":
                tmppid = id;
                diceAction = "George";
                buttonPanel.SetActive(true);
			    dicesActivate (true);
                break;
            case "Fuka":
                playerStatesM[id].moveScore(7);
                ChangeGameStatus(0);
                break;
            case "hopeAndDespair":
                if (id == playerId)
                {
                    playerStatesM[id].getDamage(-1);
                }
                else
                {
                    if (!characters[id].findEquipment("Brooch")) playerStatesM[id].getDamage(2);
                }
                ChangeGameStatus(3);
                break;
            case "Aid":
                playerStatesM[id].moveScore(7);
                drawWhiteCardAfter();
                break;
            case "doll":
                tmppid = id;
                diceAction = "doll";
                dicesActivate(true);
                break;
		}
        selectingAction = "";
        selecting = false;
	}

	IEnumerator waitSelect(){
		while (selecting) {
			yield return new WaitForSeconds(1.0f);
		}
		Debug.Log ("selecting finish");
		switch (selectingAction) {
		case "bigspider":
			drawBlackCardAfter ();
			break;
		}
		Debug.Log ("after finish");
	}


    /*                                */
    /*　     ダイス処理ここから　     */
    /*                                */

    public void checkPlayerExist()
    {
        for (int i = 0; i < playerExists.Length; i++)
        {
            playerExists[i] = false;
        }
        for (int i = 0; i < playersPositions.Length; i++)
        {
            if (!handgun)
            {
                if (i != playerId && playersPositions[i] / 2 == playersPositions[playerId] / 2 && !playerStatesM[i].dead)
                {
                    playerExists[i] = true;
                }
            }
            else
            {
                if (i != playerId && playersPositions[i] / 2 != playersPositions[playerId] / 2 && !playerStatesM[i].dead)
                {
                    playerExists[i] = true;
                }
            }
        }
    }
    public void selectDiceAction(){
        string tmpDiceAction = diceAction;
        diceAction = "";
        switch (tmpDiceAction)
        {
            case "benefit":
                playerStatesM[tmppid].getDamage(-1 * d6Value);
                drawWhiteCardAfter();
                break;
            case "George":
                playerStatesM[tmppid].getDamage(d4Value);
                ChangeGameStatus(0);
                break;
            case "werewolf":
                revenge();
                break;
            case "dynamite":
                int area = -1;
                switch (d4Value + d6Value)
                {
                    case 2: case 3:
                        area = 0;
                        break;
                    case 4: case 5:
                        area = 1;
                        break;
                    case 6:
                        area = 2;
                        break;
                    case 7:
                        area = -1;
                        break;
                    case 8:
                        area = 3;
                        break;
                    case 9:
                        area = 4;
                        break;
                    case 10:
                        area = 5;
                        break;
                }
                for (int i = 0; i < playersPositions.Length; i++)
                {
                    if (area >= 0)
                    {
                        if (playersPositions[i] == stages[area])
                        {
                            playerStatesM[i].getDamage(3);
                        }
                    }
                }
                drawBlackCardAfter();
                break;
            case "doll":
                if (d6Value >= 5)
                {
                    playerStatesM[playerId].getDamage(3);
                }
                else
                {
                    playerStatesM[tmppid].getDamage(3);
                }
                drawBlackCardAfter();
                break;
            case "MachineGun":
                machineGunAttack();
                break;
        }
    }
	

	public IEnumerator getDiceValue(){
        Debug.Log("Get Dice Value start!(" + playerId + ")");
		d6Value = 0;
		d4Value = 0;
		while (d6.GetComponent<Die>().rolling || d4.GetComponent<Die>().rolling) {
			yield return new WaitForSeconds (2.0f);
		}
		d6Value = d6.GetComponent<Die_d6>().value;
		d4Value = d4.GetComponent<Die_d6>().value;
		d4Value = d4Value % 4 + 1;
        if (masamune && gameStatus == 3) d6Value = 0;
		dicesActivate (false);
        diceroll.rolling = false;
        Debug.Log("d6:" + d6Value + " d4:" + d4Value);
        Debug.Log("Compass : " + compass + "(" + playerId + ")");
        if (diceAction != "")
        {
            Debug.Log("dice action");
            selectDiceAction();
        }
        else
        {
            Debug.Log("not diceaction");
            if (gameStatus == 0)
            {
                if (compass)
                {
                    if (compass_first == -1)
                    {
                        compass_first = d6Value + d4Value;
                        dicesActivate(true);
                    }
                    else
                    {
                        compass_second = d6Value + d4Value;
                        compassActivate(true);
                    }
                }
                else
                {
                    /* debug用にd4,d6を設定 */
                    d4Value = 2;
                    d6Value = 6;
                    ChangeGameStatus(2);
                }
            }
            else if (gameStatus == 3)
            {
                attackEnemy();
            }
        }

        yield break;
	}

    IEnumerator waitSeconds(int sec){
		Debug.Log ("start waiting");
//		Time.timeScale = 0.0f;
		yield return new WaitForSeconds (sec);
		Time.timeScale = 1.0f;
		Debug.Log ("finish waiting");
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
        if (!(selectingAction == "George" || selectingAction == "doll" ))
        {
            d4.SetActive(flag);
            d4.GetComponent<Rigidbody>().Sleep();
        }
        if (!(masamune || selectingAction == "benefit"))
        {
            d6.SetActive(flag);
            d6.GetComponent<Rigidbody>().Sleep();
        }
        buttonPanel.SetActive(flag);
		diceButton.SetActive(flag);
	}

    void compassActivate(bool flag)
    {
        buttonPanel.SetActive(flag);
        firstButton.SetActive(flag);
        secondButton.SetActive(flag);
        firstButton.transform.FindChild("Text").GetComponent<Text>().text = compass_first.ToString();
        secondButton.transform.FindChild("Text").GetComponent<Text>().text = compass_second.ToString();
    }

    public void selectCompass(int num)
    {
        compassActivate(false);
        if (num == 1)
        {
            getStageIndex(compass_first);
        }
        else
        {
            getStageIndex(compass_second);
        }
        compass_first = -1;
    }

    /* 正体公開 */
    public void activateOpenButtons(bool flag)
    {
        buttonPanel.SetActive(flag);
        openButton1.SetActive(flag);
        openButton2.SetActive(flag);
    }

    public void revealIdentity()
    {
        characters[playerId].reveal = true;
        if (characters[playerId].name == CharacterState.CharacterName.G) //Georgeの特殊効果
        {
            activateSelectButtons(true);
            selectButtons[playerId].SetActive(false);
            selectingAction = "George";
            selecting = true;
        }
        else if (characters[playerId].name == CharacterState.CharacterName.F) //Fukaの特殊能力
        {
            activateSelectButtons(true);
            selectButtons[playerId].SetActive(false);
            selectingAction = "Fuka";
            selecting = true;
        }
        else if(characters[playerId].fullname == CharacterState.CharacterFullName.Wight) //Wightの特殊能力
        {
            for (int i = 0; i < playerNum; i++)
            {
                if (i == playerId) continue;
                if (playerStatesM[i].dead) wightCounter++;
            }
            dicesActivate(true);
            ChangeGameStatus(0);
        }
        else
        {
            dicesActivate(true);
            ChangeGameStatus(0);
        }
    }

    int wolfId, wolfTarget;

    public void revengeReady(int wolfId, int targetId) //werewolfの反撃
    {
        Debug.Log("Revenge start");
        wolfId = this.wolfId;
        wolfTarget = targetId;
        diceAction = "werewolf";
        dicesActivate(true);
    }
    public void revenge()
    {
        int robeEffect = 0;
        if (wolfTarget != angelId)
        {
            if (characters[wolfId].findEquipment("Robe") || characters[wolfTarget].findEquipment("Robe"))
            {
                robeEffect = 1;
            }
            if (playerStatesM[wolfTarget].getDamage(Mathf.Abs(d6Value - d4Value) - robeEffect + characters[wolfId].attackPower)) //攻撃で死んだ場合
            {
                if (characters[playerId].findEquipment("Rosary")) //銀のロザリオの特殊効果
                {
                    foreach (string equip in characters[attackTarget].getEquipList())
                    {
                        characters[playerId].addEquipment(equip);
                    }

                }

            }
        }
        ChangeGameStatus(4);
    }

    //ここから追加
    /*フェードイン・アウトの線画*/
    public void OnGUI()
    {

        // Fade .
        if (this.isFading)
        {
            //色と透明度を更新して白テクスチャを描画 .
            this.fadeColor.a = this.fadeAlpha;
            GUI.color = this.fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
    }

    //フェードイン・アウト処理
    private IEnumerator TransCamera(float interval)
    {
        //だんだん暗く .
        this.isFading = true;
        float time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        ChangeCamera();

        //だんだん明るく .
        time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        this.isFading = false;

        GameObject faze = Instantiate(fazeEffect) as GameObject;

        Destroy(faze, 2.0f);
    }

    //カメラ切り替え
    void ChangeCamera()
    {
        
        if(mainCamera.enabled)
        {

            mainCamera.enabled = false;

            subCamera.enabled = true;
        }
        else
        {

            mainCamera.enabled = true;

            subCamera.enabled = false;
        }
    }

	// Update is called once per frame
	void Update () 
    {
	    
        //デバッグ用zキーでカメラの切り替え
        if(Input.GetKeyDown("z"))
        {
            //ChangeCamera();
            /*
            GameObject faze = Instantiate(fazeEffect) as GameObject;

            Destroy(faze, 2.0f);
             */
        }
	}
}
