using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Controllers
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private int m_defaultFrameRate = 60;

        private void Start()
        {
            Application.targetFrameRate = m_defaultFrameRate;
            Time.timeScale = 1f;

            LevelManager.Instance.OnSceneStartedLoading += OnSceneStartedLoading;
        }

        private void OnDestroy()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnSceneStartedLoading -= OnSceneStartedLoading;
            }
        }

        private void OnSceneStartedLoading()
        {
            UIManager.Instance.SetView("LoadingView");
        }
    }
}
