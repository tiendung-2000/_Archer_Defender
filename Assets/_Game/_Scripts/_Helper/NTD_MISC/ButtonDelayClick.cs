using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDelayClick : MonoBehaviour
{
    public Image image;
    public Button button;
    float time = 0f;

    void Update()
    {
        if (time > 0f)
        {
            time -= Time.deltaTime;
            image.raycastTarget = false;
        }
        else
        {
            image.raycastTarget = true;
        }
    }

    public void OnClickButton()
    {
        time = 1f;
    }
}