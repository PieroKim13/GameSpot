using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Door : MonoBehaviour
{
    enum DoorState
    {
        Close,
        Foward,
        Back
    }
    DoorState state = DoorState.Close;
    DoorState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                switch (state)
                {
                    case DoorState.Close:
                        animator.SetInteger(Door_State, 0);
                        break;
                    case DoorState.Foward:
                        animator.SetInteger(Door_State, 1);
                        break;
                    case DoorState.Back:
                        animator.SetInteger(Door_State, 2);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    Transform door;
    Animator animator;
    AudioSource[] audioSources; // 0 = Close, 1 = Open

    readonly int Door_State = Animator.StringToHash("DoorState");

    private void Awake()
    {
        animator.transform.GetComponent<Animator>();
        audioSources = GetComponents<AudioSource>();
    }
    
    public void OnInteract()
    {
        if(State == DoorState.Close)
        {

        }
        else
        {
            State = DoorState.Close;
            audioSources[0].Play();
        }
    }
}
