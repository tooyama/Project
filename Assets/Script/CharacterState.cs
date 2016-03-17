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
    public enum CharacterFullName
    {
        Catherine,
        Charles,
        George,
        Fuka,
        Werewolf,
        Wight
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
    public CharacterFullName fullname;
    public int attackPower = 0;
    public bool reveal = false;

    List<string> equipment = new List<string>();

    public int getEquipLength()
    {
        return equipment.Count;
    }

    public string getEquipImage(int num)
    {
        return equipment[num];
    }

    public void addEquipment(string equip)
    {
        Debug.Log(fullname + " / " + equip);
        equipment.Add(equip);
        if (equip.Equals("bigAxe") || equip.Equals("knife") || equip.Equals("ChainSaw"))
            attackPower++;
    }
    public void removeEquipment(string equip)
    {
        equipment.Remove(equip);
        if (equip.Equals("bigAxe") || equip.Equals("knife") || equip.Equals("ChainSaw"))
            attackPower--;
    }

    public bool findEquipment(string equip)
    {
        bool found = false;
        foreach (string e in equipment)
        {
            if (e.Equals(equip))
            {
                found = true;
                break;
            }
        }
        return found;
    }

    public List<string> getEquipList()
    {
        return equipment;
    }

    public bool hasEquip()
    {
        return equipment.Count > 0;
    }

    public bool isNeutral()
    {
        return type == CharacterType.Neutral;
    }
    public bool isShadow()
    {
        return type == CharacterType.Shadow;
    }
    public bool isHunter()
    {
        return type == CharacterType.Hunter;
    }

    public bool isOver(int num)
    {
        return maxHp >= num;
    }
    public bool isUnder(int num)
    {
        return maxHp <= num;
    }

    void Start()
    {

    }


    void Update()
    {

    }
}
