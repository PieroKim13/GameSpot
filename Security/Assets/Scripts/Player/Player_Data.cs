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
    }

    private void Awake()
    {
        base_Data = new(this);
    }

    private void Init()
    {
        player = GameManager.Player;
        base_Data.Init();
    }
}
