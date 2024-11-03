using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColor : MonoBehaviour
{
    private TMP_Text txt;

    void Awake()
    {
        txt = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if(txt.color.r >= 0)
        {
             
        }
    }
}
