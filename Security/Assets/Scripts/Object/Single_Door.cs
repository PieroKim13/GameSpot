using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Door : MonoBehaviour
{
    enum DoorState
    {
        Close = 0,
        Open,
        Lock
    }
    DoorState state = DoorState.Close;
    DoorState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                if (IsAnimationPlaying())
                {
                    return;
                }

                state = value;
                switch (state)
                {
                    case DoorState.Close:
                        animator.SetInteger(Hash_Door, 0);
                        audioSources[0].Play();
                        break;
                    case DoorState.Open:
                        animator.SetInteger(Hash_Door, 1);
                        audioSources[1].Play();
                        break;
                    case DoorState.Lock:

                        break;
                    default:
                        break;
                }
            }
        }
    }

    Animator animator;
    AudioSource[] audioSources; // 0 = Close, 1 = Open

    readonly int Hash_Door = Animator.StringToHash("DoorState");

    private void Awake()
    {
        animator =  GetComponent<Animator>();
        audioSources = GetComponents<AudioSource>();
    }

    public void OnInteract()
    {
        if (State == DoorState.Close)
        {
            State = DoorState.Open;
        }
        else
        {
            State = DoorState.Close;
        }
    }

    private bool IsAnimationPlaying()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime < 1f;
    }
}
