using UnityEngine;
/// <summary>
/// 모든 싱글톤 매니저들의 부모가 될 제네릭 클래스입니다.
/// 상속받는 클래스는 자동으로 싱글톤 패턴이 적용됩니다.
/// </summary>
/// <typeparam name="T">상속받을 클래스 타입</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();
    private static bool _shuttingDown = false;

    public static T Instance
    {
        get
        {
            if (_shuttingDown)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // 씬에 존재하는 인스턴스를 찾습니다.
                    _instance = (T)FindAnyObjectByType(typeof(T));

                    // 없다면 새로 생성합니다.
                    if (_instance == null)
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // 씬 전환 시 파괴되지 않도록 설정합니다.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            // 중복된 인스턴스가 있다면 파괴합니다.
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _shuttingDown = true;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _shuttingDown = true;
        }
    }
}
