using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Inst { get { Init(); return instance; } }

    Player player;
    Player_Data data;
    CrosshairSwitcher cross;

    public static Player Player => Inst.player;
    public static Player_Data Data => Inst.data;
    public static CrosshairSwitcher Cross => Inst.cross;

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

        data = GetComponent<Player_Data>();
        cross = FindAnyObjectByType<CrosshairSwitcher>();
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

    public void ChangeCursor(bool isChange)
    {

    }
}
