using UnityEngine;
using System.Collections;

public class CardSlide : MonoBehaviour 
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void CardMove(int range)
    {
        if (!Input.GetMouseButton(0))
        {
            return;
        }

        transform.Translate(range,0,0);
    }
}
