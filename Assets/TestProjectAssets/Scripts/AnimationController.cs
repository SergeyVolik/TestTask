using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace TestTaskProject
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField]
        private CharacterController m_Controller;
        [SerializeField]
        Animator m_Animator;

        // Start is called before the first frame update
        // Update is called once per frame

        private void Awake()
        {
            MessageBroker.Default.Receive<MessageInteractAction>().Subscribe((msg) =>
            {
                m_Animator.SetTrigger(Constans.PlayerAnimator.INTERACT_TRIGGER);
            });

            MessageBroker.Default.Receive<MessagePlayerJump>().Subscribe((msg) =>
            {
                m_Animator.SetTrigger(Constans.PlayerAnimator.JUMP_TRIGGER);
            });

            StartCoroutine(IsFallSomeTimes());

        }
        void Update()
        {
            float horizontal = Mathf.Abs(Input.GetAxisRaw("Horizontal"));
            float vertical = Mathf.Abs(Input.GetAxisRaw("Vertical"));

            m_Animator.SetBool(Constans.PlayerAnimator.IS_RUN_BOOLEAN, horizontal + vertical > 0 ? true : false);
            m_Animator.SetBool(Constans.PlayerAnimator.IS_FALL_BOOLEAN, !grounded);

        }

        float t = 0;
        bool grounded = false;
        IEnumerator IsFallSomeTimes()
        {
            while (true)
            {
                if (m_Controller.isGrounded)
                    t = 0;
                t += Time.deltaTime;

                if (t > 0.1f)
                {
                    grounded = false;
                }
                else grounded = true;
                yield return null;
            }
        }

    }

}