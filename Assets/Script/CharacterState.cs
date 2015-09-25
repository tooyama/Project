using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterState : MonoBehaviour 
{
    public enum CharacterName
    {
        C,
        G,
        F,
        W
    }

    public enum CharacterType
    {
        Shadow,
        Hunter,
        Neutral
    }

    public int maxHp = 0;
    public CharacterName name = CharacterName.C;
    public CharacterType type = CharacterType.Neutral;
	int attackPower = 0;


	List<string> equipment = new List<string>();

	public void addEquipment(string equip){
		equipment.Add(equip);
		if (equip.Equals ("bigAxe"))
			attackPower++;
	}
	public void removeEquipment(string equip){
		equipment.Remove(equip);
		if (equip.Equals ("bigAxe"))
			attackPower--;
	}
	public bool findEquipment(string equip){
		bool found = false;
		foreach (string e in equipment) {
			if(e.Equals(equip)){
				found = true;
				break;
			}
		}
		return found;
	}


	void Start () {
	
	}
	
	
	void Update () {
	
	}
}
