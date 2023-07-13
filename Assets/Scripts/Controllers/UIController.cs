using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Controllers
{
    public class UIController : MonoBehaviour
    {
        private void Start()
        {
            var gameManager = GameManager.Instance;
            gameManager.OnStart += OnStart;
            gameManager.OnEnd += OnEnd;
            gameManager.OnPause += OnPause;
            gameManager.OnResume += OnResume;
        }
        
        private void OnDestroy()
        {
            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.OnStart += OnStart;
                gameManager.OnEnd += OnEnd;
                gameManager.OnPause += OnPause;
                gameManager.OnResume += OnResume;
            }
        }

        private void OnStart()
        {
            UIManager.Instance.SetView("GameView");
        }

        private void OnEnd(GameEndReason reason)
        {
            UIManager.Instance.SetView("GameEndView");
        }

        private void OnPause()
        {
            UIManager.Instance.SetView("PauseView");
        }

        private void OnResume()
        {
            UIManager.Instance.SetView("GameView");
        }
    }
}
