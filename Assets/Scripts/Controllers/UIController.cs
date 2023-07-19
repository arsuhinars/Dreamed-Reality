using DreamedReality.Managers;
using DreamedReality.UI.Views;
using UnityEngine;

namespace DreamedReality.Controllers
{
    public class UIController : MonoBehaviour
    {
        private NoteReadView m_noteReadView;

        private void Start()
        {
            var gameManager = GameManager.Instance;
            gameManager.OnStart += OnStart;
            gameManager.OnEnd += OnEnd;
            gameManager.OnPause += OnPause;
            gameManager.OnResume += OnResume;
            gameManager.OnReadNote += OnReadNote;

            var noteReadView = UIManager.Instance.GetView("NoteReadView");
            m_noteReadView = noteReadView as NoteReadView;
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

        private void OnReadNote(string noteText)
        {
            UIManager.Instance.SetView("NoteReadView");
            m_noteReadView.Text = noteText;
        }
    }
}
