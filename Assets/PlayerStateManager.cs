﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStateManager : MonoBehaviour {

	public int score;
    public bool dead = false;
    public Transform playerPos; 
    public GameObject damageEffect;
    public GameObject stateUI;
    public GameObject[] equipUI = new GameObject[2];
    private string name;
    private GameObject equipState;
    public int number = 0;
	int maxHP = 0;
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

        name = GameObject.Find("Stage").GetComponent<SHManeger>().getName(number).ToString();

        equipState = GameObject.Find(name);
	}
	
	// Update is called once per frame
	void Update () 
    {
        CharacterState equip = equipState.GetComponent<CharacterState>();
        /*
	    if(equip.getEquipLength() != 0) 
        {
            equipUI[0].GetComponent<Image>().sprite = Resources.Load<Sprite>(equip.getEquipImage(0));
        }
        */
	}

	public int getScore(){
		return score;
	}

	public void moveScore(int newScore){
        Debug.Log("score:" + score + " newscore:" + newScore + " maxHP" + maxHP);
		if (newScore >= maxHP) {
			GameObject.FindGameObjectWithTag("stage").GetComponent<SHManeger>().ChangeGameStatus(5);
			return;
		}
		score = newScore;
		Vector3 v3 = gameObject.transform.position;
		v3.z = HP [score].transform.position.z;
		gameObject.transform.position = v3;
	}
	public void setMaxHp(int max){
		maxHP = max;
	}

    public bool getDamage(int damage)
    {
        score += damage;

        stateUI.GetComponent<Text>().text = score.ToString();

        StartCoroutine("waitForDamage", damage);

        if (score < 0) score = 0;
        if (score > maxHP) dead = true;
        if (!dead)
        {
            Vector3 v3 = gameObject.transform.position;
            v3.z = HP[score].transform.position.z;
            gameObject.transform.position = v3;
        }
        else
        {
            gameObject.SetActive(false);
        }
        return dead;
    }

    private IEnumerator waitForDamage(int damage)
    {
        yield return new WaitForSeconds(0.5f);

        GameObject effect = Instantiate(damageEffect, playerPos.transform.position, playerPos.transform.rotation) as GameObject;

        Destroy(effect, 1.0f);
    }
}
