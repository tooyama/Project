using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class SHManeger : MonoBehaviour {

	public int gameStatus;

	public static int playerNum = 5;

    public GameObject fazeEffect;

    public GameObject smokeEffect;

    public GameObject catherine;

	public GameObject d4,d6,diceButton,PlayerPanels,Monster,EffectPanels,Enemy,buttonPanel,character,buttonPrefab;

    public Camera mainCamera,subCamera;

    public DiceRoll diceroll;

	int d4Value,d6Value,greenCount,whiteCount,blackCount;

	int[] stages = new int[6];
	int[] greenCards = new int[16];
	int[] whiteCards = new int[15];
	int[] blackCards = new int[16];

	GameObject[] greens,whites,blacks,effects,attackButtons,selectButtons,playerStates,monsters,equipButtons,choiseButtonArray,hdButtons;

	CharacterState[] characters;

    GameObject stageButtons, drawButtons, choiseButtons, firstButton, secondButton, openButton1, openButton2,machineAttackButton1, machineAttackButton2;

	Transform[] panelPositions;

    PlayerStateManager[] playerStatesM;

	int[] playersPositions = new int[playerNum];
	bool[] playerExists = new bool[playerNum];
    bool[] playerWin = new bool[playerNum];

	int playerId,attackTarget,angelId = -1,compass_first = -1,compass_second,wightCounter = 0;

    bool masamune,handgun,wisdom,ritual = false;

    bool mainPlayer = true;

    List<int> attackTargetList,attackedTarget;

    /* ボタン座標用の変数(後で調整してください) */
    int buttonX = 580, buttonY = 160, buttonZ = 0, xGap = 0, yGap = -100, zGap = 0;

    string currentAction = "";

    /* AI用変数 */
    /* 各種予測値 */
    double[,] predictH = new double[playerNum,playerNum];
    double[,] predictN = new double[playerNum, playerNum];
    double[,] predictS = new double[playerNum, playerNum];
    int[,] playerData = new int[playerNum, playerNum];
    const int neutralData = 0;
    const int hunterData = 1;
    const int shadowData = 2;
    int turn = 0; //ターン数を記録
    int mainPlayerId;

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
        setPredict();
		playerId = 0;
        mainPlayerId = UnityEngine.Random.Range(0, playerNum - 1);

        Debug.Log("main player id = " + mainPlayerId);

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
        /* ダメージor装備ボタン */
        choiseButtonArray = GameObject.FindGameObjectsWithTag("choiseButtons");

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
        firstButton = Instantiate(buttonPrefab, new Vector3(buttonX,buttonY,buttonZ), Quaternion.identity) as GameObject;
        secondButton = Instantiate(buttonPrefab, new Vector3(buttonX, buttonY+yGap, buttonZ), Quaternion.identity) as GameObject;
        firstButton.transform.SetParent(buttonPanel.GetComponent<Transform>());
        secondButton.transform.SetParent(buttonPanel.GetComponent<Transform>());
        firstButton.GetComponent<Button>().onClick.AddListener(() => selectCompass(1));
        secondButton.GetComponent<Button>().onClick.AddListener(() => selectCompass(2));
        firstButton.SetActive(false);
        secondButton.SetActive(false);

        /* 正体公開用ボタン */
        openButton1 = Instantiate(buttonPrefab, new Vector3(buttonX, buttonY, buttonZ), Quaternion.identity) as GameObject;
        openButton2 = Instantiate(buttonPrefab, new Vector3(buttonX, buttonY+yGap, buttonZ), Quaternion.identity) as GameObject;
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
			int randomIndex = UnityEngine.Random.Range (0,characters.Length);
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

    /* 予測値初期化 */
    void setPredict()
    {
        for (int i = 0; i < playerNum; i++)
        {
            if (characters[i].isHunter())
            {
                playerData[i, i] = hunterData;
                for (int j = 0; j < playerNum; j++)
                {
                    if (j == i) continue;
                    playerData[i, j] = -1;
                    predictH[i, j] = 1.0;
                    predictN[i, j] = 1.0;
                    predictS[i, j] = 1.0;
                }
            }
            else if (characters[i].isShadow())
            {
                playerData[i, i] = shadowData;
                for (int j = 0; j < playerNum; j++)
                {
                    if (j == i) continue;
                    playerData[i, j] = -1;
                    predictH[i, j] = 1.0;
                    predictN[i, j] = 1.0;
                    predictS[i, j] = 1.0;
                }
            }
            else
            {
                playerData[i, i] = neutralData;
                for (int j = 0; j < playerNum; j++)
                {
                    if (j == i) continue;
                    playerData[i, j] = -1;
                    predictH[i, j] = 1.0;
                    predictN[i, j] = 1.0;
                    predictS[i, j] = 1.0;
                }
            }

        }
    }

    void deletePredicts(int playerId, int enemyId)
    {
        predictH[playerId, enemyId] = 0;
        predictN[playerId, enemyId] = 0;
        predictS[playerId, enemyId] = 0;
    }

    /* 配列のシャッフル(補助関数) */
    void randomize(int[] array)
    {
		for (int i = 0; i<array.Length; i++) {
			array[i] = i;
		}
		for (int i = 0; i<array.Length; i++) {
			int temp = array[i];
			int randomIndex = UnityEngine.Random.Range (0,array.Length);
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
        if (playerId == mainPlayerId) mainPlayer = true;
        else mainPlayer = false;
        
        Debug.Log("ChangeGameStatus:" + status + " playerId:" + playerId + "mainPlayer:" + mainPlayer + "(" + DateTime.Now + ")");
		gameStatus = status;
		switch (status) {
            case -1:
                Debug.Log("Debug predicts");
                string preH = "",preN="",preS = "";
                for (int i = 0; i < playerNum; i++)
                {
                    preH += " " + predictH[playerId, i];
                    preN += " " + predictN[playerId, i];
                    preS += " " + predictS[playerId, i];
                }
                Debug.Log("preH(" + playerId + "):" + preH);
                Debug.Log("preN(" + playerId + "):" + preN);
                Debug.Log("preS(" + playerId + "):" + preS);
                if (playerStatesM[playerId].dead)
                {
                    ChangeGameStatus(4);
                }
                if (!characters[playerId].reveal)
                {
                    if (mainPlayer)
                    {
                        activateOpenButtons(true);
                    }
                    else
                    {
                        if (revealDecision())
                        {
                            revealIdentity();
                        }
                        else
                        {
                            ChangeGameStatus(0);
                        }
                    }
                }
                else
                {
                    ChangeGameStatus(0);
                }
                turn++;
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
                StartCoroutine(TransCamera(2.0f));
                compass = characters[playerId].findEquipment("Compass");

                /* ダイスのアクティブ化 */
                dicesActivate(true);
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
                    if (characters[playerId].findEquipment("MachineGun"))
                    {
                        Debug.Log("Machine Gun!");
                        diceAction = "MachineGun";
                        bool attackPossibility = false;
                        attackTargetList = new List<int>();
                        for (int i = 0; i < playerExists.Length; i++)
                        {
                            if (playerExists[i])
                            {
                                attackTargetList.Add(i);
                                attackPossibility = true;
                            }
                        }
                        if (attackPossibility) dicesActivate(true);
                        else ChangeGameStatus(4);
                    }
                    else autoAttackSelect(masamune);
                }
                else
                {
                    if (characters[playerId].findEquipment("MachineGun")) machineGunReady();
                    else setAttackButtons(masamune);
                }
			    break;
		    case 4:
			    Debug.Log ("finished");
                if (checkWinner())
                {
                    ChangeGameStatus(5);
                }
                else
                {
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
                }
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
				moveStage(UnityEngine.Random.Range(0,5));
			}else{
                activateStageButtons(true);
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
				moveStage(UnityEngine.Random.Range(0,5));
			}else{
                activateStageButtons(true);
			}
			break;
		}

	}
    /* ステージボタン */
    public void activateStageButtons(bool flag)
    {
        buttonPanel.SetActive(flag);
        stageButtons.SetActive(flag);
    }

	/* ステージ移動 */
	public void moveStage(int stageIndex){
        Debug.Log("stageIndex:" + stageIndex);
        stageButtons.SetActive(false);
		//monsters[playerId].transform.position = panelPositions[stages[stageIndex]].position;
        iTween.MoveTo(monsters[playerId], panelPositions[stages[stageIndex]].transform.position, 0.5f);
		playersPositions [playerId] = stages [stageIndex];
		switch (stageIndex) {
		    case 0:
                currentAction = "Green";
			    drawGreenCard();
			    break;
		    case 1:
			    drawFreeCard ();
			    break;
		    case 2:
                currentAction = "White";
			    drawWhiteCard();
			    break;
		    case 3:
                currentAction = "Black";
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
        if (mainPlayer)
        {
            activateSelectButtons(true);
            selectButtons[playerId].SetActive(false);
            selectingAction = "obaba";
            selecting = true;
        }
        else
        {
            greenCardAction(makeObabaTarget());
        }
	}

	public void greenCardAction(int pid){
        tmppid = pid;
        currentAction = "Green";
        /* オババ効果を予測値に追加 */
        switch (greenCards[greenCount] + 1)
        {
            case 1:
            case 14: //NかHなら装備を渡すor1ダメージ
                if (characters[pid].isHunter() || characters[pid].isNeutral())
                {
                    predictN[playerId, pid] *= 1.5;
                    predictH[playerId, pid] *= 1.5;
                    predictS[playerId, pid] = 0;
                }
                else
                {
                    playerData[playerId, pid] = shadowData;
                    deletePredicts(playerId,pid);
                }
                break;
            case 2:
            case 3: //Hなら
            case 9: 
                if (characters[pid].isHunter())
                {
                    playerData[playerId, pid] = hunterData;
                }
                else
                {
                    predictH[playerId, pid] = 0;
                    predictN[playerId, pid] *= 1.5;
                    predictS[playerId, pid] *= 1.5;
                }
                break;
            case 4: //HP>=12なら2ダメージ
                if (characters[pid].isOver(12))
                {
                    predictN[playerId, pid] = 0;
                }
                else
                {
                    playerData[playerId, pid] = neutralData;
                    deletePredicts(playerId, pid);
                }
                break;
            case 5:
            case 7: //HかSなら装備を渡すor1ダメージ
                if (characters[pid].isHunter() || characters[pid].isShadow())
                {
                    predictH[playerId, pid] *= 1.5;
                    predictS[playerId, pid] *= 1.5;
                    predictN[playerId, pid] = 0;
                }
                else
                {
                    playerData[playerId, pid] = neutralData;
                    deletePredicts(playerId, pid);
                }
                break;
            case 6: //Sなら
            case 13:
            case 16:
                if (characters[pid].isShadow())
                {
                    playerData[playerId, pid] = shadowData;
                    deletePredicts(playerId, pid);
                }
                else
                {
                    predictH[playerId, pid] *= 1.5;
                    predictN[playerId, pid] *= 1.5;
                }
                break;
            case 8:
            case 12: //NかSなら装備を渡すor1ダメージ
                if (characters[pid].isNeutral() || characters[pid].isShadow())
                {
                    predictN[playerId, pid] *= 1.5;
                    predictS[playerId, pid] *= 1.5;
                    predictH[playerId, pid] = 0;
                }
                else
                {
                    playerData[playerId, pid] = hunterData;
                    deletePredicts(playerId, pid);
                }
                break;
            case 10: //Nなら1回復(それ以外なら1ダメージ)
                if (characters[pid].isNeutral())
                {
                    playerData[playerId, pid] = neutralData;
                    deletePredicts(playerId, pid);
                }
                else
                {
                    predictH[playerId, pid] *= 1.5;
                    predictS[playerId, pid] *= 1.5;
                    predictN[playerId, pid] = 0;
                }
                break;
            case 11: //HP<=11なら1ダメージ
                if (characters[pid].isUnder(11))
                {
                    playerData[playerId, pid] = neutralData;
                    deletePredicts(playerId, pid);
                }
                else
                {
                    predictN[playerId, pid] = 0;
                }

                break;
            case 15: //相手にカードを見せる(実装する？)
                break;
            default:
                break;
        }
        /* オババカードの各効果 */
		switch (greenCards [greenCount]+1) {
            case 1: case 14: //NかHなら装備を渡すor1ダメージ
                if (characters[pid].isHunter()) activateChoiseButtons(true);
                else greenCardAfter();
                break;
            case 2: case 3: //Hなら1ダメージ
    			if(characters[pid].isHunter()) playerStatesM[pid].getDamage(1);
                greenCardAfter();
    			break;
    		case 4: //HP>=12なら2ダメージ
			    if(characters[pid].isOver(12)) playerStatesM[pid].getDamage(2);
                greenCardAfter();
			    break;
            case 5: case 7: //HかSなら装備を渡すor1ダメージ
                if (characters[pid].isHunter() || characters[pid].isShadow()) activateChoiseButtons(true);
                else greenCardAfter();
                break;
            case 6: //Sなら1ダメージ
                if (characters[pid].isShadow()) playerStatesM[pid].getDamage(1);
                greenCardAfter();
			    break;
            case 8: case 12: //NかSなら装備を渡すor1ダメージ
                if (characters[pid].isNeutral() || characters[pid].isShadow()) activateChoiseButtons(true);
                else greenCardAfter();
                break;
            case 9: //Hなら1回復(それ以外なら1ダメージ)
                if (characters[pid].isHunter()) playerStatesM[pid].getDamage(-1);
                else playerStatesM[pid].getDamage(1);
                greenCardAfter();
    			break;
            case 10: //Nなら1回復(それ以外なら1ダメージ)
                if (characters[pid].isNeutral()) playerStatesM[pid].getDamage(-1);
                else playerStatesM[pid].getDamage(1);
                greenCardAfter();
    			break;
    		case 11: //HP<=11なら1ダメージ
			    if(characters[pid].isUnder(11)) playerStatesM[pid].getDamage(1);
                greenCardAfter();
			    break;
            case 13: //Sなら2ダメージ
                if (characters[pid].isShadow()) playerStatesM[pid].getDamage(2);
                greenCardAfter();
			    break;
            case 15: //相手にカードを見せる(実装する？)
                greenCardAfter();
                break;
            case 16: //Sなら1回復(それ以外なら1ダメージ)
                if (characters[pid].isShadow()) playerStatesM[pid].getDamage(-1);
                else playerStatesM[pid].getDamage(1);
                greenCardAfter();
			    break;
            default:
                greenCardAfter();
            break;
		}
	}

    int makeObabaTarget()
    {
        int obabaTarget = -1;
        double minPredict = double.MaxValue;
        for (int i = 0; i < playerNum; i++)
        {
            if (i == playerId) continue;
            double sum_m, sum_v;
            sum_m = predictH[playerId,i] + predictN[playerId,i]+predictS[playerId,i];
            sum_v = predictH[playerId,i]*predictH[playerId,i] + predictN[playerId,i]*predictN[playerId,i]+predictS[playerId,i]*predictS[playerId,i];
            double variance = (sum_v / 3.0) - (sum_m * sum_m / 9.0);
            if (variance > 0 && variance < minPredict)
            {
                obabaTarget = i;
            }
        }
        if (obabaTarget == -1)
        {
            int randomTarget = UnityEngine.Random.Range(0, playerNum - 2);
            if (randomTarget >= playerId) randomTarget++;
            obabaTarget = randomTarget;
        }
        return obabaTarget;
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
        if (checkWinner())
        {
            ChangeGameStatus(5);
        }
        else
        {
            ChangeGameStatus(3);
        }
    }


    /* ダメージを受けるか装備を渡す */

        /* ボタンのactivate */
    public void activateChoiseButtons(bool flag)
    {
        if (tmppid == mainPlayerId)
        {
            buttonPanel.SetActive(flag);
            choiseButtons.SetActive(flag);
            Debug.Log("Choise Button Array Length:" + choiseButtonArray.Length + " flag:" + flag);
            if (flag)
            {
                for (int i = 0; i < choiseButtonArray.Length; i++)
                {
                    Debug.Log("Text:" + choiseButtonArray[i].transform.FindChild("Text").GetComponent<Text>().text + " not hasEquip" + !characters[tmppid].hasEquip() + " equipCount:" + characters[tmppid].getEquipList().Count);
                    if (choiseButtonArray[i].transform.FindChild("Text").GetComponent<Text>().text == "装備を渡す")
                    {
                        choiseButtonArray[i].SetActive(characters[tmppid].hasEquip());
                    }
                }
            }
        }
        else
        {
            /* ダメージを受ける動作に限定 */
            getDamage();
        }
    }

    /* 装備を渡す */
    public void equipHandoffSelect(bool fromPlayer)
    {
        int fromId;
        if (fromPlayer) fromId = playerId;
        else fromId = tmppid;
        choiseButtons.SetActive(false);
        if (fromId == mainPlayerId)
        {
            buttonPanel.SetActive(true);
            equipButtons = new GameObject[characters[fromId].getEquipList().Count];
            int count = 0;
            foreach (string equip in characters[fromId].getEquipList())
            {
                equipButtons[count] = Instantiate(buttonPrefab, new Vector3(buttonX + count * xGap, buttonY + count * yGap + buttonZ + count * zGap), Quaternion.identity) as GameObject;
                equipButtons[count].transform.SetParent(buttonPanel.GetComponent<Transform>());
                equipButtons[count].GetComponent<Button>().onClick.AddListener(() => handoffEquip(equip, fromPlayer));
                equipButtons[count].GetComponent<Button>().onClick.AddListener(() => activateEquipButtons(false));
                equipButtons[count].transform.FindChild("Text").GetComponent<Text>().text = equip;
                count++;
            }
            activateEquipButtons(true);
        }
        else
        {
            List<string> equipList = characters[fromId].getEquipList();
            int equipLength = equipList.Count;
            handoffEquip(equipList[UnityEngine.Random.Range(0, equipLength - 1)],fromPlayer);
        }
    }
    public void activateEquipButtons(bool flag)
    {
        for (int i = 0; i < equipButtons.Length; i++)
        {
            equipButtons[i].SetActive(flag);
        }
    }

    public void handoffEquip(string equip,bool fromPlayer)
    {
        int fromId, toId;
        if (fromPlayer)
        {
            fromId = playerId;
            toId = tmppid;
        }
        else
        {
            fromId = tmppid;
            toId = playerId;
        }
        characters[fromId].removeEquipment(equip);
        characters[toId].addEquipment(equip);
        switch (currentAction)
        {
            case "Green":
                greenCardAfter();
                break;
            case "Black":
                drawBlackCardAfter();
                break;
            case "Alter":
                ChangeGameStatus(3);
                break;
        }
    }
    
        /* ダメージを受ける */
        public void getDamage()
        {
            playerStatesM[tmppid].getDamage(1);
            greenCardAfter();
        }
	
	/* 教会 */
	public void drawWhiteCard(){
		/* 時空の扉からのドロー時の処理 */
		drawButtons.SetActive (false);
        Debug.Log("Draw White Card No." + (whiteCards[whiteCount] + 1));		
		whites [whiteCards [whiteCount]].SetActive (true);
		/* 白カードの各効果 */
		switch (whiteCards [whiteCount]+1)
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
                playerStatesM[playerId].getDamage(-2);
                drawWhiteCardAfter();
                break;
            case 4:
                /* 魔除けのお守り */
                characters[playerId].addEquipment("Amulet");
                drawWhiteCardAfter();
                break;
            case 5:
                /* 恩恵 */
                selectingAction = "benefit";
                if (mainPlayer)
                {
                    activateSelectButtons(true);
                    selectButtons[playerId].SetActive(false);
                    selecting = true;
                }
                else
                {
                    selectCharacter(makeTarget(true));
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
                /* 守護天使 */
                angelId = playerId; //攻撃対象のIdがangelIdならノーダメ
                drawWhiteCardAfter();
                break;
            case 10:
                /* 裁きの閃光 */
                for (int i = 0; i < playerStatesM.Length; i++)
                {
                    if (i != playerId)
                    {
                        playerStatesM[i].getDamage(2);
                    }
                }
                drawWhiteCardAfter();
                break;
            case 11:
                /* 応急手当 */
                selectingAction = "Aid";
                if (mainPlayer)
                {
                    activateSelectButtons(true);
                }
                else
                {
                    selectCharacter(makeTarget(true));
                }
                break;
            case 12:
                /* 神秘のブローチ */
                characters[playerId].addEquipment("Brooch");
                drawWhiteCardAfter();
                break;
            case 13:
                /* 闇を祓う鏡 */
                if (characters[playerId].fullname == CharacterState.CharacterFullName.Werewolf) revealIdentity();
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

        Debug.Log("Draw Black Card No." + (blackCards[blackCount] + 1));

		/* 黒カードの各効果 */
        switch (blackCards[blackCount] + 1)
        {
            case 1:
                /* 妖刀マサムネ */
                characters[playerId].addEquipment("Masamune");
                drawBlackCardAfter();
                break;
            case 2:
                /* 血に飢えた大蜘蛛 */
                selectingAction = "bigspider";
                if (mainPlayer)
                {
                    activateSelectButtons(true);
                    selectButtons[playerId].SetActive(false);
                    /* 魔除けのお守り効果 */
                    for (int i = 0; i < playerNum; i++)
                    {
                        if (characters[i].findEquipment("Amulet")) selectButtons[i].SetActive(false);
                    }
                    selecting = true;
                }
                else
                {
                    selectCharacter(makeTarget(false));
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
            case 6:
                /* バナナの皮 */
                selectingAction = "banana";
                if (characters[playerId].getEquipList().Count == 0) playerStatesM[playerId].getDamage(1);
                else
                {
                    if (mainPlayer)
                    {
                        activateSelectButtons(true);
                        selectButtons[playerId].SetActive(false);
                        selecting = true;
                    }
                    else
                    {
                        selectCharacter(makeTarget(false));
                    }
                }
                drawBlackCardAfter();
                break;
            case 7:
            case 11:
                /* 気まぐれな小悪魔 */
                if (mainPlayer)
                {
                    bool canRob = false;
                    for (int i = 0; i < playerNum; i++)
                    {
                        if (i != playerId && characters[i].hasEquip())
                        {
                            canRob = true;
                            break;
                        }
                    }
                    if (canRob)
                    {
                        buttonPanel.SetActive(true);
                        for (int i = 0; i < playerNum; i++)
                        {
                            if (characters[i].hasEquip()) selectButtons[i].SetActive(true);
                        }
                        selectButtons[playerId].SetActive(false);
                        selectingAction = "devil";
                    }
                }
                else
                {
                    drawBlackCardAfter();
                }
                break;
            case 8:
                /* 戦慄の闇儀式 */
                activateOpenButtons(true);
                ritual = true;
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
                selectingAction = "bat";
                if (mainPlayer)
                {
                    activateSelectButtons(true);
                    selectButtons[playerId].SetActive(false);
                    /* 魔除けのお守り効果 */
                    for (int i = 0; i < playerNum; i++)
                    {
                        if (characters[i].findEquipment("Amulet")) selectButtons[i].SetActive(false);
                    }
                    selecting = true;
                }
                else
                {
                    selectCharacter(makeTarget(false));
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
                if (mainPlayer)
                {
                    activateSelectButtons(true);
                    selectButtons[playerId].SetActive(false);
                }
                else
                {
                    selectCharacter(makeTarget(false));
                }
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
		Debug.Log ("draw black card after");
		ChangeGameStatus(3);
	}


	/* 時空の扉 */
	void drawFreeCard(){
		if (!mainPlayer) {
			switch(UnityEngine.Random.Range (0,2)){
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
            activateDrawButtons(true);
		}
	}

    public void activateDrawButtons(bool flag)
    {
        buttonPanel.SetActive(flag);
        drawButtons.SetActive(flag);
    }

	/* 希望と絶望の森 */
	void hopeAndDespair(){
        selectingAction = "hopeAndDespair";
        if (mainPlayer)
        {
            activateSelectButtons(true);
        }
        else
        {
            selectCharacter(makeTarget(false));
        }
	}

    void hopeAndDespairSelect()
    {
        if (mainPlayer)
        {
            hdButtons = new GameObject[2];
            for (int i = 0; i < 2; i++)
            {
                hdButtons[i] = Instantiate(buttonPrefab, new Vector3(buttonX + i * xGap, buttonY + i * yGap, buttonZ + i * zGap), Quaternion.identity) as GameObject;
                hdButtons[i].transform.SetParent(buttonPanel.GetComponent<Transform>());
            }
            hdButtons[0].transform.FindChild("Text").GetComponent<Text>().text = "2ダメージを与える";
            hdButtons[0].GetComponent<Button>().onClick.AddListener(() => playerStatesM[tmppid].getDamage(2));
            hdButtons[0].GetComponent<Button>().onClick.AddListener(() => activateHdButtons(false));
            hdButtons[0].GetComponent<Button>().onClick.AddListener(() => ChangeGameStatus(3));
            if (characters[tmppid].findEquipment("Brooch")) hdButtons[0].SetActive(false); //幸運のブローチの効果
            hdButtons[1].transform.FindChild("Text").GetComponent<Text>().text = "1ダメージ回復する";
            hdButtons[1].GetComponent<Button>().onClick.AddListener(() => playerStatesM[tmppid].getDamage(-1));
            hdButtons[1].GetComponent<Button>().onClick.AddListener(() => activateHdButtons(false));
            hdButtons[1].GetComponent<Button>().onClick.AddListener(() => ChangeGameStatus(3));
            activateHdButtons(true);
        }
        else
        {
            playerStatesM[tmppid].getDamage(2);
            ChangeGameStatus(3);
        }
    }

    void activateHdButtons(bool flag)
    {
        for (int i = 0; i < hdButtons.Length; i++)
        {
            hdButtons[i].SetActive(flag);
        }
        buttonPanel.SetActive(flag);
    }

    /* いにしえの祭壇 */
    void altar()
    {
        bool canRob = false;
        List<int> robList = new List<int>();
        for (int i = 0; i < playerNum; i++)
        {
            if (i != playerId && characters[i].hasEquip())
            {
                canRob = true;
                robList.Add(i);
            }
        }
        if (canRob)
        {
            selectingAction = "Alter";
            currentAction = "Alter";
            if (mainPlayer)
            {
                buttonPanel.SetActive(true);
                for (int i = 0; i < robList.Count; i++)
                {
                    if (characters[robList[i]].hasEquip()) selectButtons[i].SetActive(true);
                }
                selectButtons[playerId].SetActive(false);
            }
            else
            {
                selectCharacter(robList[UnityEngine.Random.Range(0, robList.Count - 1)]);
            }
        }
        else
        {
            ChangeGameStatus(3);
        }
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
        updatePredicate(playerId, attackTarget);
        int robeEffect = 0;
        if (attackTarget != angelId)
        {
            int damage = Mathf.Abs(d6Value - d4Value);
            if (characters[playerId].findEquipment("Robe") || characters[attackTarget].findEquipment("Robe"))
            {
                robeEffect = 1;
            }
            Debug.Log("Damage:" + damage + " d6:" + d6Value + " d4:" + d4Value);
            if (damage > 0) damage = damage - robeEffect + characters[playerId].attackPower;
            if (playerStatesM[attackTarget].getDamage(damage)) //攻撃で死んだ場合
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

    void autoAttackSelect(bool masamune)
    {
        Debug.Log("Auto attack select (masamune:" + masamune + ")");
        int existCount = 0;
        for (int i = 0; i < playerExists.Length; i++)
        {
            if (playerExists[i]) existCount++;
        }
        Debug.Log("Exist count:" + existCount);
        if (existCount == 0) ChangeGameStatus(4);
        else
        {
            if (characters[playerId].isHunter() || characters[playerId].isShadow())
            {
                int target;
                if (characters[playerId].isHunter()) target = shadowData;
                else target = hunterData;
                int targetHP = int.MaxValue, targetRest = int.MaxValue;
                bool findEnemy = false;
                for (int i = 0; i < playerNum; i++)
                {
                    if (i == playerId) continue;
                    if (!playerExists[i]) continue;
                    if (playerData[playerId, i] == target)
                    {
                        findEnemy = true;
                        int tmpHP = playerStatesM[i].getScore();
                        int tmpRest = int.MaxValue;
                        if (characters[i].reveal)
                        {
                            tmpRest = characters[i].maxHp - targetHP;
                        }
                        if (tmpRest < 3 && tmpRest < targetRest)
                        {
                            attackTarget = i;
                            targetRest = tmpRest;
                        }
                        else
                        {
                            if (tmpHP < targetHP)
                            {
                                targetHP = tmpHP;
                                attackTarget = i;
                                targetRest = tmpRest;
                            }
                        }
                    }
                }
                if (!findEnemy)
                {
                    double maxPredict = 1.0;
                    if (masamune) maxPredict = double.MinValue;
                    for (int i = 0; i < playerNum; i++)
                    {
                        if (i == playerId) continue;
                        if (!playerExists[i]) continue;
                        if (characters[playerId].isHunter() && predictS[playerId, i] > maxPredict)
                        {
                            attackTarget = i;
                            maxPredict = predictS[playerId, i];
                            findEnemy = true;
                        }
                        else if (characters[playerId].isShadow() && predictH[playerId, i] > maxPredict)
                        {
                            attackTarget = i;
                            maxPredict = predictS[playerId, i];
                            findEnemy = true;
                        }

                    }
                }
                if (findEnemy)
                {
                    dicesActivate(true);
                }
                else ChangeGameStatus(4);
            }
            else
            {
                if (characters[playerId].fullname == CharacterState.CharacterFullName.Catherine)
                {
                    int randomTarget = UnityEngine.Random.Range(0, playerNum + 2);
                    if (randomTarget >= playerId) randomTarget++;
                    if (randomTarget >= playerNum) ChangeGameStatus(4);
                    else
                    {
                        attackTarget = randomTarget;
                        dicesActivate(true);
                    }
                }
                else ChangeGameStatus(4);
            }
        }
    }

    void updatePredicate(int attackId, int receiveId)
    {
        for (int i = 0; i < playerNum; i++)
        {
            if (i == attackId) continue;
            if (playerData[i, attackId] >= 0)
            {
                if (playerData[i, attackId] == hunterData)
                {
                    if(predictS[i,receiveId] > 0) predictS[i, receiveId] += 0.5;
                    predictH[i, receiveId] *= 0.8;
                }
                else if (playerData[i, attackId] == shadowData)
                {
                    if(predictH[i,receiveId] > 0) predictH[i, receiveId] += 0.5;
                    predictS[i, receiveId] *= 0.8;
                }

            }
            else
            {
                if (predictH[i, receiveId] > 0) predictH[i, receiveId] += 0.3 * predictS[i, attackId];
                if (predictS[i, receiveId] > 0) predictS[i, receiveId] += 0.3 * predictH[i, attackId];
                if (predictH[i, attackId] > 0) predictH[i, attackId] += 0.3 * predictS[i, receiveId];
                if (predictS[i, attackId] > 0) predictS[i, attackId] += 0.3 * predictH[i, receiveId];
            }
        }
    }

    public void machineGunReady()
    {
        bool attackPossibility = false;
        attackTargetList = new List<int>();
        for (int i = 0; i < playerExists.Length; i++)
        {
            if (playerExists[i])
            {
                attackTargetList.Add(i);
                attackPossibility = true;
            }
        }
        machineAttackButton1 = Instantiate(buttonPrefab, new Vector3(buttonX, buttonY, buttonZ), Quaternion.identity) as GameObject;
        machineAttackButton2 = Instantiate(buttonPrefab, new Vector3(buttonX, buttonY+yGap, buttonZ), Quaternion.identity) as GameObject;
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
                int damage = Mathf.Abs(d6Value - d4Value);
                if (characters[playerId].findEquipment("Robe") || characters[target].findEquipment("Robe"))
                {
                    robeEffect = 1;
                }
                if (damage > 0) damage = damage - robeEffect + characters[playerId].attackPower;
                Debug.Log("Damage:" + damage + " d6:" + d6Value + " d4:" + d4Value);
                if (playerStatesM[target].getDamage(damage)) //攻撃で死んだ場合
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
        if (revengeOccur)
        {
            revengeReady(attackTarget, playerId);
        }
        else
        {
            ChangeGameStatus(4);
        }
    }

    public void setAttackButtons(bool masamune)
    {
        bool attackPossibility = false;
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

	public void activateAttackButtons(bool flag){
        buttonPanel.SetActive(flag);
        foreach (GameObject obj in attackButtons)
        {
			obj.SetActive(flag);
		}
	}
	public void activateSelectButtons(bool flag){
        buttonPanel.SetActive(flag);
        for (int i = 0; i < selectButtons.Length; i++)
        {
            if (!playerStatesM[i].dead) selectButtons[i].SetActive(flag);
        }
	}

    public void activateMachineAttackButtons(bool flag)
    {
        machineAttackButton1.SetActive(flag);
        machineAttackButton2.SetActive(flag);
        buttonPanel.SetActive(flag);
    }
    public void startMachineGun()
    {
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
                tmppid = id;
                hopeAndDespairSelect();
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
            case "banana":
                tmppid = id;
                equipHandoffSelect(true);
                break;
            case "devil":
                tmppid = id;
                equipHandoffSelect(false);
                break;
            case "Alter":
                tmppid = id;
                equipHandoffSelect(false);
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
                if (i != playerId && playersPositions[i] / 2 != playersPositions[playerId] / 2 && !playerStatesM[i].dead && playersPositions[i] != -2)
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
                        if (playersPositions[i] == stages[area] && !characters[i].findEquipment("Amulet")) //魔除けのお守り効果
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
                        if (mainPlayer) compassActivate(true);
                        else selectCompass(UnityEngine.Random.Range(1, 2));
                    }
                }
                else
                {
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

    IEnumerator diceRollAfterSeconds(int sec)
    {
        yield return new WaitForSeconds(sec);
        diceroll.SendMessage("diceRoll");
        yield break;
    }

	IEnumerator waitingClick(){
		while (true) {
			if(Input.GetMouseButton(0)) break;
			yield return null;
		}
	}
	
	void dicesActivate(bool flag){
        if (flag)
        {
            d4.transform.position = new Vector3(5, 20, -2);
            d6.transform.position = new Vector3(-2, 14, 0);
            d4.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
            d6.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
            if (!(selectingAction == "George" || selectingAction == "doll"))
            {
                d4.SetActive(flag);
                d4.GetComponent<Rigidbody>().Sleep();
            }
            if (!(masamune || selectingAction == "benefit"))
            {
                d6.SetActive(flag);
                d6.GetComponent<Rigidbody>().Sleep();
            }
            if (mainPlayer)
            {
                buttonPanel.SetActive(flag);
                diceButton.SetActive(flag);
            }
            else
            {
                StartCoroutine(diceRollAfterSeconds(5));
            }
        }
        else
        {
            d4.SetActive(flag);
            d6.SetActive(flag);
            if (mainPlayer)
            {
                buttonPanel.SetActive(flag);
                diceButton.SetActive(flag);
            }
        }
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
        if (ritual)
        {
            openButton2.GetComponent<Button>().onClick.RemoveListener(() => ChangeGameStatus(0));
            openButton2.GetComponent<Button>().onClick.AddListener(() => drawBlackCardAfter());
        }
    }

    public void revealIdentity()
    {
        characters[playerId].reveal = true;

        if (playerId == mainPlayerId)
        {
            GameObject effect = Instantiate(smokeEffect, monsters[mainPlayerId].transform.position, monsters[mainPlayerId].transform.rotation) as GameObject;
            Destroy(effect, 1.0f);

            iTween.ScaleTo(monsters[mainPlayerId], iTween.Hash("scale", new Vector3(0,0,0), "time", 1.0f));
            GameObject cath = Instantiate(catherine, monsters[mainPlayerId].transform.position, monsters[mainPlayerId].transform.rotation) as GameObject;
            monsters[mainPlayerId] = cath;
            iTween.ScaleTo(monsters[mainPlayerId], iTween.Hash("scale", new Vector3(50, 50, 50), "time", 1.0f));
        }

        for (int i = 0; i < playerNum; i++)
        {
            if (i == playerId) continue;
            if (characters[playerId].isNeutral()) playerData[i, playerId] = neutralData;
            if (characters[playerId].isHunter()) playerData[i, playerId] = hunterData;
            if (characters[playerId].isShadow()) playerData[i, playerId] = shadowData;
            deletePredicts(i, playerId);
        }

        if (ritual)
        {
            if (characters[playerId].isShadow()) playerStatesM[playerId].moveScore(0);
            ritual = false;
        }
        if (characters[playerId].name == CharacterState.CharacterName.G) //Georgeの特殊効果
        {
            selectingAction = "George";
            if (mainPlayer)
            {
                activateSelectButtons(true);
                selectButtons[playerId].SetActive(false);
                selecting = true;
            }
            else
            {
                selectCharacter(makeTarget(false));
            }
        }
        else if (characters[playerId].name == CharacterState.CharacterName.F) //Fukaの特殊能力
        {
            selectingAction = "Fuka";
            if (mainPlayer)
            {
                activateSelectButtons(true);
                selectButtons[playerId].SetActive(false);
                selecting = true;
            }
            else
            {
                bool found = false;
                for (int i = 0; i < playerNum; i++)
                {
                    if(characters[i].isHunter() && i != playerId && playerStatesM[i].getScore() > 7) {
                        selectCharacter(i);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    selectCharacter(makeTarget(false));
                }
            }
        }
        else if(characters[playerId].fullname == CharacterState.CharacterFullName.Wight) //Wightの特殊能力
        {
            for (int i = 0; i < playerNum; i++)
            {
                if (i == playerId) continue;
                if (playerStatesM[i].dead) wightCounter++;
            }
            if (ritual)
            {
                drawBlackCardAfter();
            }
            else
            {
                if (mainPlayer)
                {
                    ChangeGameStatus(0);
                }
                else
                {
                }
            }
        }
        else
        {
            if (ritual)
            {
                drawBlackCardAfter();
            }
            else
            {
                ChangeGameStatus(0);
            }
        }
    }

    int wolfId, wolfTarget;

    public void revengeReady(int wolfId, int targetId) //werewolfの反撃
    {
        Debug.Log("Revenge start");
        this.wolfId = wolfId;
        wolfTarget = targetId;
        diceAction = "werewolf";
        dicesActivate(true);
    }
    public void revenge()
    {
        int robeEffect = 0;
        if (wolfTarget != angelId)
        {
            int damage = Mathf.Abs(d6Value - d4Value);
            if (characters[wolfId].findEquipment("Robe") || characters[wolfTarget].findEquipment("Robe"))
            {
                robeEffect = 1;
            }
            if (damage > 0) damage = damage - robeEffect + characters[wolfId].attackPower;
            if (playerStatesM[wolfTarget].getDamage(damage)) //攻撃で死んだ場合
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

    bool revealDecision()
    {
        bool reveal = false;
        int count = 0;
        if (characters[playerId].fullname == CharacterState.CharacterFullName.Catherine)
        {
            for (int i = 0; i < playerNum; i++)
            {
                if (i == playerId) continue;
                if (playerStatesM[i].dead) count++;
            }
            if (count > 0) reveal = true;
        }
        else
        {

            for (int i = 0; i < playerNum; i++)
            {
                if (characters[i].reveal) count++;
            }
            if (count >= playerNum - 1) reveal = true;
            if (!reveal)
            {
                if (playerStatesM[playerId].getScore() >= characters[playerId].maxHp * 0.8) reveal = true;
            }
        }
        return reveal;
    }

    /* 勝敗判定処理 */
    bool checkWinner()
    {
        for(int i = 0;i<playerStatesM.Length;i++)
        {
            if(characters[i].isHunter()) //ハンターの勝利条件確認
            {
                playerWin[i] = true;
                for(int j = 0;j<playerStatesM.Length;j++)
                {
                    if(characters[j].isShadow() && !playerStatesM[j].dead) playerWin[i] = false;
                }
            }
            else if(characters[i].isShadow()) //シャドウの勝利条件確認
            {
                playerWin[i] = true;
                for(int j = 0;j<playerStatesM.Length;j++)
                {
                    if(characters[j].isHunter() && !playerStatesM[j].dead) playerWin[i] = false;
                }
            }
            else //キャサリンの勝利条件確認(ニュートラル追加の際は要変更)
            {
                if (playerStatesM[i].dead)
                {
                    playerWin[i] = true;
                    for (int j = 0; j < playerStatesM.Length; j++)
                    {
                        if (j != i && playerStatesM[j].dead) playerWin[i] = false;
                    }
                }
                else
                {
                    int count = 0;
                    for (int j = 0; j < playerStatesM.Length; j++)
                    {
                        if (j != i && !playerStatesM[j].dead) count++;
                    }
                    if (count == 1) playerWin[i] = true;
                }
            }
        }
        bool checker = false;
        for(int i = 0;i<playerWin.Length;i++) if(playerWin[i]) checker = true;
        return checker;
    }

   /* ランダムターゲット選択 */
    int makeTarget(bool merit)
    {
        int madeTarget = -1;
        if (merit)
        {
            if (characters[playerId].isHunter() || characters[playerId].isShadow())
            {
                int target;
                if (characters[playerId].isHunter()) target = hunterData;
                else target = shadowData;
                int targetHP = int.MaxValue, targetRest = int.MaxValue;
                bool findEnemy = false;
                for (int i = 0; i < playerNum; i++)
                {
                    if (i == playerId) continue;
                    if (playerData[playerId, i] == target)
                    {
                        findEnemy = true;
                        int tmpHP = playerStatesM[i].getScore();
                        int tmpRest = int.MaxValue;
                        if (characters[playerId].reveal)
                        {
                            tmpRest = characters[i].maxHp - targetHP;
                        }
                        if (tmpRest < 3 && tmpRest < targetRest)
                        {
                            madeTarget = i;
                            targetRest = tmpRest;
                        }
                        else
                        {
                            if (tmpHP < targetHP)
                            {
                                targetHP = tmpHP;
                                madeTarget = i;
                                targetRest = tmpRest;
                            }
                        }
                    }
                }
                if (!findEnemy)
                {
                    double maxPredict = 1.0;
                    for (int i = 0; i < playerNum; i++)
                    {
                        if (i == playerId) continue;
                        if (characters[playerId].isHunter() && predictH[playerId, i] > maxPredict)
                        {
                            madeTarget = i;
                            maxPredict = predictS[playerId, i];
                            findEnemy = true;
                        }
                        else if (characters[playerId].isShadow() && predictS[playerId, i] > maxPredict)
                        {
                            madeTarget = i;
                            maxPredict = predictS[playerId, i];
                            findEnemy = true;
                        }
                    }
                }
                if (!findEnemy)
                {
                    madeTarget = UnityEngine.Random.Range(0, playerNum - 2);
                    if (madeTarget >= playerId) madeTarget++;
                }
            }
            else
            {
                madeTarget = UnityEngine.Random.Range(0, playerNum - 2);
                if (madeTarget >= playerId) madeTarget++;
            }
        }
        else
        {
            if (characters[playerId].isHunter() || characters[playerId].isShadow())
            {
                int target;
                if (characters[playerId].isHunter()) target = shadowData;
                else target = hunterData;
                int targetHP = int.MaxValue, targetRest = int.MaxValue;
                bool findEnemy = false;
                for (int i = 0; i < playerNum; i++)
                {
                    if (i == playerId) continue;
                    if (playerData[playerId, i] == target)
                    {
                        findEnemy = true;
                        int tmpHP = playerStatesM[i].getScore();
                        int tmpRest = int.MaxValue;
                        if (characters[playerId].reveal)
                        {
                            tmpRest = characters[i].maxHp - targetHP;
                        }
                        if (tmpRest < 3 && tmpRest < targetRest)
                        {
                            madeTarget = i;
                            targetRest = tmpRest;
                        }
                        else
                        {
                            if (tmpHP < targetHP)
                            {
                                targetHP = tmpHP;
                                madeTarget = i;
                                targetRest = tmpRest;
                            }
                        }
                    }
                }
                if (!findEnemy)
                {
                    double maxPredict = 1.0;
                    for (int i = 0; i < playerNum; i++)
                    {
                        if (i == playerId) continue;
                        if (characters[playerId].isHunter() && predictS[playerId, i] > maxPredict)
                        {
                            madeTarget = i;
                            maxPredict = predictS[playerId, i];
                            findEnemy = true;
                        }
                        else if (characters[playerId].isShadow() && predictH[playerId, i] > maxPredict)
                        {
                            madeTarget = i;
                            maxPredict = predictS[playerId, i];
                            findEnemy = true;
                        }
                    }
                }
                if (!findEnemy)
                {
                    madeTarget = UnityEngine.Random.Range(0, playerNum - 2);
                    if (madeTarget >= playerId) madeTarget++;
                }
            }
            else
            {
                madeTarget = UnityEngine.Random.Range(0, playerNum - 2);
                if (madeTarget >= playerId) madeTarget++;
            }
        }
        return madeTarget;
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

        if (gameStatus == 0)
        {
            GameObject faze = Instantiate(fazeEffect) as GameObject;

            if (!mainPlayer)
            {
                faze.GetComponentInChildren<Image>().color = Color.green;
                faze.GetComponentInChildren<Text>().text = "(CPU)";
            }

            Destroy(faze, 2.0f);
        }
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
