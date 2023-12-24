using UnityEngine;

namespace TreasureHunter.Utilities
{
    public abstract class MonoSingleton : MonoBehaviour
    {
        public abstract void Destroy();
    }

    public class MonoSingleton<T> : MonoSingleton where T : MonoBehaviour
    {
        private static bool _isDestroyed = false;
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_isDestroyed)
                {
                    return null;
                }

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogWarning("has duplicated singleton");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = _instance.GetType().Name;
                    }
                }

                return _instance;
            }

            private set { _instance = value; }
        }

        public static bool HasInstance { get { return _instance != null; } }

        public virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this);
            _isDestroyed = false;
        }

        public virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _isDestroyed = true;
            }
        }

        public override void Destroy()
        {
            _instance = null;
            Destroy(gameObject);
        }
    }
}