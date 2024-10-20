using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairSwitcher : MonoBehaviour
{
    public Image crossHair;
    public Sprite sprite1;
    public Sprite sprite2;

    private void Awake()
    {
        crossHair = transform.GetChild(0).GetComponent<Image>();
    }

    public void CrossHairChange(bool isAllow)
    {
        if (isAllow)
        {
            crossHair.sprite = sprite1;
        }
        else
        {
            crossHair.sprite = sprite2;
        }
    }
}
