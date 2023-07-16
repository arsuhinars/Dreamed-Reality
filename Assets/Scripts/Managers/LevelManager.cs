using System;
using UnityEngine;

namespace DreamedReality.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [Serializable]
        private class SubLevel
        {
            public GameObject rootObject;
        }    

        public static LevelManager Instance { get; private set; } = null;

        [SerializeField] private int m_initialLevelIndex = 0;
        [SerializeField] private SubLevel[] m_subLevels;

        private int m_currLevelIdx;

        public void SwitchSubLevel(int subLevelIndex)
        {
            m_subLevels[m_currLevelIdx].rootObject.SetActive(false);
            m_subLevels[subLevelIndex].rootObject.SetActive(true);
            m_currLevelIdx = subLevelIndex;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Singleton already exists");
                Destroy(this);
            }
        }

        private void Start()
        {
            GameManager.Instance.OnStart += OnGameStart;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        private void OnGameStart()
        {
            for (int i = 0; i < m_subLevels.Length; ++i)
            {
                m_subLevels[i].rootObject.SetActive(false);
            }

            m_currLevelIdx = m_initialLevelIndex;
            m_subLevels[m_initialLevelIndex].rootObject.SetActive(true);
        }
    }
}
