using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColor : MonoBehaviour
{
    private TMP_Text txt;
    private Color txtColor;
    [SerializeField] private float colorChangeSpeed = 1.0f;
    private bool aUp = false;

    void Awake()
    {
        txt = GetComponent<TMP_Text>();
    }

    void Update()
    {
        txtColor = txt.color;
        
        if(aUp)
        {
            txtColor.a += Time.deltaTime * colorChangeSpeed;

            if(txt.color.a >= 1.0f)
            {
                aUp = false;
            }
        }
        else
        {
            txtColor.a -= Time.deltaTime * colorChangeSpeed;

            if(txt.color.a <= 0.3f)
            {
                aUp = true;
            }
        }

        txt.color = txtColor;
    }
}
