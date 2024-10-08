using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Inst { get { Init(); return instance; } }

    Player player;
    Player_Data player_Data;

    public static Player Player => Inst.player;
    public static Player_Data Player_Data => Inst.player_Data;

    private void Start()
    {
        player = FindObjectOfType<Player>(true);
    }

    private void Awake()
    {
        Init();

        if(instance != this)
        {
            Destroy(this.gameObject);
        }

        player_Data = FindAnyObjectByType<Player_Data>();
    }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("GameManager");
            if(go == null)
            {
                go = new GameObject { name = $"GameManager" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<GameManager>();
        }
    }
}
