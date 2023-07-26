using System;
using System.Collections;
using UnityEngine;

namespace DreamedReality.Managers
{
    [Serializable]
    public struct PlayerProgress
    {
        public int maxLevelIndex;
    }

    public class ProgressManager : MonoBehaviour
    {
        public const string PLAYER_PROGRESS_PREFS_KEY = "PlayerProgress";

        public PlayerProgress PlayerProgress => m_progress;

        public static ProgressManager Instance { get; private set; } = null;

        private PlayerProgress m_progress;

        public void ClearProgress()
        {
            m_progress = new PlayerProgress()
            {
                maxLevelIndex = -1,
            };

            SaveProgress();
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

        private IEnumerator Start()
        {
            LoadProgress();
            yield return null;
            UpdateProgress();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void LoadProgress()
        {
            var defaultProgressValue = JsonUtility.ToJson(
                new PlayerProgress { maxLevelIndex = -1 }
            );

            m_progress = JsonUtility.FromJson<PlayerProgress>(
                PlayerPrefs.GetString(
                    PLAYER_PROGRESS_PREFS_KEY, defaultProgressValue
                )
            );
        }

        private void SaveProgress()
        {
            PlayerPrefs.SetString(
                PLAYER_PROGRESS_PREFS_KEY, JsonUtility.ToJson(m_progress)
            );
        }

        private void UpdateProgress()
        {
            var currLevelIdx = LevelManager.Instance.CurrentLevelIndex;
            if (currLevelIdx == -1)
            {
                return;
            }

            if (currLevelIdx > m_progress.maxLevelIndex)
            {
                m_progress.maxLevelIndex = currLevelIdx;
                SaveProgress();
            }
        }
    }
}
