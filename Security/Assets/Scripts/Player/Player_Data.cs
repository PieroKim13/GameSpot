using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Base_Data
{
    Player_Data player_Data;

    [SerializeField]
    float base_MaxHP;
    public float Base_MaxHP
    {
        get => base_MaxHP;
        private set
        {
            base_MaxHP = value;
            player_Data.MaxHP = base_MaxHP;
            on_MaxHP_Change?.Invoke(base_MaxHP);
        }
    }
    [SerializeField]
    float currentHP;
    public float CurrentHP
    {
        get => currentHP;
        set
        {
            currentHP = Mathf.Clamp(value, 0.0f, base_MaxHP);
            player_Data.HP = currentHP;
            on_CurrentHP_Change?.Invoke(currentHP);
            if(currentHP <= 0.0f)
            {
                on_Die?.Invoke();
            }
        }
    }
    [SerializeField]
    float base_MaxStamina;
    public float Base_MaxStamina
    {
        get => base_MaxStamina;
        private set
        {
            base_MaxStamina = value;
            player_Data.MaxStamina = base_MaxStamina;
            on_MaxStamina_Change?.Invoke(base_MaxStamina);
        }
    }
    [SerializeField]
    float currentStamina;
    public float CurrentStamina
    {
        get => currentStamina;
        set
        {
            currentStamina = Mathf.Clamp(value, 0.0f, base_MaxStamina);
            player_Data.Stamina = currentStamina;
            on_CurrentStamina_Change?.Invoke(currentStamina);
        }
    }

    public Action<float> on_MaxHP_Change;
    public Action<float> on_CurrentHP_Change;
    public Action<float> on_MaxStamina_Change;
    public Action<float> on_CurrentStamina_Change;
    public Action on_Die;

    public Base_Data(Player_Data player_Data)
    {
        this.player_Data = player_Data;

    }

    public void Init()
    {
        Base_MaxHP = 100.0f;
        CurrentHP = Base_MaxHP;
        Base_MaxStamina = 100.0f;
        CurrentStamina = Base_MaxStamina;
    }
}

public class Player_Data : MonoBehaviour
{
    Color color = Color.white;

    CanvasGroup canvansGroup;

    Transform child;
    Transform hpGauge;
    Transform staminaGauge;

    TextMeshProUGUI hpText;
    Slider slider;
    Image backGroundImgage;
    Image fillImage;

    Player player;
    Base_Data base_Data;

    public Base_Data Base_Data => base_Data;

    float hp;
    float maxHP;
    float stamina;
    float maxStamina;

    bool isAlive => hp > 0;

    public float HP
    {
        get => hp;
        set
        {
            if (isAlive)
            {
                hp = value;
                hpText.text = $"{hp}%";
            }
        }
    }

    public float MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
        }
    }

    public float Stamina
    {
        get => stamina;
        set
        {
            stamina = value;
        }
    }

    public float MaxStamina
    {
        get => maxStamina;
        set
        {
            maxStamina = value;
        }
    }

    private void Start()
    {
        Init();
        Base_Data.on_CurrentStamina_Change += OnValueChange;
    }

    private void Awake()
    {
        GetComponents();
        base_Data = new(this);
    }

    private void Init()
    {
        player = GameManager.Player;
        base_Data.Init();

        slider.value = Base_Data.CurrentStamina / Base_Data.Base_MaxStamina;
        backGroundImgage.color = new Color(color.r, color.g, color.b, color.a * 0.3f);
        fillImage.color = new Color(color.r, color.g, color.b, color.a * 0.3f);

        Base_Data.CurrentHP -= 10.0f;
        Base_Data.CurrentHP++;
        hpText.text = $"{hp}";
    }

    void GetComponents()
    {
        canvansGroup = GetComponent<CanvasGroup>();

        hpGauge = transform.GetChild(0);
        hpText = hpGauge.GetChild(0).GetComponent<TextMeshProUGUI>();

        staminaGauge = transform.GetChild(1);
        slider = staminaGauge.GetComponent<Slider>();
        child = staminaGauge.GetChild(0);
        backGroundImgage = child.GetComponent<Image>();
        child = staminaGauge.GetChild(1);
        fillImage = child.GetComponentInChildren<Image>();
        

    }

    void Open()
    {
        canvansGroup.alpha = 1.0f;
    }

    void Close()
    {
        canvansGroup.alpha = 0.0f;
    }

    void OnValueChange(float ratio)
    {
        slider.value = ratio * 0.01f;
    }
}
