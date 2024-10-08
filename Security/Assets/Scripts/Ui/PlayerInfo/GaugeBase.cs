using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBase : MonoBehaviour
{
    public Color color = Color.white;

    protected Slider slider;

    protected float maxValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Transform child;

        child = transform.GetChild(0);
        Image backGroundImage = child.GetComponent<Image>();
        backGroundImage.color = new Color(color.r, color.g, color.b, color.a*0.3f);

        child = transform.GetChild(1);
        Image fillImage = child.GetComponentInChildren<Image>();
        fillImage.color = new Color(color.r, color.g, color.b, color.a * 0.3f);
    }

    protected void OnValueChange(float ratio)
    {
        //ratio = Mathf.Clamp01(ratio);
        slider.value = ratio * 0.01f;
        Debug.Log(ratio);
    }
}
