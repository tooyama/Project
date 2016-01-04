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
    public int attackPower = 0;

    List<string> equipment = new List<string>();

    public void addEquipment(string equip)
    {
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
