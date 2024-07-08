using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class ThirdPersonCharacter : MonoBehaviour
    {
        [SerializeField] float m_MovingTurnSpeed = 360;
        [SerializeField] float m_StationaryTurnSpeed = 180;
        [SerializeField] float m_JumpPower = 12f;
        [Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
        [SerializeField] float m_RunCycleLegOffset = 0.2f; // specific to the character in sample assets, will need to be modified to work with others
        [SerializeField] float m_MoveSpeedMultiplier = 1f;
        [SerializeField] float m_AnimSpeedMultiplier = 1f;
        [SerializeField] float m_GroundCheckDistance = 0.1f;

        Rigidbody m_Rigidbody;
        Animator m_Animator;
        bool m_IsGrounded;
        float m_OrigGroundCheckDistance;
        const float k_Half = 0.5f;
        float m_ForwardAmount;
        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;
        bool m_Crouching;
        public HealthManager healthManager;
        private Animator myAnimation;

        public CheckpointManager checkpointManager; // Referência ao CheckpointManager

        public Transform cameraTransform; // Referência à câmera

        [SerializeField] int maxJumps = 2; // Número máximo de saltos permitidos
        int jumpCount = 0; // Contador de saltos realizados

        public static bool IsShopOpen = false; // Adicionado para rastrear se a loja está aberta
        public static bool IsPaused = false; // Adicionado para rastrear se o menu de pausa está aberto

        bool doubleJumpEnabled = false;

        void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;

            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            m_OrigGroundCheckDistance = m_GroundCheckDistance;

            healthManager = FindObjectOfType<HealthManager>();
            myAnimation = GetComponent<Animator>();
            checkpointManager = FindObjectOfType<CheckpointManager>();

            if (SceneManager.GetActiveScene().name == "Nivel1 endscene")
            {
                doubleJumpEnabled = true;
            }
            else
            {
                doubleJumpEnabled = false;
            }

            LockCursor();
        }

        void Update()
        {
            if (!IsShopOpen && !IsPaused) // Apenas bloqueia o cursor se a loja ou o menu de pausa não estiverem abertos
            {
                LockCursor();
            }
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Move(Vector3 move, bool crouch, bool jump)
        {
            if (IsShopOpen || IsPaused) return; // Impede movimento quando a loja ou o menu de pausa estão abertos

            if (move.magnitude > 1f) move.Normalize();
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_ForwardAmount = move.z;

            if (m_IsGrounded)
            {
                HandleGroundedMovement(crouch, jump);
            }
            else
            {
                HandleAirborneMovement(jump);
            }

            ScaleCapsuleForCrouching(crouch);
            PreventStandingInLowHeadroom();
            UpdateAnimator(move);

            Vector3 targetMove = transform.forward * m_ForwardAmount * m_MoveSpeedMultiplier + transform.right * move.x * m_MoveSpeedMultiplier;
            m_Rigidbody.velocity = new Vector3(targetMove.x, m_Rigidbody.velocity.y, targetMove.z);

            if (cameraTransform != null)
            {
                Vector3 cameraForward = cameraTransform.forward;
                cameraForward.y = 0;
                if (cameraForward.sqrMagnitude > 0.0f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_StationaryTurnSpeed);
                }
            }
        }

        void ScaleCapsuleForCrouching(bool crouch)
        {
            if (m_IsGrounded && crouch)
            {
                if (m_Crouching) return;
                m_Capsule.height = m_Capsule.height / 2f;
                m_Capsule.center = m_Capsule.center / 2f;
                m_Crouching = true;
            }
            else
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength))
                {
                    m_Crouching = true;
                    return;
                }
                m_Capsule.height = m_CapsuleHeight;
                m_Capsule.center = m_CapsuleCenter;
                m_Crouching = false;
            }
        }

        void PreventStandingInLowHeadroom()
        {
            if (!m_Crouching)
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength))
                {
                    m_Crouching = true;
                }
            }
        }

        void UpdateAnimator(Vector3 move)
        {
            m_Animator.SetFloat("Forward", move.magnitude, 0.1f, Time.deltaTime);
            m_Animator.SetBool("Crouch", m_Crouching);
            m_Animator.SetBool("OnGround", m_IsGrounded);
            if (!m_IsGrounded)
            {
                m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
            }

            float runCycle = Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
            if (m_IsGrounded)
            {
                m_Animator.SetFloat("JumpLeg", jumpLeg);
            }

            if (m_IsGrounded && move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                m_Animator.speed = 1;
            }
        }

        void HandleAirborneMovement(bool jump)
        {
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_Rigidbody.AddForce(extraGravityForce);

            m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;

            if (doubleJumpEnabled && jump && jumpCount < maxJumps)
            {
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                m_IsGrounded = false;
                m_Animator.applyRootMotion = false;
                jumpCount++;

                if (jumpCount == 2)
                {
                    m_Animator.SetTrigger("DoubleJump");
                }
            }
        }

        void HandleGroundedMovement(bool crouch, bool jump)
        {
            jumpCount = 0;

            if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                m_IsGrounded = false;
                m_Animator.applyRootMotion = false;
                m_GroundCheckDistance = 0.1f;
                jumpCount++;
            }
        }

        public void OnAnimatorMove()
        {
            if (m_IsGrounded && Time.deltaTime > 0)
            {
                Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
                v.y = m_Rigidbody.velocity.y;
                m_Rigidbody.velocity = v;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                myAnimation.SetTrigger("Hit0");
                healthManager.LoseHealth(10);
            }
            else if (collision.gameObject.tag == "EnemyRocket")
            {
                myAnimation.SetTrigger("Hit0");
                healthManager.LoseHealth(40);
            }
            else if (collision.gameObject.tag == "DemoEnd")
            {
                SceneManager.LoadScene("Creditos");
            }
            else if (collision.gameObject.tag == "LimitGround"){
                myAnimation.SetTrigger("Hit0");
                healthManager.LoseHealth(120);
            }
        }

        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                m_GroundNormal = hitInfo.normal;
                m_IsGrounded = true;
                m_Animator.applyRootMotion = true;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundNormal = Vector3.up;
                m_Animator.applyRootMotion = false;
            }
        }

        public void Die()
        {
            Respawn();
        }

        public void Respawn()
        {
            if (checkpointManager != null)
            {
                Vector3 respawnPosition = checkpointManager.GetRespawnPosition();
                transform.position = respawnPosition;
                healthManager.ResetHealth();
            }
            else
            {
                Debug.LogError("CheckpointManager não definido! Verifique se você atribuiu o objeto CheckpointManager ao script ThirdPersonCharacter.");
            }
        }

        public void MoveToPosition(Vector3 position, Quaternion rotation)
        {
            m_Rigidbody.velocity = Vector3.zero; // Parar qualquer movimento existente
            m_Rigidbody.angularVelocity = Vector3.zero; // Parar qualquer rotação existente
            transform.position = position;
            transform.rotation = rotation;

            // Garanta que o Rigidbody e o Animator sejam atualizados corretamente
            m_Rigidbody.MovePosition(position);
            m_Rigidbody.MoveRotation(rotation);
        }
    }
}
