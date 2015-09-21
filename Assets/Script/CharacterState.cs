using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour 
{
    public enum CharacterType
    {
        Shadow,
        Hunter,
        Neutral
    }

    public int maxHp = 0;
    public CharacterType type = CharacterType.Neutral;

	void Start () {
	
	}
	
	
	void Update () {
	
	}
}
