using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CameraSettings m_settings;
        [SerializeField] private Transform m_lookTarget;

        private Vector3 m_posOffset;
        private Vector3 m_velocity;

        public void ResetPosition()
        {
            m_velocity = Vector3.zero;
            transform.position = m_lookTarget.position + m_posOffset;
        }

        private void Start()
        {
            transform.rotation = Quaternion.LookRotation(m_settings.lookDirection);
            m_posOffset = -m_settings.lookDirection.normalized * m_settings.lookDistance;
            m_posOffset += m_settings.lookCenterOffset;
        }

        private void Update()
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                m_lookTarget.position + m_posOffset,
                ref m_velocity,
                m_settings.moveSmoothTime
            );
        }
    }
}
