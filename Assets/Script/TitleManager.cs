using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour 
{

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            GetComponentInChildren<Text>().text = "";

            LoadLevel("Game");
        }
    }

    void LoadLevel(string name)
    {
        float time = 0.5f;

        FadeCamera.Instance.FadeOut(time, () =>
        {
            Application.LoadLevel(name);

            FadeCamera.Instance.FadeIn(time, () =>
            {
            });
        });
    }
}
