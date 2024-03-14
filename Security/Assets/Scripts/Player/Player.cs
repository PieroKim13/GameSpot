using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float stamina = 10.0f;
    public float Stamina
    {
        get => stamina;
        set
        {
            stamina = value;
        }
    }
}
