using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Controllers
{
    public class MainMenuUIController : MonoBehaviour
    {
        private void Start()
        {
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
