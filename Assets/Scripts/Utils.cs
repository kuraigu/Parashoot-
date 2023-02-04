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
    }
}
