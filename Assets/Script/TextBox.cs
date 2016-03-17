using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBox : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GameObject.Find("TextBoxString").GetComponent<Text>();
    }

    public void ChangeText(string t)
    {
        text.text = t;
    }
}
