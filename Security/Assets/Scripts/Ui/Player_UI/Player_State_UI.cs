using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_State_UI : MonoBehaviour
{
    Player_Data data;

    CanvasGroup group;

    Transform hpBar;
    Transform staBar;

    TextMeshProUGUI hpText;
    Slider staSlider;
    Image backGroundImage;
    Image fillImage;

    Color color = Color.white;

    private void Start()
    {
        data = GameManager.Data;
        
        Init();
        data.Base_Data.on_CurrentStamina_Change += OnStaValueChange;
        data.Base_Data.on_CurrentHP_Change += OnHpValueChange;
    }

    private void Awake()
    {
        GetComponents();
    }

    void GetComponents()
    {
        group = GetComponent<CanvasGroup>();

        hpBar = transform.GetChild(0);
        hpText = hpBar.GetChild(0).GetComponent<TextMeshProUGUI>();

        staBar = transform.GetChild(1);
        staSlider = staBar.GetComponent<Slider>();
        backGroundImage = staSlider.transform.GetChild(0).GetComponent<Image>();
        fillImage = staSlider.transform.GetChild(1).GetComponentInChildren<Image>();
    }

    void Init()
    {
        hpText.text = $"{Mathf.FloorToInt(data.Base_Data.CurrentHP)}%";

        staSlider.value = data.Base_Data.CurrentStamina / data.Base_Data.Base_MaxStamina;
        backGroundImage.color = new Color(color.r, color.g, color.b, color.a * 0.3f);
        fillImage.color = new Color(color.r, color.g, color.b, color.a * 0.3f);
    }

    void OnPlayerState()
    {
        group.alpha = 1.0f;
    }

    void OffPlayerState()
    {
        group.alpha = 0.0f;
    }

    void OnStaValueChange(float ratio)
    {
        staSlider.value = ratio * 0.01f;
    }
    
    void OnHpValueChange(float ratio)
    {
        hpText.text = $"{Mathf.FloorToInt(ratio)}%";
    }
}
