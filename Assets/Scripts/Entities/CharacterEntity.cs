using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterEntity : MonoBehaviour, ISpawnableEntity
    {
        public Vector2 MoveVector { get => m_moveVec; set => m_moveVec = value; }
        public bool IsGrounded => m_isGrounded;
        public bool IsAlive => m_isAlive;

        [SerializeField] private CharacterSettings m_settings;

        private Rigidbody m_rb;
        private CapsuleCollider m_collider;

        private Vector2 m_moveVec;
        private float m_moveRot;
        private float m_moveAngleSpeed;

        private bool m_isAlive = false;
        private bool m_isGrounded = false;
        private float m_lowerCapsuleY;
        private Vector3 m_averageGroundNormal;

        public void Spawn()
        {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;

            m_moveVec = Vector2.zero;
            m_moveRot = transform.rotation.eulerAngles.y;
            m_moveAngleSpeed = 0f;

            m_isAlive = true;
        }

        public void Kill()
        {
            m_isAlive = false;
        }

        public void Jump()
        {
            if (m_isGrounded)
            {
                m_rb.AddForce(Vector3.up * m_settings.jumpImpulse, ForceMode.Impulse);
            }
        }

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody>();
            m_collider = GetComponentInChildren<CapsuleCollider>();
        }

        private void Start()
        {
            m_lowerCapsuleY = m_collider.center.y - m_collider.height * 0.5f + m_collider.radius;
        }

        private void FixedUpdate()
        {
            GroundedRoutine();
            MovementRoutine();
            DragRoutine();
        }

        private void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            HandleCollision(collision);
        }

        private void HandleCollision(Collision collision)
        {
            for (int i = 0; i < collision.contactCount; ++i)
            {
                var p = collision.GetContact(i);
                if (m_collider.transform.InverseTransformPoint(p.point).y >= m_lowerCapsuleY)
                {
                    continue;
                }

                m_averageGroundNormal += collision.GetContact(i).normal;
            }
        }

        private void GroundedRoutine()
        {
            if (!Mathf.Approximately(m_averageGroundNormal.sqrMagnitude, 0f))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, m_averageGroundNormal);
                m_isGrounded = slopeAngle < m_settings.maxSlopeAngle;
            }
            else
            {
                m_isGrounded = false;
            }

            m_averageGroundNormal = Vector3.zero;
        }

        private void MovementRoutine()
        {
            if (!Mathf.Approximately(m_moveVec.sqrMagnitude, 0f))
            {
                float targetRot = Mathf.Atan2(m_moveVec.y, m_moveVec.x) * Mathf.Rad2Deg;
                m_moveRot = Mathf.SmoothDampAngle(
                    m_moveRot, targetRot, ref m_moveAngleSpeed, m_settings.rotationSmoothTime
                );
            }

            float forceMagnitude = m_isGrounded ? m_settings.maxMoveForce : m_settings.flyMoveForce;

            m_rb.rotation = Quaternion.AngleAxis(m_moveRot, Vector3.up);
            m_rb.AddForce(
                new Vector3(
                    m_moveVec.x, 0f, m_moveVec.y
                ) * forceMagnitude
            );
        }

        private void DragRoutine()
        {
            var drag = Vector3.Min(
                m_isGrounded ? m_settings.groundDrag : m_settings.airDrag,
                Vector3.one / Time.fixedDeltaTime
            );

            m_rb.AddForce(
                Vector3.Scale(-m_rb.velocity, drag), ForceMode.Acceleration
            );

            if (m_isGrounded)
            {
                var gravityDrag = Vector3.Project(Physics.gravity, m_averageGroundNormal);
                gravityDrag -= Physics.gravity;
                m_rb.AddForce(gravityDrag, ForceMode.Acceleration);
            }
        }
    }
}
