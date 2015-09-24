using UnityEngine;
using System.Collections;

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

	void Start () {
	
	}
	
	
	void Update () {
	
	}
}
