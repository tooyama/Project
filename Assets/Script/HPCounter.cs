using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPCounter : MonoBehaviour 
{
    private GameObject[] HPs = new GameObject[5];

    private Vector3[] defaultHPPos = new Vector3[5];

    private Vector3[] defaultHPSize = new Vector3[5];

    private bool popUpFlag = false;

	void Start () 
    {
        for (int i = 0; i < HPs.Length; ++i)
        {
            string path = "HPCircle" + (i + 1).ToString();

            HPs[i] = transform.FindChild(path).gameObject;
        }

        for (int i = 0; i < HPs.Length; ++i)
        {
            defaultHPPos[i] = HPs[i].transform.position;

            defaultHPSize[i] = HPs[i].transform.localScale;
        }

        for (int i = 0; i < HPs.Length; ++i)
        {
            HPs[i].transform.position = HPs[2].transform.position;

            HPs[i].transform.localScale = Vector3.zero;
        }
	}

	void Update () 
    {
	    if(Input.GetKeyDown("a"))
        {
            if(popUpFlag)
            {
                popUpFlag = false;

                GetComponent<Image>().color = new Color(255,255,255,0);

                for (int i = 0; i < HPs.Length; ++i)
                {
                    iTween.MoveTo(HPs[i], HPs[2].transform.position, 0.5f);
                    iTween.ScaleTo(HPs[i], iTween.Hash("scale", Vector3.zero, "time", 0.5f));
                }
            }
            else
            {
                popUpFlag = true;

                GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);

                for (int i = 0; i < HPs.Length; ++i)
                {
                    iTween.MoveTo(HPs[i], defaultHPPos[i], 0.5f);
                    iTween.ScaleTo(HPs[i], iTween.Hash("scale", defaultHPSize[i], "time", 0.5f));
                }
            }
        }
	}
}
