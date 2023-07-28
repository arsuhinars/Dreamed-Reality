using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Controllers
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private SfxType m_sfxType;

        public void Play()
        {
            AudioManager.Instance.PlaySound(m_sfxType, transform.position);
        }
    }
}
