using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandCard : MonoBehaviour 
{
    public float moveRange = 20.0f;
    public float moveTime = 0.1f;

    private GameObject selectCard;
    private GameObject[] cardArray = new GameObject[6];
    private int cardArrayLength;
    private int selectCardNumber;
    private bool isInputLock;

	void Start () 
    {
        isInputLock = false;

        selectCardNumber = 0;

        cardArrayLength = cardArray.Length;

        selectCard = transform.FindChild("CardImage").gameObject;

        GameObject handList = transform.FindChild("HandList").gameObject;

        for(int i=0; i<cardArrayLength; ++i)
        {
            cardArray[i] = handList.transform.GetChild(i).gameObject;

            string spritePath = "CardImage/SH-" + (i+1).ToString();

            cardArray[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(spritePath);
        }

        selectCard.GetComponent<Image>().sprite = cardArray[selectCardNumber].GetComponent<Image>().sprite;

        moveCardObject(cardArray[selectCardNumber], true);
	}
	
	void Update () 
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (!isInputLock)
        {
            if (inputHorizontal != 0)
            {
                StartCoroutine(LockInput());

                moveCardObject(cardArray[selectCardNumber], false);

                if (0 < inputHorizontal)
                {
                    if (selectCardNumber < cardArrayLength - 1)
                    {
                        ++selectCardNumber;
                    }
                    else
                    {
                        selectCardNumber = 0;
                    }
                }
                else if (inputHorizontal < 0)
                {
                    if (0 < selectCardNumber)
                    {
                        --selectCardNumber;
                    }
                    else
                    {
                        selectCardNumber = cardArrayLength - 1;
                    }
                }

                moveCardObject(cardArray[selectCardNumber], true);

                selectCard.GetComponent<Image>().sprite = cardArray[selectCardNumber].GetComponent<Image>().sprite;
            }
        }
	}

    private void moveCardObject(GameObject cardObject, bool isMoveUp)
    {
        if(isMoveUp)
        {
            iTween.MoveBy(cardObject, iTween.Hash("y", moveRange, "time", moveTime));
        }
        else
        {
            iTween.MoveBy(cardObject, iTween.Hash("y", -moveRange, "time", moveTime));
        }
    }

    private IEnumerator LockInput()
    {
        isInputLock = true;

        yield return new WaitForSeconds(0.1f);

        isInputLock = false;
    }
}
