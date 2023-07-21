using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class CharacterEntity : MonoBehaviour, ISpawnableEntity
    {
        public Vector2 MoveVector { get => m_moveVec; set => m_moveVec = value; }
        public bool IsGrounded => m_isGrounded;
        public bool IsAlive => m_isAlive;

        [SerializeField] private CharacterSettings m_settings;

        private Rigidbody m_rb;
        private CapsuleCollider m_collider;
        private Animator m_animator;

        private Vector2 m_moveVec;
        private float m_moveTargetRot;
        private float m_moveRot;
        private float m_moveAngleSpeed;

        private bool m_isAlive = false;
        private bool m_isGrounded = false;
        private float m_lowerCapsuleY;
        private Vector3 m_averageGroundNormal;
        private float m_flyTime;

        private int m_isGroundedParamId;
        private int m_moveFactorParamId;
        private int m_onJumpParamId;
        private int m_flyTimeParamId;

        public void Spawn()
        {
            m_isAlive = true;
        }

        public void Kill()
        {
            m_isAlive = false;
        }

        public void TeleportTo(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);

            m_rb.position = position;
            m_rb.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;

            m_moveVec = Vector2.zero;
            m_moveRot = rotation.eulerAngles.y;
            m_moveTargetRot = m_moveRot;
            m_moveAngleSpeed = 0f;
        }

        public void Jump()
        {
            if (m_isGrounded)
            {
                m_rb.AddForce(Vector3.up * m_settings.jumpImpulse, ForceMode.Impulse);
                m_animator.SetTrigger(m_onJumpParamId);
            }
        }

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody>();
            m_collider = GetComponentInChildren<CapsuleCollider>();
            m_animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_lowerCapsuleY = m_collider.center.y;
            m_lowerCapsuleY -= m_collider.height * 0.5f - m_collider.radius;
            m_lowerCapsuleY -= m_collider.radius * Mathf.Cos(m_settings.maxSlopeAngle);

            m_isGroundedParamId = Animator.StringToHash("IsGrounded");
            m_moveFactorParamId = Animator.StringToHash("MovingFactor");
            m_onJumpParamId = Animator.StringToHash("OnJump");
            m_flyTimeParamId = Animator.StringToHash("FlyTime");
        }

        private void FixedUpdate()
        {
            if (!m_isAlive)
            {
                return;
            }

            GroundedRoutine();
            MovementRoutine();
            DragRoutine();
            AnimatorRoutine();
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
            if (m_isGrounded)
            {
                m_flyTime = 0f;
            }
            else
            {
                m_flyTime += Time.fixedDeltaTime;
            }

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
            if (!Mathf.Approximately(m_moveVec.sqrMagnitude, 0f) && m_isGrounded)
            {
                m_moveTargetRot = Mathf.Atan2(m_moveVec.x, m_moveVec.y) * Mathf.Rad2Deg;
            }

            m_moveRot = Mathf.SmoothDampAngle(
                m_moveRot, m_moveTargetRot, ref m_moveAngleSpeed, m_settings.rotationSmoothTime
            );

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

        private void AnimatorRoutine()
        {
            m_animator.SetBool(m_isGroundedParamId, m_isGrounded);
            m_animator.SetFloat(m_moveFactorParamId, m_moveVec.magnitude);
            m_animator.SetFloat(m_flyTimeParamId, m_flyTime);
        }
    }
}
