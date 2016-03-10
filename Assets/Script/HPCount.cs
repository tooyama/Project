using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPCount : MonoBehaviour 
{
    public float hp = 12.0f;
    private float defaultHp;
    private Text hpText;
    private Image circleImage;

	void Start () 
    {
        defaultHp = hp;
        circleImage = transform.FindChild("HPCircle").GetComponent<Image>();
        hpText = transform.FindChild("HPCircle").FindChild("HPCount").GetComponent<Text>();
        hpText.text = hp.ToString();
	}
	
	void Update () 
    {
	    if(Input.anyKeyDown)
        {
            ChangeState(1);
        }
	}

    void ChangeState(float damage)
    {
        if(hp > 0)
        {
            hp -= damage;
        }

        circleImage.fillAmount = hp / defaultHp;

        hpText.text = hp.ToString();
    }
}
