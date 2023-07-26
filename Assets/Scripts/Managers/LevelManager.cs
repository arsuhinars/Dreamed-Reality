using DreamedReality.Scriptables;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DreamedReality.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [Serializable]
        private class SubLevel
        {
            public GameObject rootObject;
        }

        public event Action OnSceneStartedLoading;
        public event Action OnSceneFinishedLoading;

        public int CurrentLevelIndex => m_currLevelIdx;
        public int CurrentSubLevelIndex => m_currSubLevelIdx;

        public static LevelManager Instance { get; private set; } = null;

        [SerializeField] private LevelsSettings m_settings;
        [Space]
        [SerializeField] private int m_initialSubLevelIndex = 0;
        [SerializeField] private SubLevel[] m_subLevels;

        private int m_currLevelIdx;
        private int m_currSubLevelIdx;

        public void LoadLevel(int levelIndex)
        {
            LoadScene(m_settings.levels[levelIndex].sceneName);
        }

        public void LoadMainMenu()
        {
            LoadScene(m_settings.mainMenuSceneName);
        }

        public void SwitchSubLevel(int subLevelIndex)
        {
            if (m_currLevelIdx == -1)
            {
                Debug.LogError("Unable to change sublevel, because level is not loaded");
                return;
            }

            m_subLevels[m_currSubLevelIdx].rootObject.SetActive(false);
            m_subLevels[subLevelIndex].rootObject.SetActive(true);
            m_currSubLevelIdx = subLevelIndex;
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
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart += OnGameStart;
            }

            m_currSubLevelIdx = -1;
            m_currLevelIdx = -1;

            var activeSceneName = SceneManager.GetActiveScene().name;
            for (int i = 0; i < m_settings.levels.Length; ++i)
            {
                var level = m_settings.levels[i];
                if (level.sceneName == activeSceneName)
                {
                    m_currLevelIdx = i;
                    break;
                }
            }
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

            m_currSubLevelIdx = m_initialSubLevelIndex;
            m_subLevels[m_initialSubLevelIndex].rootObject.SetActive(true);
        }

        private void LoadScene(string sceneName)
        {
            var op = SceneManager.LoadSceneAsync(sceneName);

            OnSceneStartedLoading?.Invoke();

            StartCoroutine(LoadingCoroutine(op));
        }

        private IEnumerator LoadingCoroutine(AsyncOperation operation)
        {
            Time.timeScale = 0f;
            operation.allowSceneActivation = false;

            float startTime = Time.realtimeSinceStartup;
            yield return new WaitUntil(() => operation.progress >= 0.9f);

            float delay = Time.realtimeSinceStartup - startTime;
            yield return new WaitForSecondsRealtime(
                Mathf.Max(m_settings.minLoadingTime - delay, 0f)
            );

            operation.allowSceneActivation = true;
            yield return new WaitUntil(() => operation.isDone);

            Time.timeScale = 1f;
            OnSceneFinishedLoading?.Invoke();
        }
    }
}
