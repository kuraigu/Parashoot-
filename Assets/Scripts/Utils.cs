using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreeMatrix
{
    public static class Utils
    {
        [System.Serializable]
        public class Range<T>
        {
            [SerializeField]
            private T _min;
            [SerializeField]
            private T _max;
            [SerializeField]
            private T _current;

            public T min
            { get { return _min; } set { _min = value; } }

            public T max
            { get { return _max; } set { _max = value; } }

            public T current
            { get { return _current; } set { _current = value; } }


            public Range(T min, T max)
            {
                _min = min;
                _max = max;
            }

            public Range<T> Set(T min, T max)
            {

                _min = min;
                _max = max;

                return this;
            }
        }


        public class GameObjectHelper
        {
            public static IEnumerator UnscaledTimeDestroyCoroutine(GameObject gameObject, float duration)
            {
                yield return new WaitForSecondsRealtime(duration);

                GameObject.Destroy(gameObject);
            }


            public static IEnumerator UnscaledTimeSetActiveCoroutine(GameObject gameObject, float duration, bool value)
            {
                yield return new WaitForSecondsRealtime(duration);

                if (gameObject != null)
                {
                    gameObject.SetActive(value);
                }
            }

            [System.Serializable]
            public class PrefabCache<T>
            {
                [SerializeField] private T _prefab;
                [SerializeField] private T _instance;

                public T prefab
                {get {return _prefab;}}

                public T instance 
                {get {return _instance;} set {_instance = value;}}
            }


            [System.Serializable]
            public class PrefabCacheList<T>
            {
                private T _prefab;
                private List<T> _instanceList = new List<T>();
            }
        }
    }
}
