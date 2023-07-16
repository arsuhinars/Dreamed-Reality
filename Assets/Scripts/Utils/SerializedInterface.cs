using System;
using UnityEngine;

namespace DreamedReality.Utils
{
    [Serializable]
    public class SerializedInterface<T> where T : class
    {
        public T Value
        {
            get
            {
                if (!m_isInitialized)
                {
                    Initialize();
                }

                return m_cachedValue;
            }
        }

        [SerializeField] private Component m_value;
        private bool m_isInitialized = false;
        private T m_cachedValue;

        private void Initialize()
        {
            if (m_value == null)
            {
                m_isInitialized = true;
                m_cachedValue = null;
                return;
            }

            m_cachedValue = m_value as T;
            if (m_cachedValue == null)
            {
                Debug.LogError("Value in SerializedInterface must inherit given interface");
            }

            m_isInitialized = true;
        }
    }
}
