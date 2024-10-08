using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// 이미 종료처리에 했는지 확인하는 변수
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// 싱글톤의 객체
    /// </summary>
    private static T instance;

    /// <summary>
    /// 싱글톤의 객체를 읽기 위한 프로퍼티
    /// </summary>
    public static T Instance
    {
        get
        {
            // 이미 종료 처리 했을 때
            if(isShutDown)
            {
                // 경고 출력
                Debug.LogWarning($"{typeof(T).Name} 싱글톤은 이미 삭제중이다.");
                return null;
            }
            if(instance == null)
            {
                if(FindObjectOfType<T>(true) == null)
                {
                    GameObject obj = new GameObject();  // 오브젝트 생성
                    obj.name = $"{typeof(T).Name} Singleton";   // 이름 추가
                    instance = obj.AddComponent<T>();   // 싱글톤 객체 추가
                    DontDestroyOnLoad(instance.gameObject); // 씬이 사라질 때 GameObject가 삭제되지 않도록 하는 함수
                }
                else
                {
                    Debug.Log($"FindObjectOfType<T>(true) = {FindObjectOfType<T>(true)} 가끔 여기로 빠지는것들이 있다.");
                    Debug.Log("여기로 빠지면 일단 비활성화된 오브젝트에 싱글톤을 추가하지 않았나 확인");
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// 해당 컴포넌트가 사용되는 첫씬에서 한번 Awake가 발동된다.
    /// </summary>
    protected virtual void Awake()
    {
        if(instance == null) // 씬에 배치되어있는 첫번째 싱글톤 GameObject
        {
            instance = this as T;

            DontDestroyOnLoad(instance.gameObject); // 씬이 사라질 때 GameObject가 삭제되지 않도록 하는 함수
        }
        else // 첫번째 싱글톤 게임 오브젝트가 만들어진 이후에 만들어진 싱글톤 GameObject
        {
            if(instance != this) // 같은 객체일수도 있어서 아닐경우만 처리
            {
                Destroy(this.gameObject);   // 첫번째 싱글톤과 다른 객체면 삭제
            }
        }
    }

    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 프로그램이 종료되었을 때 실행되는 함수
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true;
    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init(scene, mode);
    }

    protected virtual void Init(Scene scene, LoadSceneMode mode)
    {

    }
}
