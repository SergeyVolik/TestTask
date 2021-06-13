using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace TestTaskProject
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        [SerializeField]
        private CharacterController m_Controller;
        [SerializeField]
        private Transform m_MainCamera;

        [SerializeField]
        private float m_PlayerSpeed = 2.0f;
        [SerializeField]
        private float m_JumpHeight = 1.0f;
        private float m_GravityValue = -9.81f;

        [SerializeField]
        private float m_TurnSmoothTime = 0.1f;
        private float m_TurnSmoothVelocity;
        private bool m_GroundedPlayer;

        private Vector3 m_PlayerVelocity;
        // Update is called once per frame

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            m_GroundedPlayer = m_Controller.isGrounded;

            if (m_GroundedPlayer && m_PlayerVelocity.y < 0)
            {
                m_PlayerVelocity.y = 0f;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;



            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + m_MainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_TurnSmoothVelocity, m_TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                m_Controller.Move(moveDir.normalized * m_PlayerSpeed * Time.deltaTime);
            }

            if (Input.GetButtonDown("Jump") && m_GroundedPlayer)
            {
                //m_PlayerVelocity.y += Mathf.Sqrt(m_JumpHeight * -3.0f * m_GravityValue);
                Invoke("Jump", 0.1f);
                MessageBroker.Default.Publish(new MessagePlayerJump());

            }

            m_PlayerVelocity.y += m_GravityValue * Time.deltaTime;
            m_Controller.Move(m_PlayerVelocity * Time.deltaTime);


        }

        void Jump()
        {
            m_PlayerVelocity.y += Mathf.Sqrt(m_JumpHeight * -3.0f * m_GravityValue);
        }
        
    }
}
