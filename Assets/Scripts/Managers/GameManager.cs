using System;
using System.Collections;
using UnityEngine;

namespace DreamedReality.Managers
{
    public enum GameEndReason
    {
        PlayerWin, PlayerDied
    }

    public enum GameState
    {
        None, Started, Ended, Paused
    }

    public class GameManager : MonoBehaviour
    {
        public event Action OnStart;
        public event Action<GameEndReason> OnEnd;
        public event Action OnPause;
        public event Action OnResume;

        public static GameManager Instance { get; private set; } = null;

        public GameState State => m_state;
        public bool IsStarted => m_state == GameState.Started;
        public bool IsPaused => m_state == GameState.Paused;

        private GameState m_state = GameState.None;

        public void StartGame()
        {
            m_state = GameState.Started;
            Time.timeScale = 1f;

            OnStart?.Invoke();
        }

        public void EndGame(GameEndReason reason)
        {
            if (m_state != GameState.Started)
            {
                return;
            }

            m_state = GameState.Ended;
            Time.timeScale = 1f;

            OnEnd?.Invoke(reason);
        }

        public void PauseGame()
        {
            if (m_state != GameState.Started)
            {
                return;
            }

            m_state = GameState.Paused;
            Time.timeScale = 0f;

            OnPause?.Invoke();
        }

        public void ResumeGame()
        {
            if (m_state != GameState.Paused)
            {
                return;
            }

            m_state = GameState.Started;
            Time.timeScale = 1f;

            OnResume?.Invoke();
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
            yield return null;
            StartGame();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
