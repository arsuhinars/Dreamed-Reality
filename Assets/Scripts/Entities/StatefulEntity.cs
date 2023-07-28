using DreamedReality.Managers;
using DreamedReality.Tweeners;
using DreamedReality.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace DreamedReality.Entities
{
    public class StatefulEntity : AbstractStatefulEntity
    {
        protected override bool InitialState => m_initialState;

        [SerializeField] private bool m_initialState;
        [SerializeField]
        private SerializedInterface<IStatefulTweener> m_tweener;
        [SerializeField] private SfxType m_turnOnSfx;
        [SerializeField] private SfxType m_turnOffSfx;
        [Space]
        [SerializeField] private UnityEvent OnTurnOn;
        [SerializeField] private UnityEvent OnTurnOff;

        protected override void OnStateChange(bool state, bool isInitial)
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

            if (!isInitial)
            {
                AudioManager.Instance.PlaySound(state ? m_turnOnSfx : m_turnOffSfx, transform.position);
            }
            else
            {
                m_tweener.Value?.Complete();
            }
        }
    }
}
