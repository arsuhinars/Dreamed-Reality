using DreamedReality.Managers;
using DreamedReality.Tweeners;
using DreamedReality.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace DreamedReality.Entities
{
    public class StatefulEntity : AbstractStatefulEntity
    {
        [SerializeField] private bool m_initialState;
        [SerializeField]
        private SerializedInterface<IStatefulTweener> m_tweener;
        [Space]
        [SerializeField] private UnityEvent OnTurnOn;
        [SerializeField] private UnityEvent OnTurnOff;

        protected override void OnStateChange(bool state)
        {
            if (state)
            {
                m_tweener.Value?.PlayIn();
                OnTurnOn?.Invoke();
            }
            else
            {
                m_tweener.Value?.PlayOut();
                OnTurnOff?.Invoke();
            }
        }

        private void Start()
        {
            GameManager.Instance.OnStart += OnGameStart;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        private void OnGameStart()
        {
            State = m_initialState;
            m_tweener.Value?.Complete();
        }
    }
}
