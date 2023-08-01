using DreamedReality.Managers;
using System.Collections;
using UnityEngine;

namespace DreamedReality.Controllers
{
    public class GameEndController : MonoBehaviour
    {
        [SerializeField] private float m_gameEndDelay;
        
        public void EndGame()
        {
            StartCoroutine(GameEndCoroutine());
        }

        private IEnumerator GameEndCoroutine()
        {
            yield return new WaitForSecondsRealtime(m_gameEndDelay);

            UIManager.Instance.ScreenFade.FadeIn(() =>
            {
                GameManager.Instance.EndGame(GameEndReason.PlayerWin);
            });
        }
    }
}
