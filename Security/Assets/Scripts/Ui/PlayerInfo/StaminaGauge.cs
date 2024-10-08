using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaGauge : GaugeBase
{
    private void Start()
    {
        Player_Data player_Data = GameManager.Player_Data;
        maxValue = player_Data.Base_Data.Base_MaxStamina;
        slider.value = player_Data.Base_Data.CurrentStamina / player_Data.Base_Data.Base_MaxStamina;
        player_Data.Base_Data.on_CurrentStamina_Change += OnValueChange;
    }
}
