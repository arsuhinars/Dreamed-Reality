using DreamedReality.Inputs;
using DreamedReality.Managers;
using DreamedReality.UI.Views;
using UnityEngine;
using UnityEngine.Localization;

namespace DreamedReality.Controllers
{
    public class GameUIController : MonoBehaviour
    {
        private MobileInputProvider m_inputProvider;
        private NoteReadView m_noteReadView;

        private void Start()
        {
            var gameManager = GameManager.Instance;
            gameManager.OnStart += OnStart;
            gameManager.OnEnd += OnEnd;
            gameManager.OnPause += OnPause;
            gameManager.OnResume += OnResume;
            gameManager.OnReadNote += OnReadNote;

            LevelManager.Instance.OnSceneStartedLoading += OnSceneStartedLoading;

            var noteReadView = UIManager.Instance.GetView("NoteReadView");
            m_noteReadView = noteReadView as NoteReadView;

            var gameView = UIManager.Instance.GetView("GameView") as GameView;
            m_inputProvider = gameView.CreateMobileInputProvider();

            GameInputManager.Instance.RegisterInputProvider(m_inputProvider);
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
                gameManager.OnReadNote -= OnReadNote;
            }

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnSceneStartedLoading -= OnSceneStartedLoading;
            }

            if (GameInputManager.Instance != null)
            {
                GameInputManager.Instance.UnregisterInputProvider(m_inputProvider);
            }
        }

        private void OnStart()
        {
            UIManager.Instance.SetView("GameView");
            UIManager.Instance.ScreenFade.StopFade();
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

        private void OnReadNote(LocalizedString noteText)
        {
            UIManager.Instance.SetView("NoteReadView");
            m_noteReadView.Text = noteText;
        }

        private void OnSceneStartedLoading()
        {
            UIManager.Instance.SetView("LoadingView");
        }
    }
}
