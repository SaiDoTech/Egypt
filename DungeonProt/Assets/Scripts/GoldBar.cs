using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldBar : MonoBehaviour
{
    public Image bar;
    public float fill;
    public Text text;
    void Start()
    {
        fill = 1f;
        text.text = "100";
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = fill;
    }
}
