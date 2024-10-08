using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    IEnumerator sprintCorutine;

    Player_Controller controller;
    Player_Data player_Data;
    public Player_Data Player_Data => player_Data;

    public bool isAlive => player_Data.Base_Data.CurrentHP > 0;

    private void Start()
    {
        player_Data = GameManager.Player_Data;

        sprintCorutine = sprintDerease();
    }

    private void Awake()
    {
        controller = GetComponent<Player_Controller>();
        controller.onSprinting = () => StaminaChange_Coroutine(true);
        controller.offSprinting = () => StaminaChange_Coroutine(false);
    }

    IEnumerator sprintDerease()
    {
        while (true)
        {
            player_Data.Base_Data.CurrentStamina -= 20.0f * Time.deltaTime;
            if (player_Data.Base_Data.CurrentStamina <= 0)
            {
                controller.OffSprinting();
            }
            yield return null;
        }
    }

    private void StaminaChange_Coroutine(bool isSprint)
    {
        if (isSprint)
        {
            StopCoroutine(sprintCorutine);
            sprintCorutine = sprintDerease();
            StartCoroutine(sprintCorutine);
        }
        else
        {
            StopCoroutine(sprintCorutine );
            sprintCorutine = StaminaRegetateCroutine();
            StartCoroutine(sprintCorutine);
        }
    }

    IEnumerator StaminaRegetateCroutine()
    {
        if(!controller.isStamina)
        {
            controller.isStamina = true;
            yield return new WaitForSeconds(0.75f);
        }

        float regenpersec = player_Data.Base_Data.Base_MaxStamina / 10;
        float timeElapsed = 0.0f;
        while(timeElapsed < 10)
        {
            timeElapsed += Time.deltaTime;
            player_Data.Base_Data.CurrentStamina += Time.deltaTime * regenpersec;

            yield return null;
        }

    }
}
